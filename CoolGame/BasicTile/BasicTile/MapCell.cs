using System;
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
        public int TileID { get; set; }

        public MapCell(int tileID)
        {
            TileID = tileID;
        }
    }
}
