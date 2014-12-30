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
        public static Texture2D TileSetTexture;

        //
        //begin-- 
        //For square map (part1), tile width, height = 32 
        //For square map (part2), replace TileWidth=48 and TileHeight=48
        //For hexagonal map (part3), TileWidth=33, TIleHeight=27
        //For isometric map (part 4), replace TileWdith,TIleHeight=64
        public static int TileWidth = 64;
        public static int TileHeight = 64;

        //For square map TileStepX = TileWidth, TileStepY = TileHeight
        //For hexagonal map:
        //horizontal: 33 (wdith of tile) + 19 (wdith of top side of tile) + 2 pixels spaces
        //vertical: 15 (half of height) + 2 pixels spaces
        //For isometric map: StepX=64, StepY=16
        public static int TileStepX = 64;
        public static int TileStepY = 16;


        //hexagonal map:
        // OddRowXOffset: every odd row would need to push to the side by width of top side: 19 + empty space along top right: 7 + 1 pixel space
        //isometric map:
        //XOffset 32
        public static int OddRowXOffset = 32;
        public static int HeightTileOffset = 32;
        public static int MultiSizeTileOffset = 64;
        //end --

        public static int MaxTileHorizontalIndex = 10;
        public static int MaxTileVerticalIndex = 15;

        /// <summary>
        /// Gets the type of tile according to an index.
        /// Notice that tile index can be 0,1,2 for a 3 tile set. See part1_tileset.png and
        /// part2_tileset.png. indexing goes from left to right, top to bottom
        /// </summary>
        /// <param name="tileIndex"></param>
        /// <returns></returns>
        public static Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            int tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }

        public static Rectangle GetSourceTileSet()
        {
            return new Rectangle(0, 0, TileSetTexture.Width, TileSetTexture.Height);
        }
    }
}
