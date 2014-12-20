﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicTile
{
    /// <summary>
    /// MapCell (itself managing an area of the map that represents a tile area, but not necessary a single tile)
    /// </summary>
    public class MapCell
    {
        //list of tile ids can stack any number of tile images on the same space
        public List<int> BaseTiles = new List<int>();


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
        }

        public void AddBaseTile(int tileID)
        {
            BaseTiles.Add(tileID);
        }


    }
}
