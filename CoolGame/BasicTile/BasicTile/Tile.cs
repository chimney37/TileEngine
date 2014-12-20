using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    /// <summary>
    /// Manages the tile resource
    /// </summary>
    static class Tile
    {
        static public Texture2D TileSetTexture;

        //set the tile width and height
        static public int TileWidth = 48;
        static public int TileHeight = 48;

        /// <summary>
        /// Gets the type of tile according to an index.
        /// Notice that tile index can be 0,1,2 for a 3 tile set. See part1_tileset.png and
        /// part2_tileset.png. indexing goes from left to right, top to bottom
        /// </summary>
        /// <param name="tileIndex"></param>
        /// <returns></returns>
        static public Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            int tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
