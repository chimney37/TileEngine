﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BasicTileEngineMono.Components;

namespace BasicTileEngineMono
{
    /// <summary>
    /// A Map made of tiles is created and managed
    /// 
    /// References: 
    /// Isometric picking : http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:7
    /// </summary>
    public class TileMap
    {
        #region PROPERTIES
        //manage a mouse map for isometric picking
        private Texture2D mouseMap;
        //manage slopes on map
        private Texture2D slopeMaps;

        public List<MapRow> Rows = new List<MapRow>();


        //Map Width/Height : the size of the entire map
        public int MapWidth = 25;
        public int MapHeight = 64;

        //manage multisize Tile objects
        public delegate void AddMultiSizeDelegate(int row, int column, int height);

        //manage a list of tile logical objects managed by IDs
        protected Dictionary<int, GameLogicalObject> ObjectDictionary = new Dictionary<int, GameLogicalObject>();
        Dictionary<int, AddMultiSizeDelegate> MultiTileAdderDictionary = new Dictionary<int, AddMultiSizeDelegate>();


        public int MaxTileHorizontalIndex = 10;
        public int MaxTileVerticalIndex = 16;

        private int _HorizontalIndex = 0;
        private int _VerticalIndex = 0;
        public int HorizontalIndex
        {
            get { return _HorizontalIndex; }
            set
            {
                OldTileIdx = TileIndex;
                _HorizontalIndex = (value < 0) ? this.MaxTileHorizontalIndex - 1 : value % this.MaxTileHorizontalIndex;
            }
        }
        public int VerticalIndex
        {
            get { return _VerticalIndex; }
            set
            {
                OldTileIdx = TileIndex;
                _VerticalIndex = (value < 0) ? this.MaxTileVerticalIndex - 1 : value % this.MaxTileVerticalIndex;
            }
        }
        public int TileIndex
        {
            get { return HorizontalIndex + VerticalIndex * this.MaxTileHorizontalIndex; }
        }
        public int OldTileIdx { get; set; }
        #endregion

        #region CONSTRUCTORS
        public TileMap(Texture2D mouseMap, Texture2D slopeMap)
        {
            this.mouseMap = mouseMap;
            this.slopeMaps = slopeMap;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    // default tileID = 0 (green grassland) for all places
                    thisRow.Columns.Add(new MapCell(0) { X = x, Y = y });
                }
                Rows.Add(thisRow);
            }

            //TODO: should create a level editor
            //TODO: should enum tileIDs
            // Create Sample Map Data with other variations of data

            AddBaseTile(3, 0, 3);
            AddBaseTile(4, 0, 3);
            AddBaseTile(5, 0, 1);
            AddBaseTile(6, 0, 1);
            AddBaseTile(7, 0, 1);

            AddBaseTile(3, 1, 3);
            AddBaseTile(4, 1, 1);
            AddBaseTile(5, 1, 1);
            AddBaseTile(6, 1, 1);
            AddBaseTile(7, 1, 1);

            AddBaseTile(2, 2, 3);
            AddBaseTile(3, 2, 1);
            AddBaseTile(4, 2, 1);
            AddBaseTile(5, 2, 1);
            AddBaseTile(6, 2, 1);
            AddBaseTile(7, 2, 1);

            AddBaseTile(2, 3, 3);
            AddBaseTile(3, 3, 1);
            AddBaseTile(4, 3, 1);
            AddBaseTile(5, 3, 2);
            AddBaseTile(6, 3, 2);
            AddBaseTile(7, 3, 2);

            AddBaseTile(2, 4, 3);
            AddBaseTile(3, 4, 1);
            AddBaseTile(4, 4, 1);
            AddBaseTile(5, 4, 2);
            AddBaseTile(6, 4, 2);
            AddBaseTile(7, 4, 2);

