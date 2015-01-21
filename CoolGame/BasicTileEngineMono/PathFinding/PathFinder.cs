using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BasicTileEngineMono
{
    /// <summary>
    /// Basic Implementation of Path Finding using A* Search
    /// References:
    /// http://www.policyalmanac.org/games/aStarTutorial.htm
    /// </summary>
    public class PathFinder
    {
        public List<PathNode> OpenList = new List<PathNode>();
        public List<PathNode> ClosedList = new List<PathNode>();

        public PathFinder(TileMap map)
        {
            for (int y = 0; y < map.MapHeight; y++)
                for (int x = 0; x < map.MapWidth; x++)
                    map.GetMapCell(x, y).InitCostFunction();
        }

        public bool Search(int StartX, int StartY, int EndX, int EndY, TileMap map)
        {
            //init open and closed list
            OpenList.Clear();
            ClosedList.Clear();

            //get start node
            PathNode startNode = map.GetMapCell(StartX, StartY);
            startNode.X = StartX;
            startNode.Y = StartY;
            startNode.parentNode = null;

            //add the starting node to the open list, if this is not walkable, fail search
            if (startNode.IsWalkable(null))
                OpenList.Add(startNode);
            else
                return false;

            PathNode endNode = map.GetMapCell(EndX, EndY);
            endNode.X = EndX;
            endNode.Y = EndY;

            //if end node is not walkable(reachable by object), fail search
            if (!endNode.IsWalkable(null))
                return false;

            while (OpenList.Count() > 0)
            {
                PathNode currentNode = GetOpenListMinFNode();
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                if (currentNode.X == EndX && currentNode.Y == EndY)
                    break;

                SearchAdjacentNodes(currentNode, EndX, EndY, map);
            }

            return true;
        }

        /// <summary>
        /// Search in 8 directions of staggered isometric coordinate system
        /// </summary>
        /// <param name="curNode"></param>
        /// <param name="EndX"></param>
        /// <param name="EndY"></param>
        /// <param name="map"></param>
        private void SearchAdjacentNodes(PathNode curNode, int EndX, int EndY, TileMap map)
        {
            //TODO: maybe better way to write this. need to account for even and odd rows
            //curNode is odd row, apply a certain offset
            if(curNode.Y % 2 == 1)
            {
                SearchNode(curNode, curNode.X, curNode.Y - 2, EndX, EndY, map); //N
                SearchNode(curNode, curNode.X + 1, curNode.Y - 1, EndX, EndY, map); //NE
                SearchNode(curNode, curNode.X + 1, curNode.Y, EndX, EndY, map); //E
                SearchNode(curNode, curNode.X + 1, curNode.Y + 1, EndX, EndY, map); //SE
                SearchNode(curNode, curNode.X, curNode.Y + 2, EndX, EndY, map); //S
                SearchNode(curNode, curNode.X, curNode.Y + 1, EndX, EndY, map); //SW
                SearchNode(curNode, curNode.X - 1, curNode.Y, EndX, EndY, map); //W
                SearchNode(curNode, curNode.X, curNode.Y - 1, EndX, EndY, map); //NW
            }
            //even row apply even offset
            else
            {
                SearchNode(curNode, curNode.X, curNode.Y - 2, EndX, EndY, map); //N
                SearchNode(curNode, curNode.X, curNode.Y - 1, EndX, EndY, map); //NE
                SearchNode(curNode, curNode.X + 1, curNode.Y, EndX, EndY, map); //E
                SearchNode(curNode, curNode.X, curNode.Y + 1, EndX, EndY, map); //SE
                SearchNode(curNode, curNode.X, curNode.Y + 2, EndX, EndY, map); //S
                SearchNode(curNode, curNode.X - 1, curNode.Y + 1, EndX, EndY, map); //SW
                SearchNode(curNode, curNode.X - 1, curNode.Y, EndX, EndY, map); //W
                SearchNode(curNode, curNode.X - 1, curNode.Y - 1, EndX, EndY, map); //NW
            }

            
        }
        //search node
        private void SearchNode(PathNode curNode,int X, int Y,int EndX, int EndY, TileMap map)
        {
            //if out of bounds, do not search
            if (X < 0 || Y < 0 || X >= map.MapWidth || Y >= map.MapHeight)
                return; 

            PathNode searchNode = map.GetMapCell(X, Y);
            searchNode.X = X;
            searchNode.Y = Y;

            if(searchNode.IsWalkable(null) &&
                !ClosedList.Contains(searchNode))
            {
                //check if openlist does not contain node
                if (!OpenList.Contains(searchNode))
                {
                    searchNode.SetParentNode(curNode);

                    //Record F, G and H
                    //G = cost of currentNode + cost from currentNode(Parent) to searchNode
                    searchNode.G = curNode.G + PathNode.CalculateAdjCost(curNode.X, curNode.Y, X, Y);
                    searchNode.H = TileMap.L0TileDistance(X, Y, EndX, EndY);
                    searchNode.F = searchNode.G + searchNode.H;

                    OpenList.Add(searchNode);
                }
                else
                {
                    //if the path to the searchNode is better than going through the parent, then to the adjacent node
                    //then change the parent to the current square, reclculate G and F of square
                    int PathG = curNode.G + PathNode.CalculateAdjCost(curNode.X, curNode.Y, X, Y);
                    if (searchNode.G > PathG)
                    {
                        searchNode.SetParentNode(curNode);

                        //G = cost of currentNode + cost from currentNode(Parent) to searchNode
                        searchNode.G = PathG;
                        searchNode.F = searchNode.G + searchNode.H;
                    }
                }
            }
        }

        private PathNode GetOpenListMinFNode()
        {
            PathNode minNode = null;
            int compareValue = int.MaxValue;
            foreach(PathNode n in OpenList)
            {
                int CurrentValue = n.F;
                if (CurrentValue <= compareValue)
                {
                    compareValue = CurrentValue;
                    minNode = n;
                }
            }
            return minNode;
        }

        //TODO: this is the reuslts backwards as a string. Need another Func to return a new List of coordinates as the Path Result.
        public string ResultStringBackWards()
        {
            PathNode Current = ClosedList.Last();

            StringBuilder sb = new StringBuilder();

            while(Current != null)
            {
                sb.Append(string.Format("({0},{1})", Current.X, Current.Y));
                Current = Current.parentNode;
            }

            return sb.ToString();
        }

        public List<PathNode> PathResult()
        {
            List<PathNode> searchresult = new List<PathNode>();

            PathNode Current = ClosedList.Last();
            while (Current != null)
            {
                searchresult.Add(Current);
                Current = Current.parentNode;
            }

            searchresult.Reverse();

            return searchresult;
        }
    }

    public abstract class PathNode :GameObject,IPathNode<Object>
    {
        public PathNode parentNode { get; set; }

        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsReachable { get; set; }

        public abstract bool IsWalkable(Object obj);


        public void SetParentNode(PathNode parent)
        {
            parentNode = parent;
        }

        //Resets all cost to zero
        public void InitCostFunction()
        {
            this.F = 0;
            this.G = 0;
            this.H = 0;
        }

        /// <summary>
        /// Calculates cost to get to adjacent cell (only works for staggered isometric map)
        /// returns correct cost ONLY for adjacent cell
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <returns></returns>
        public static int CalculateAdjCost(int X1, int Y1, int X2, int Y2)
        {
            int DiffX = Math.Abs(X1 - X2);
            int DiffY = Math.Abs(Y1 - Y2);

            //diagonal move for an isometric staggered
            if (DiffY > 1)
                return 14;
            //diagonal move
            else if (DiffX == 1 && DiffY == 0)
                return 14;
            else
                return 10;
        }
    }

    public interface IPathNode<T>
    {
        bool IsWalkable(T obj);
    }
}
