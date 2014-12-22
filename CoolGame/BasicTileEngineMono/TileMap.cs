using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    /// <summary>
    /// A Map made of tiles is created and managed
    /// 
    /// </summary>
    class TileMap
    {
        private Texture2D mouseMap;

        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 50;
        public int MapHeight = 50;

        public TileMap(Texture2D mouseMap)
        {
            this.mouseMap = mouseMap;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    // default tileID = 0 (green grassland) for all places
                    thisRow.Columns.Add(new MapCell(0));
                }
                Rows.Add(thisRow);
            }

            //TODO: should enum tileIDs
            // Create Sample Map Data with other variations of data
            Rows[0].Columns[3].TileID = 3;
            Rows[0].Columns[4].TileID = 3;
            Rows[0].Columns[5].TileID = 1;
            Rows[0].Columns[6].TileID = 1;
            Rows[0].Columns[7].TileID = 1;

            Rows[1].Columns[3].TileID = 3;
            Rows[1].Columns[4].TileID = 1;
            Rows[1].Columns[5].TileID = 1;
            Rows[1].Columns[6].TileID = 1;
            Rows[1].Columns[7].TileID = 1;

            Rows[2].Columns[2].TileID = 3;
            Rows[2].Columns[3].TileID = 1;
            Rows[2].Columns[4].TileID = 1;
            Rows[2].Columns[5].TileID = 1;
            Rows[2].Columns[6].TileID = 1;
            Rows[2].Columns[7].TileID = 1;

            Rows[3].Columns[2].TileID = 3;
            Rows[3].Columns[3].TileID = 1;
            Rows[3].Columns[4].TileID = 1;
            Rows[3].Columns[5].TileID = 2;
            Rows[3].Columns[6].TileID = 2;
            Rows[3].Columns[7].TileID = 2;

            Rows[4].Columns[2].TileID = 3;
            Rows[4].Columns[3].TileID = 1;
            Rows[4].Columns[4].TileID = 1;
            Rows[4].Columns[5].TileID = 2;
            Rows[4].Columns[6].TileID = 2;
            Rows[4].Columns[7].TileID = 2;

            Rows[5].Columns[2].TileID = 3;
            Rows[5].Columns[3].TileID = 1;
            Rows[5].Columns[4].TileID = 1;
            Rows[5].Columns[5].TileID = 2;
            Rows[5].Columns[6].TileID = 2;
            Rows[5].Columns[7].TileID = 2;


            //Add some fringe transitions around water area (part2)
            /*
            Rows[3].Columns[5].AddBaseTile(30);
            Rows[4].Columns[5].AddBaseTile(27);
            Rows[5].Columns[5].AddBaseTile(28);

            Rows[3].Columns[6].AddBaseTile(25);
            Rows[5].Columns[6].AddBaseTile(24);

            Rows[3].Columns[7].AddBaseTile(31);
            Rows[4].Columns[7].AddBaseTile(26);
            Rows[5].Columns[7].AddBaseTile(29);

            Rows[4].Columns[5].AddBaseTile(102);
            Rows[4].Columns[6].AddBaseTile(104);
            Rows[4].Columns[7].AddBaseTile(103);
            */


            //add multi-size tiles for tree (stacking)
            AddLargeLeavedTree(6, 8);
            AddLargeLeavedTree(10, 10);

            //add some stacking tiles for isometry
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

            //rocks stacking on top of one another
            Rows[14].Columns[5].AddHeightTile(62);
            Rows[14].Columns[5].AddHeightTile(61);
            Rows[14].Columns[5].AddHeightTile(63);

            //add some topper tiles for isometry
            Rows[17].Columns[4].AddTopperTile(114);
            Rows[16].Columns[5].AddTopperTile(115);
            Rows[14].Columns[4].AddTopperTile(125);
            Rows[15].Columns[5].AddTopperTile(91);
            Rows[16].Columns[6].AddTopperTile(94);


            Rows[14].Columns[5].AddTopperTile(125);

            AddLargeLeavedTree(20, 2, 0);
            AddSmallConeTree(14, 5, 3);

            AddMediumConeTree(4, 0);

            AddSmallConeTree(5, 0);

            // End Create Sample Map Data
        }

        //TODO: create a data loader configurable by text file, making new data addable w/o code change

        public void AddLargeLeavedTree(int row, int column, int height=0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(158, 0, 0, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(148, 0, 1, height);    //middle
            Rows[row].Columns[column].AddMultiSizeTile(138, 0, 2, height);    //upp
            Rows[row].Columns[column].AddMultiSizeTile(157, 1, 0, height);    //branch lower left
            Rows[row].Columns[column].AddMultiSizeTile(147, 1, 1, height);    //branch middle left
            Rows[row].Columns[column].AddMultiSizeTile(149, -1, 1, height);   //branch middle right
            Rows[row].Columns[column].AddMultiSizeTile(159, -1, 0, height);   //branch middle right
        }

        public void AddMediumConeTree(int row, int column, int height=0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(122, 0, 1, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(132, 0, 0, height);    //middle
        }

        public void AddSmallConeTree(int row, int column, int height = 0)
        {
            Rows[row].Columns[column].AddMultiSizeTile(123, 0, 1, height);    //trunk
            Rows[row].Columns[column].AddMultiSizeTile(133, 0, 0, height);    //middle
        }

        //convert pixel-based location on the map into map-cell reference
        //see : http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:7
        public Point WorldToMapCell(Point worldPoint, out Point localPoint)
        {
            //get the map cell from worldPoint
            Point mapCell = new Point(
               (int)(worldPoint.X / mouseMap.Width),
               ((int)(worldPoint.Y / mouseMap.Height)) * 2
               );

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
                //http://rbwhitaker.wikidot.com/extracting-texture-data
                mouseMap.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), myUint, 0, 1);

                //set the map cell offsets depending on the color
                //uint values returned are in AABBGGRR order
                switch(myUint[0])
                {
                    case 0xFF0000FF:// Red
                        dx = -1;
                        dy = -1;
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

        //overload, simply return a point
        public Point WorldToMapCell(Point worldPoint)
        {
            Point dummy;
            return WorldToMapCell(worldPoint, out dummy);
        }
    }

    /// <summary>
    /// A row of map cells
    /// </summary>
    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
