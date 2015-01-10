using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BasicTile
{
    public abstract class GameObject: ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class GameTileInfo
    {
        public TileType TileType { get; set; }
        public int TileID { get; set; }
        public int TileXOffset { get; set; }
        public int TileYOffset { get; set; }
        public int Height { get; set; }
    }

    /// <summary>
    /// A Logical Game Object (e.g. Tree1, Rock1, etc)
    /// </summary>
    public class GameLogicalObject : GameObject
    {
        protected List<GameTileInfo> GameTileInfoList = new List<GameTileInfo>();

        public string Name { get; set; }
        public ObjTileSizeType TileSizeType { get; set; }

        public void Add(GameTileInfo gameTileInfo)
        {
            GameTileInfoList.Add(gameTileInfo);
        }

        public IEnumerable<GameTileInfo> GetEnumerable()
        {
            return GameTileInfoList.AsEnumerable();
        }
    }

    public enum TileType
    {
        Base,
        Height,
        Topper,
        Multi
    }
    public enum ObjTileSizeType
    {
        Single,
        Multi
    }
}
