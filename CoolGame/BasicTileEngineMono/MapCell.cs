using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicTile
{
    /// <summary>
    /// MapCell (itself managing an area of the map that represents a tile area, but not necessary a single tile)
    /// </summary>
    public class MapCell : PathNode
    {
        //list of tile ids can stack any number of tile images on the same space
        public List<int> BaseTiles = new List<int>();
        //support to store tiles that are elevated above the base level
        public List<int> HeightTiles = new List<int>();
        //support to store tiles that are of multi-size tiles <tileID, cellOffsetX, cellOffsetY>
        public List<Tuple<int, int, int, int>> MultiSizeTiles = new List<Tuple<int, int, int, int>>();
        //support to allow place tiles on top of elevated map areas created by HeightTiles list -
        public List<int> TopperTiles = new List<int>();

        //to specify walkable and unwalkable of a map cell
        public bool Walkable 
        {
            get { return base.IsReachable; }
            set { base.IsReachable = value; } 
        }

        //to specify a cell as a slope in some direction (8 directions: 0 to 7)
        //reference: http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:9
        /// Slope values
        ///  no slope : -1
        ///  SW -> NE : 0    
        ///  SE -> NW : 1
        ///  NW -> SE : 2
        ///  NE -> SW : 3
        ///  S  -> N : 4
        ///  N  -> S : 5
        ///  E  -> W : 6
        ///  W  -> E : 7
        ///  TODO: enums slope values for better understandability
        public int SlopeMap { get; set; }

        public int TileID
        {
            //assume that we are asking for the first base tile if there's more than one
            //other wise return 0
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
            
            //make sure there is at least 1 tile and set it to value,
            //otherwise add it
            set
            {
                if (BaseTiles.Count > 0)
                    BaseTiles[0] = value;
                else
                    AddBaseTile(value);
            }
        }

        public MapCell(int tileID)
        {
            TileID = tileID;
            Walkable = true;
            SlopeMap = -1;  //no slope data associated by default
        }

        #region METHODS
        public void AddBaseTile(int tileID)
        {
            //TODO: set unwalkable for Tile ID range
            BaseTiles.Add(tileID);
        }
        public void RemoveBaseTile()
        {
            RemoveTile(ref BaseTiles);
        }
        public void AddHeightTile(int tileID)
        {
            //TODO: set slope map for Tile ID range that has slopes
            HeightTiles.Add(tileID);
        }
        public void RemoveHeightTile()
        {
            RemoveTile(ref HeightTiles);
        }
        public void AddTopperTile(int tileID)
        {
            TopperTiles.Add(tileID);
        }
        public void RemoveTopperTile()
        {
            RemoveTile(ref TopperTiles);
        }
        private void RemoveTile(ref List<int> list)
        {
            if (list.Count() > 0)
                list.RemoveAt(list.Count() - 1);
        }

        public void AddMultiSizeTile(int tileID, int CellOffsetX, int CellOffsetY, int CellOffsetZ)
        {
            MultiSizeTiles.Add(new Tuple<int, int, int,int>(tileID, CellOffsetX, CellOffsetY, CellOffsetZ));
        }
        public void RemoveMultiSizeTiles()
        {
            MultiSizeTiles.RemoveRange(0, MultiSizeTiles.Count());
        }
        #endregion

        //determines if Map Cell is walkable depending on the type of object
        public override bool IsWalkable(object obj)
        {
            return base.IsReachable;
        }
    }
}