            AddBaseTile(2, 5, 3);
            AddBaseTile(3, 5, 1);
            AddBaseTile(4, 5, 1);
            AddBaseTile(5, 5, 2);
            AddBaseTile(6, 5, 2);
            AddBaseTile(7, 5, 2);

            //add some stacking tiles for isometric height
            Rows[16].Columns[4].AddHeightTile(54);
            Rows[17].Columns[3].AddHeightTile(54);
            Rows[15].Columns[3].AddHeightTile(54);
            Rows[16].Columns[3].AddHeightTile(53);
            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(51);
            Rows[18].Columns[3].AddHeightTile(51);
            Rows[19].Columns[3].AddHeightTile(50);
            Rows[18].Columns[4].AddHeightTile(55);
            Rows[14].Columns[4].AddHeightTile(54);
            //set unwalkable
            Rows[16].Columns[4].Walkable = false;
            Rows[17].Columns[3].Walkable = false;
            Rows[15].Columns[3].Walkable = false;
            Rows[16].Columns[3].Walkable = false;
            Rows[15].Columns[4].Walkable = false;
            Rows[15].Columns[4].Walkable = false;
            Rows[15].Columns[4].Walkable = false;
            Rows[18].Columns[3].Walkable = false;
            Rows[19].Columns[3].Walkable = false;
            Rows[18].Columns[4].Walkable = false;
            Rows[14].Columns[4].Walkable = false;

            //add some stacking tiles for isometric height
            Rows[24].Columns[8].AddHeightTile(76);
            Rows[23].Columns[8].AddHeightTile(76);
            Rows[25].Columns[7].AddHeightTile(76);
            //set unwalkable terrain
            Rows[24].Columns[8].Walkable = false;
            Rows[23].Columns[8].Walkable = false;
            Rows[25].Columns[7].Walkable = false;

            Rows[24].Columns[8].AddHeightTile(77);
            Rows[23].Columns[8].AddHeightTile(77);
            Rows[25].Columns[7].AddHeightTile(77);
            Rows[24].Columns[8].AddHeightTile(67);
            Rows[23].Columns[8].AddHeightTile(68);
            Rows[25].Columns[7].AddHeightTile(69);
            //set unwalkable terrain
            Rows[24].Columns[8].Walkable = false;
            Rows[23].Columns[8].Walkable = false;
            Rows[25].Columns[7].Walkable = false;
            Rows[24].Columns[8].Walkable = false;
            Rows[23].Columns[8].Walkable = false;
            Rows[25].Columns[7].Walkable = false;

            Rows[26].Columns[7].AddHeightTile(76);
            Rows[25].Columns[6].AddHeightTile(75);
            Rows[24].Columns[6].AddHeightTile(75);
            Rows[23].Columns[5].AddHeightTile(75);
            Rows[22].Columns[5].AddHeightTile(75);

            //set unwalkable terrain
            Rows[26].Columns[7].Walkable = false;
            Rows[25].Columns[6].Walkable = false;
            Rows[24].Columns[6].Walkable = false;
            Rows[23].Columns[5].Walkable = false;
            Rows[22].Columns[5].Walkable = false;

            //multiple height stacking
            Rows[14].Columns[5].AddHeightTile(62);
            Rows[14].Columns[5].AddHeightTile(61);
            Rows[14].Columns[5].AddHeightTile(63);
            //set unwalkable
            Rows[14].Columns[5].Walkable = false;

            //add some topper tiles
            Rows[17].Columns[4].AddTopperTile(114);
            Rows[16].Columns[5].AddTopperTile(115);
            Rows[14].Columns[4].AddTopperTile(125);
            Rows[15].Columns[5].AddTopperTile(91);
            Rows[16].Columns[6].AddTopperTile(94);
            Rows[14].Columns[5].AddTopperTile(125);

            //add multi-size tiles for trees
            AddLargeLeavedTree(6, 8);
            AddLargeLeavedTree(10, 10);
            AddLargeLeavedTree(20, 2, 0);
            AddMediumConeTree(4, 0);
            AddSmallConeTree(14, 5, 3);
            AddSmallConeTree(5, 0);

