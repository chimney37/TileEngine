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

        /// <summary>
        /// Gets the type of tile according to an index.
        /// Notice that tile index can be 0,1,2 for a 3 tile set. See part1_tileset.png
        /// </summary>
        /// <param name="tileIndex"></param>
        /// <returns></returns>
        static public Rectangle GetSourceRectangle(int tileIndex)
        {
            return new Rectangle(tileIndex * 32, 0, 32, 32);
        }
    }
}
