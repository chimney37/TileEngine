using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    /// <summary>
    /// Manages the tile resource.
    /// FOr more information refer to: http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:3
    /// </summary>
    static class Tile
    {
        static public Texture2D TileSetTexture;

        //
        //begin-- Hexagonal map ( part3)
        //For square map, tile width, height = 32 (part1)
        //For square map (part2), replace TileWidth=48 and TileHeight=48
        static public int TileWidth = 33;
        static public int TileHeight = 27;

        //For square map TileStepX = TileWidth, TileStepY = TileHeight
        //horizontal: 33 (wdith of tile) + 19 (wdith of top side of tile) + 2 pixels spaces
        //vertical: 15 (half of height) + 2 pixels spaces
        static public int TileStepX = 54;
        static public int TileStepY = 17;
        //every odd row would need to push to the side by width of top side: 19 + empty space along top right: 7 + 1 pixel space
        static public int OddRowXOffset = 27;
        //end --

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