            Rows[15].Columns[5].Walkable = false;
            Rows[16].Columns[6].Walkable = false;

            //Add a small hill
            //topper tiles are added around the edges to create a sloping visual effect
            Rows[12].Columns[9].AddHeightTile(34);
            Rows[11].Columns[9].AddHeightTile(34);
            Rows[11].Columns[8].AddHeightTile(34);
            Rows[10].Columns[9].AddHeightTile(34);

            Rows[12].Columns[8].AddTopperTile(31);
            Rows[12].Columns[8].SlopeMap = 0;
            Rows[13].Columns[8].AddTopperTile(31);
            Rows[13].Columns[8].SlopeMap = 0;

            Rows[12].Columns[10].AddTopperTile(32);
            Rows[12].Columns[10].SlopeMap = 1;
            Rows[13].Columns[9].AddTopperTile(32);
            Rows[13].Columns[9].SlopeMap = 1;

            Rows[14].Columns[9].AddTopperTile(30);
            Rows[14].Columns[9].SlopeMap = 4;

            // End Create Sample Map Data
        }
        #endregion

        #region METHODS

        #region DATA ADDERS AND REMOVALS
        //TODO: create a data loader configurable by text file, making new data addable w/o code change
        public void AddBaseTile(int mapx, int mapy, int id)
        {
            Rows[mapy].Columns[mapx].TileID = id;
            Rows[mapy].Columns[mapx].gameObjects.Add(new GameLogicalObject() { Name = GetTileMapLogicalObjName(id), TileSizeType = ObjTileSizeType.Single });
        }
        public void RemoveBaseTile(int mapx, int mapy)
        {
            Rows[mapy].Columns[mapx].RemoveBaseTile();
        }
        public void AddHeightTile(int mapx, int mapy, int id)
        {
            Rows[mapy].Columns[mapx].AddHeightTile(id);
            Rows[mapy].Columns[mapx].gameObjects.Add(new GameLogicalObject() { Name = GetTileMapLogicalObjName(id), TileSizeType = ObjTileSizeType.Single });
        }
        public void RemoveHeightTile(int mapx, int mapy)
        {
            Rows[mapy].Columns[mapx].RemoveHeightTile();
        }
        public void AddTopperTile(int mapx, int mapy, int id)
        {
            Rows[mapy].Columns[mapx].AddTopperTile(id);
            Rows[mapy].Columns[mapx].gameObjects.Add(new GameLogicalObject() { Name = GetTileMapLogicalObjName(id), TileSizeType = ObjTileSizeType.Single });
        }
        public void RemoveTopperTile(int mapx, int mapy)
        {
            Rows[mapy].Columns[mapx].RemoveTopperTile();
        }
        public void AddMultiTile(int mapx, int mapy, int id)
        {
            AddFromTileMapConfig(mapy, mapx, id);
            Rows[mapy].Columns[mapx].gameObjects.Add(new GameLogicalObject() { Name = GetTileMapLogicalObjName(id), TileSizeType = ObjTileSizeType.Multi });
        }
        public void RemoveMultiTile(int mapx, int mapy)
        {
            Rows[mapy].Columns[mapx].RemoveMultiSizeTiles();
        }


        protected void AddLargeLeavedTree(int row, int column, int height=0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(158, 0, 0, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(148, 0, 1, height);    //middle
            Rows[row].Columns[column].AddMultiSizeTile(138, 0, 2, height);    //branch upper middle
            Rows[row].Columns[column].AddMultiSizeTile(137, 1, 2, height);    //branch upper left
            Rows[row].Columns[column].AddMultiSizeTile(139, -1, 2, height);   //branch upper right
            Rows[row].Columns[column].AddMultiSizeTile(157, 1, 0, height);    //branch lower left
            Rows[row].Columns[column].AddMultiSizeTile(147, 1, 1, height);    //branch middle left
            Rows[row].Columns[column].AddMultiSizeTile(149, -1, 1, height);   //branch middle right
            Rows[row].Columns[column].AddMultiSizeTile(159, -1, 0, height);   //branch middle right

        }
        protected void AddMediumConeTree(int row, int column, int height = 0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(122, 0, 1, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(132, 0, 0, height);    //middle


        }
        protected void AddSmallConeTree(int row, int column, int height = 0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(123, 0, 1, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(133, 0, 0, height);    //middle

        }

        /// <summary>
        /// Registers data added mappings of multi-sized tiles from configuration file
        /// </summary>
        public void RegisterConfigurationFile()
        {
            IniFile ini = new IniFile(@"./Config/TileSet4.ini");

            foreach (var section in ini.ReadIni())
            {
                string sectionname = section.Key;
                TileType tileType = TileType.Base;

                GameLogicalObject logicalObj = new GameLogicalObject() { Name = sectionname, TileSizeType= ObjTileSizeType.Single};

                foreach (var pair in section.Value)
                {
                    try
                    {
                        //resolve Key : TileType
                        if (pair.Key == "TileType")
                        {
                            tileType = (TileType)Enum.Parse(typeof(TileType), pair.Value);
                            continue;
                        }

                        //resolve Key : DataIndex (for Multi-tile size objects)
                        if (pair.Key == "DataIndex")
                        {
                            logicalObj.TileSizeType = ObjTileSizeType.Multi;

                            string[] val = pair.Value.Split('|');
                            foreach (string s in val)
                            {
                                string[] frag = s.Split(',');

                                GameTileInfo data = new GameTileInfo() { TileType = tileType };

                                data.TileID = int.Parse(frag[0]);
                                data.TileXOffset = int.Parse(frag[1]);
                                data.TileYOffset = int.Parse(frag[2]);

                                Debug.WriteLine(logicalObj.Name);

                                if (!ObjectDictionary.ContainsKey(data.TileID))
                                    ObjectDictionary.Add(data.TileID, logicalObj);

                                logicalObj.Add(data);
                            }
                            continue;
                        }

                        //If add in bulk (for singly sized tiles)
                        if (pair.Key == "BulkDataIndex")
                        {
                            string[] val = pair.Value.Split(',');
                            foreach(string frag in val)
                            {
                                Regex rangeExpression = new Regex("(?<startIdx>[0-9]+):(?<endIdx>[0-9]+)");

                                Match matchRangeExpres = rangeExpression.Match(frag);

                                if (matchRangeExpres.Success)
                                {
                                    int startIndex = int.Parse(matchRangeExpres.Groups["startIdx"].Value);
                                    int endIndex = int.Parse(matchRangeExpres.Groups["endIdx"].Value);

                                    for(int i = startIndex; i <= endIndex; i++)
                                    {
                                        logicalObj = (GameLogicalObject)logicalObj.Clone();

                                        logicalObj.Name = sectionname + i.ToString();
                                        logicalObj.Add(new GameTileInfo() { TileType = tileType, TileID = i });

                                        Debug.WriteLine(logicalObj.Name);

                                        if (!ObjectDictionary.ContainsKey(i))
                                            ObjectDictionary.Add(i, logicalObj);
                                    }
                                }
                                else
                                {
                                    int TileID = int.Parse(frag);

                                    logicalObj = (GameLogicalObject)logicalObj.Clone();

                                    logicalObj.Name = sectionname + frag;
                                    logicalObj.Add(new GameTileInfo() { TileType = tileType, TileID = TileID });

                                    Debug.WriteLine(logicalObj.Name);

                                    if (!ObjectDictionary.ContainsKey(TileID))
                                        ObjectDictionary.Add(TileID, logicalObj);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("MultiSize Tile Settings ini file error.");
                    }
                }
            }

        }

        /// <summary>
        /// Adds data according to data found in configuration
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="id"></param>
        public void AddFromTileMapConfig(int row, int column, int id)
        {
            //TODO: load sections data according to what's loaded from ini file
            if(ObjectDictionary.ContainsKey(id))
            {
                foreach (GameTileInfo t in ObjectDictionary[id].GetEnumerable())
                {
                    switch(t.TileType)
                    {
                        case TileType.Multi:
                            Rows[row].Columns[column].AddMultiSizeTile(t.TileID, t.TileXOffset, t.TileYOffset, Rows[row].Columns[column].HeightTiles.Count());
                        break;
                    }
                }
            }
        }

        #endregion

        #region TILE MAP OPERATIONS

        public List<int> GetTileMapHilightIndexes(int id)
        {
            if (ObjectDictionary.ContainsKey(id))
            {
                if (ObjectDictionary[id].TileSizeType == ObjTileSizeType.Multi)
                    return (from data in ObjectDictionary[id].GetEnumerable()
                            select data.TileID
                        ).ToList();
                else
                    return new List<int>() { id };
            }
            else
                return new List<int>() { id };
        }
        public IEnumerable<GameTileInfo> GetGameTileInfoList(int id)
        {
            if (ObjectDictionary.ContainsKey(id) &&
                ObjectDictionary[id].TileSizeType == ObjTileSizeType.Multi)
            {
                return ObjectDictionary[id].GetEnumerable();
            }
            else
                return new [] { new GameTileInfo(){ TileID = id, TileXOffset = 0, TileYOffset = 0} };
        }

        public string GetTileMapLogicalObjName(int id)
        {
            if (ObjectDictionary.ContainsKey(id))
                return ObjectDictionary[id].Name;
            else
                return id.ToString();
        }

        public ObjTileSizeType GetLogicalObjTileType(int id)
        {
            if (ObjectDictionary.ContainsKey(id))
                return ObjectDictionary[id].TileSizeType;
            else
                return ObjTileSizeType.Single;
        }

        public void GetLeftTileIndex()
        {

            string objname = GetTileMapLogicalObjName(TileIndex);

            while (GetTileMapLogicalObjName(TileIndex) == objname)
                --HorizontalIndex;           
        }
        public void GetRightTileIndex()
        {
            string objname = GetTileMapLogicalObjName(TileIndex);

            while (GetTileMapLogicalObjName(TileIndex) == objname)
                ++HorizontalIndex;   
        }
        public void GetUpTileIndex()
        {
            string objname = GetTileMapLogicalObjName(TileIndex);

            while (GetTileMapLogicalObjName(TileIndex) == objname)
                --VerticalIndex; 
        }
        public void GetDownTileIndex()
        {
            string objname = GetTileMapLogicalObjName(TileIndex);

            while (GetTileMapLogicalObjName(TileIndex) == objname)
                ++VerticalIndex;
        }

        #endregion

        #region MAP CELL COORDINATE FUNCTIONS
        public MapCell GetMapCell(int MapCellX, int MapCellY)
        {
            return Rows[MapCellY].Columns[MapCellX];
        }
        //get adjacent cells
        public List<MapCell> GetAdjMapCells(int MapCellX, int MapCellY)
        {
            List<MapCell> cells = new List<MapCell>(8);
            if (MapCellY % 2 == 1)
            {
                cells.Add(this.GetMapCell(MapCellX, MapCellY - 2)); //N
                cells.Add(this.GetMapCell(MapCellX + 1, MapCellY - 1)); //NE
                cells.Add(this.GetMapCell(MapCellX + 1, MapCellY)); //E
                cells.Add(this.GetMapCell(MapCellX + 1, MapCellY + 1)); //SE
                cells.Add(this.GetMapCell(MapCellX, MapCellY + 2)); //S
                cells.Add(this.GetMapCell(MapCellX, MapCellY + 1)); //SW
                cells.Add(this.GetMapCell(MapCellX - 1, MapCellY)); //W
                cells.Add(this.GetMapCell(MapCellX, MapCellY - 1)); //NW
            }
            //even row apply even offset
            else
            {
                cells.Add(this.GetMapCell(MapCellX, MapCellY - 2)); //N
                cells.Add(this.GetMapCell(MapCellX, MapCellY - 1)); //NE
                cells.Add(this.GetMapCell(MapCellX + 1, MapCellY)); //E
                cells.Add(this.GetMapCell(MapCellX, MapCellY + 1)); //SE
                cells.Add(this.GetMapCell(MapCellX, MapCellY + 2)); //S
                cells.Add(this.GetMapCell(MapCellX - 1, MapCellY + 1)); //SW
                cells.Add(this.GetMapCell(MapCellX - 1, MapCellY)); //W
                cells.Add(this.GetMapCell(MapCellX - 1, MapCellY - 1)); //NW
            }
            return cells;
        }

        public MapCell GetMapCellAtDirection(IsometricDirections dir, int MapCellX, int MapCellY)
        {
            if (MapCellY % 2 == 1)
            {
                switch(dir)
                {
                    case IsometricDirections.N:
                        return this.GetMapCell(MapCellX, MapCellY - 2); //N
                    case IsometricDirections.NE:
                        return this.GetMapCell(MapCellX + 1, MapCellY - 1); //NE
                    case IsometricDirections.E:
                        return this.GetMapCell(MapCellX + 1, MapCellY); //E
                    case IsometricDirections.SE:
                        return this.GetMapCell(MapCellX + 1, MapCellY + 1); //SE
                    case IsometricDirections.S:
                        return this.GetMapCell(MapCellX, MapCellY + 2); //S
                    case IsometricDirections.SW:
                        return this.GetMapCell(MapCellX, MapCellY + 1); //SW
                    case IsometricDirections.W:
                        return this.GetMapCell(MapCellX - 1, MapCellY); //W
                    case IsometricDirections.NW:
                        return this.GetMapCell(MapCellX, MapCellY - 1); //NW
                    default:
                        return null;
                }
            }
            //even row apply even offset
            else
            {
                switch (dir)
                {
                    case IsometricDirections.N:
                        return this.GetMapCell(MapCellX, MapCellY - 2); //N
                    case IsometricDirections.NE:
                        return this.GetMapCell(MapCellX, MapCellY - 1); //NE
                    case IsometricDirections.E:
                        return this.GetMapCell(MapCellX + 1, MapCellY); //E
                    case IsometricDirections.SE:
                        return this.GetMapCell(MapCellX, MapCellY + 1); //SE
                    case IsometricDirections.S:
                        return this.GetMapCell(MapCellX, MapCellY + 2); //S
                    case IsometricDirections.SW:
                        return this.GetMapCell(MapCellX - 1, MapCellY + 1); //SW
                    case IsometricDirections.W:
                        return this.GetMapCell(MapCellX - 1, MapCellY); //W
                    case IsometricDirections.NW:
                        return this.GetMapCell(MapCellX - 1, MapCellY - 1); //NW
                    default:
                        return null;
                }
            }
        }

        //gets from MapCell To Screen (Center of rombus)
        public Vector2 MapCellToScreen(Camera camera, int MapCellX, int MapCellY)
        {
            Vector2 FirstMapCell = GetCameraFirstSquare(camera);

            int rowOffset = (MapCellY % 2 == 1) ? rowOffset = Tile.OddRowXOffset : 0;
            return new Vector2((MapCellX - FirstMapCell.X) * mouseMap.Width + rowOffset, (MapCellY - FirstMapCell.Y) * mouseMap.Height / 2 - 16)*camera.Scale;
        }
        public Vector2 MapCellToScreen(Camera camera, Vector2 mapCellPoint)
        {
            return MapCellToScreen(camera, (int)mapCellPoint.X, (int)mapCellPoint.Y);
        }

        public Vector2 GetCameraFirstSquare(Camera camera)
        {
            return new Vector2(camera.Location.X / Tile.TileStepX, camera.Location.Y / Tile.TileStepY);
        }

        //overload, return a point from world coordinates
        public Point WorldToMapCell(Point worldPoint)
        {
            Point dummy;
            return WorldToMapCell(worldPoint, out dummy);
        }
        //overload using a Vector for worldPoint
        public Point WorldToMapCell(Vector2 worldPoint)
        {
            return WorldToMapCell(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }
        //overload to use Vector
        public MapCell GetCellAtWorldPoint(Vector2 worldPoint)
        {
            return GetCellAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }
        //overload to use Vector
        public int GetOverallHeight(Vector2 worldPoint)
        {
            return GetOverallHeight(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }


        //convert pixel-based location on the map into map-cell reference
        private Point WorldToMapCell(Point worldPoint, out Point localPoint)
        {
            //get the map cell from worldPoint
            Point mapCell = new Point(
                worldPoint.X / mouseMap.Width,
               (worldPoint.Y / mouseMap.Height) * 2
               );

            //this.mouseMap.

            //get local point within a map cell
            int localPointX = worldPoint.X % mouseMap.Width;
            int localPointY = worldPoint.Y % mouseMap.Height;

            //represents the map cell offsets
            int dx = 0;
            int dy = 0;

            uint[] myUint = new uint[1];

            //Do a point in rectangle check, if so, find out which part.
            if (new Rectangle(0, 0, mouseMap.Width, mouseMap.Height).Contains(localPointX, localPointY))
            {
                //extract a single pixel of map data from localPointX,Y
                //store results in uint array
                //TODO: use Color[] to get data so we don't need to bitmask off the colors below
                //http://rbwhitaker.wikidot.com/extracting-texture-data
                mouseMap.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), myUint, 0, 1);

                //set the map cell offsets depending on the color
                //uint values returned are in AABBGGRR order
                switch(myUint[0])
                {
                    case 0xFF0000FF:// Red
                        dx = -1;
                        dy = -1;

                        //TODO: need a better understanding of this manipulation
                        localPointX = localPointX + (mouseMap.Width / 2);
                        localPointY = localPointY + (mouseMap.Height / 2);
                        break;
                    case 0xFF00FF00:// Green
                        dx = -1;
                        dy = 1;
                        localPointX = localPointX + (mouseMap.Width / 2);
                        localPointY = localPointY - (mouseMap.Height / 2);
                        break;
                    case 0xFF00FFFF:// Yellow
                        dy = -1;
                        localPointX = localPointX - (mouseMap.Width / 2);
                        localPointY = localPointY + (mouseMap.Height / 2);
                        break;
                    case 0xFFFF0000:// Blue
                        dy = 1;
                        localPointX = localPointX - (mouseMap.Width / 2);
                        localPointY = localPointY - (mouseMap.Height / 2);
                        break;
                }
            }


            mapCell.X += dx;
            //subtract two from the Y coordinate because the top two "rows" of our map actually don't contain real map information
            mapCell.Y += dy - 2;

            //make sure X or Y don't become negative. Negative coordinates are illegal
            mapCell.X = (mapCell.X < 0) ? 0 : mapCell.X;
            mapCell.Y = (mapCell.Y < 0) ? 0 : mapCell.Y;

            //TODO: need this in another part. need to comment
            localPoint = new Point(localPointX, localPointY);

            return mapCell;
        }
        //uses the existing methods to look up the location of a map point and
        //determining what actual map cell is at that point and returning it
        private MapCell GetCellAtWorldPoint(Point worldPoint)
        {
            Point mapPoint = WorldToMapCell(worldPoint);
            return Rows[mapPoint.Y].Columns[mapPoint.X];
        }
        //get a height of a point on a cell with slope
        private int GetCellSlopeHeightAtWorldPoint(Point worldPoint)
        {
            Point localPoint;
            Point mapPoint = WorldToMapCell(worldPoint, out localPoint);
            int slopeMap = Rows[mapPoint.Y].Columns[mapPoint.X].SlopeMap;

            return GetSlopeMapHeight(localPoint, slopeMap);
        }
        private int GetCellSlopeHeightAtWorldPoint(Vector2 worldPoint)
        {
            return GetCellSlopeHeightAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }
        //get height of a point in a sloped cell using local pixel and a slope map index (0 to 7)
        private int GetSlopeMapHeight(Point localPixel, int slopeMap)
        {
            //get a point from the slope map offset by localPixel, assuming the width is the same as the mouse map
            Point texturePoint = new Point(slopeMap * mouseMap.Width + localPixel.X, localPixel.Y);

            Color[] slopeColor = new Color[1];

            //check if the rectangle of the slope map contains the point specified by local pixel
            if (new Rectangle(0, 0, slopeMaps.Width, slopeMaps.Height).Contains(texturePoint.X, texturePoint.Y))
            {
                //get the color of the point
                slopeMaps.GetData(0, new Rectangle(texturePoint.X, texturePoint.Y, 1, 1), slopeColor, 0, 1);

                //Get the height by scaling the height tile offset by a proportion of the slopeColor intensity
                //represented by R. a slopeColor.R of 255(white) would give 0 offset, while a slopeColor of 0(black) would give 1.
                return (int)(((float)(255 - slopeColor[0].R) / 255f) * Tile.HeightTileOffset);
            }

            return 0;
        }
        //get height of a point in terms of world coordinates
        public int GetOverallHeight(Point worldPoint)
        {
            Point mapCellPoint = WorldToMapCell(worldPoint);
            int height = Rows[mapCellPoint.Y].Columns[mapCellPoint.X].HeightTiles.Count * Tile.HeightTileOffset;
            height += GetCellSlopeHeightAtWorldPoint(worldPoint);

            return height;
        }
        public int GetOverallCenterHeight(int row, int column)
        {
            int height = Rows[row].Columns[column].HeightTiles.Count * Tile.HeightTileOffset;
            int slopeMap = Rows[row].Columns[column].SlopeMap;
            height += GetSlopeMapHeight(new Point(mouseMap.Width / 2, mouseMap.Height / 2), slopeMap);

            return height;
        }
        #endregion

        #region DISTANCE FUNCTIONS
        //returns the Lo Tile distance given a staggered isometric coordinate system
        //reference:http://gamedev.stackexchange.com/questions/50668/distance-between-two-points-with-staggered-isometric-coordinate-system
        public static int L0TileDistance(Point s, Point e)
        {
            return L0TileDistance(s.X, s.Y, e.X, e.Y);
        }

        //override
        public static int L0TileDistance(int sX, int sY, int eX, int eY)
        {
            int distance;

            int XDiff = Math.Abs(sX - eX);
            int YDiff = Math.Abs(sY - eY);

            //vertical distance
            if (XDiff == 0)
                distance = Convert.ToInt32(Math.Ceiling(YDiff / 2.0));
            //horizontal dist
            else if (YDiff == 0)
                distance = XDiff;
            //diagonal and others
            else
            {
                //handle special case when 2nd position is left of first
                if (sX > eX)
                {
                    //handle odd or even row
                    if (sY % 2 == 1)
                        distance = Convert.ToInt32(Math.Ceiling(YDiff / 2.0 + XDiff));
                    else
                        distance = Convert.ToInt32(Math.Floor(YDiff / 2.0 + XDiff));
                }
                else
                {
                    if (sY % 2 == 1)
                        distance = Convert.ToInt32(Math.Floor(YDiff / 2.0 + XDiff));
                    else
                        distance = Convert.ToInt32(Math.Ceiling(YDiff / 2.0 + XDiff));
                }
            }
           
            return distance;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// A row of map cells
    /// </summary>
    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    public enum IsometricDirections
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW,
        UNDEFINED,
    }
}
