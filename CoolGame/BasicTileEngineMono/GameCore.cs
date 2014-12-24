using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;

namespace BasicTile
{

    public class GameCore : GameProcess
    {

        #region GAME CLIENT PROPERTIES
        //Default Tile Map: defines what's in a map
        //squaresAcross/Down : define how many tiles to show on screen at once
        TileMap myMap;
        int squaresAcross = 25;
        int squaresDown = 32;

        //for isometric map support
        //this isometric set, is by 64x64,but the image only occupies bottom 32 pixels of tile.
        //http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:4
        //Base offset controls the screen coordinate offset from the top left of the screen coordinates
        int baseOffsetX = -32;
        int baseOffsetY = -64;
        float heightRowDepthMod = 0.0000001f;
        float scale = 1.0f;

        //2D camera
        Camera camera;

        //for keeping the first rendering square upper left of screen according to camera view
        Vector2 firstSquare;

        //debugging tile locations on map
        SpriteFont pericles6;
        SpriteFont snippets14;
        bool EnableDebugging = false;

        KeyboardState oldState;
        MouseState oldMouseState;
        int prevMouseScrollValue;

        //highlighting tiles
        Texture2D hilight;

        //NPC character
        SpriteAnimation npc;
        Point npcMapPoint;

        //player character
        SpriteAnimation vlad;
        Point vladMapPoint;

        #endregion


        public override void Initialize(Game game)
        {
            //intialize old keyboard state
            oldState = Keyboard.GetState();

            oldMouseState = Mouse.GetState();
            prevMouseScrollValue = Mouse.GetState().ScrollWheelValue;

            //make mouse visible
            game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            //Load tiles
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");

            //load fonts
            pericles6 = Content.Load<SpriteFont>(@"Fonts\Pericles6");
            snippets14 = Content.Load<SpriteFont>(@"Fonts\Snippets14");

            //initialize TileMap
            myMap = new TileMap(
                Content.Load<Texture2D>(@"Textures\TileSets\mousemap"),
                Content.Load<Texture2D>(@"Textures\TileSets\part9_slopemaps"));

            TileMap.Scale = this.scale;

            //initialize camera
            camera = new Camera();

            camera.ViewWidth = graphics.PreferredBackBufferWidth;
            camera.ViewHeight = graphics.PreferredBackBufferHeight;
            camera.WorldWidth = ((myMap.MapWidth - 2) * Tile.TileStepX);
            camera.WorldHeight = ((myMap.MapHeight - 2) * Tile.TileStepY);
            camera.DisplayOffset = new Vector2(baseOffsetX, baseOffsetY);
            camera.Scale = this.scale;

            //intiliaze highlight
            hilight = Content.Load<Texture2D>(@"Textures\TileSets\hilight");

            //create sprite and animations for player
            vlad = new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48"));

            //TODO: maybe enuming the strings for runtime safety
            //design note: 1 animation sequence per row in the image
            vlad.AddAnimation("WalkEast", 0, 48 * 0, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorth", 0, 48 * 1, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouth", 0, 48 * 4, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkWest", 0, 48 * 7, 48, 48, 8, 0.1f);

            vlad.AddAnimation("IdleEast", 0, 48 * 0, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorth", 0, 48 * 1, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleWest", 0, 48 * 7, 48, 48, 1, 0.2f);

            vlad.Position = new Vector2(100, 100);
            vlad.DrawOffset = new Vector2(-24, -38);    //specifying position where character is standing
            vlad.CurrentAnimation = "WalkEast";
            vlad.IsAnimating = true;

            //load NPC texture
            npc = new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\SmileyWalk"));
            npc.AddAnimation("Idle1", 0, 0, 64, 64, 4, 0.1f, "Idle2");
            npc.AddAnimation("Idle2", 0, 64 * 1, 64, 64, 4, 0.1f, "Idle3");
            npc.AddAnimation("Idle3", 0, 64 * 2, 64, 64, 4, 0.1f, "Idle4");
            npc.AddAnimation("Idle4", 0, 64 * 3, 64, 64, 4, 0.1f, "Idle1");
            npc.Position = new Vector2(200, 200);
            npc.DrawOffset = new Vector2(-32, -32);
            npc.CurrentAnimation = "Idle1";
            npc.IsAnimating = true;
        }

        public override void Update(GameTime gameTime, Context context)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            #region ZOOM CONTROL
            //TODO: mouse scroll for zooming in and out
            if (ms.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                scale += 0.5f;
            else if (ms.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                scale -= 0.5f;

            //TODO: move clamp to camera
            //clamp maximum zoom to 2.0 times
            scale = MathHelper.Clamp(scale, 1.0f, 2.0f);

            //scale tiling render dimensions
            camera.Scale = this.scale;

            #endregion


            // TODO: Add your update logic here
            #region SET DIRECTION AND MOVEMENT VECTORS PER KEY PRESS TYPE
            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            string animation = "";

            if (ks.IsKeyDown(Keys.NumPad7))
            {
                moveDir = new Vector2(-2, -1);
                animation = "WalkNorthWest";
                moveVector += new Vector2(-2, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad8))
            {
                moveDir = new Vector2(0, -1);
                animation = "WalkNorth";
                moveVector += new Vector2(0, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad9))
            {
                moveDir = new Vector2(2, -1);
                animation = "WalkNorthEast";
                moveVector += new Vector2(2, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad4))
            {
                moveDir = new Vector2(-2, 0);
                animation = "WalkWest";
                moveVector += new Vector2(-2, 0);
            }

            if (ks.IsKeyDown(Keys.NumPad6))
            {
                moveDir = new Vector2(2, 0);
                animation = "WalkEast";
                moveVector += new Vector2(2, 0);
            }

            if (ks.IsKeyDown(Keys.NumPad1))
            {
                moveDir = new Vector2(-2, 1);
                animation = "WalkSouthWest";
                moveVector += new Vector2(-2, 1);
            }

            if (ks.IsKeyDown(Keys.NumPad2))
            {
                moveDir = new Vector2(0, 1);
                animation = "WalkSouth";
                moveVector += new Vector2(0, 1);
            }

            if (ks.IsKeyDown(Keys.NumPad3))
            {
                moveDir = new Vector2(2, 1);
                animation = "WalkSouthEast";
                moveVector += new Vector2(2, 1);
            }

            //prevent from moving if not walkable
            if (myMap.GetCellAtWorldPoint(vlad.Position + moveDir).Walkable == false)
                moveDir = Vector2.Zero;

            //set height restrictions if player attempts to make move that changes height abruptly
            if (Math.Abs(myMap.GetOverallHeight(vlad.Position) - myMap.GetOverallHeight(vlad.Position + moveDir)) > 10)
                moveDir = Vector2.Zero;

            //if movement exists, call move and apply animation
            if (moveDir.Length() != 0)
            {
                vlad.MoveBy((int)moveDir.X, (int)moveDir.Y);
                if (vlad.CurrentAnimation != animation)
                    vlad.CurrentAnimation = animation;
            }
            else
                vlad.CurrentAnimation = "Idle" + vlad.CurrentAnimation.Substring(4);
            #endregion

            #region CLAMPING PLAYER POSITION WITHIN MAP
            vlad.Position = new Vector2(
                MathHelper.Clamp(vlad.Position.X, vlad.DrawOffset.X + 64, camera.WorldWidth - vlad.DrawOffset.X),
                MathHelper.Clamp(vlad.Position.Y, vlad.DrawOffset.Y + 128, camera.WorldHeight - vlad.DrawOffset.Y));

            #endregion

            #region AUTO SCROLLING AS PLAYER APPROACH EDGE
            Vector2 testPos = camera.WorldToScreen(vlad.Position);

            if (testPos.X < 100)
                camera.Move(new Vector2(testPos.X - 100, 0));

            if (testPos.X > (camera.ViewWidth - 100))
                camera.Move(new Vector2(testPos.X - (camera.ViewWidth - 100), 0));

            if (testPos.Y < 100)
                camera.Move(new Vector2(0, testPos.Y - 100));

            if (testPos.Y > (camera.ViewHeight - 100))
                camera.Move(new Vector2(0, testPos.Y - (camera.ViewHeight - 100)));
            #endregion

            //update the map cell where player is
            vladMapPoint = myMap.WorldToMapCell(new Point((int)vlad.Position.X, (int)vlad.Position.Y));

            vlad.Update(gameTime);
            npc.Update(gameTime);



            #region UPDATE THE FIRST SQUARE LOCATION
            //Camera is intiially at 0,0 and camera can move around
            //we need to enables pan to the location of the map where the camera points
            //firstX and firstY are map square coordinates of the game map that camera points at
            firstSquare = new Vector2(camera.Location.X / Tile.TileStepX, camera.Location.Y / Tile.TileStepY);
            #endregion

            #region OTHER KEY TOGGLING
            //enable or disable debugging coordinates
            if (ks.IsKeyUp(Keys.Delete) && oldState.IsKeyDown(Keys.Delete))
                EnableDebugging = !EnableDebugging;

            //quit core to menu
            if (ks.IsKeyUp(Keys.Q) && oldState.IsKeyDown(Keys.Q))
                context.changeState(typeof(GameMenu));

            //Sub-Process Stack Operations
            if (this.IsEmptySubProcessStack())
            {
                //Show MessageBox 1
                if (ks.IsKeyDown(Keys.S))
                {
                    GameMessageBox message = context.getMessageBox("タイルエンジンへようこそ。このメッセージを消す場合はBを押してください。", "メッセージ");
                    this.PushProcess(message);
                }

                //Show 2 MessageBoxes
                if (ks.IsKeyDown(Keys.A))
                {
                    GameMessageBox message2 = context.getMessageBox("タイルエンジン著作者：大朏　哲明", "メッセージ", 100,150);
                    GameMessageBox message = context.getMessageBox("タイルエンジンへようこそ。このメッセージを消す場合はBを押してください。","メッセージ",100,150);

                    this.PushProcess(message2);
                    this.PushProcess(message);
                }
            }
            #endregion


            oldState = ks;
            oldMouseState = ms;

            base.Update(gameTime, context);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            #region PREPARATION
            //get the first rendering square map cell coordinates
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //compute the max depth count to draw any map (for 50x50 map this would be (51 + 51*64)*10=33150)
            float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;
            #endregion


            //Set up sprite batch. Tells XNA/Monogame that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            #region DRAW TILES
            //zig-zag rendering approach
            //http://stackoverflow.com/questions/892811/drawing-isometric-game-worlds


            //loop through map cells given current screenview
            for (int y = 0; y < squaresDown; y++)
            {
                //for supporting hexagonal and isometric maps, odd row must be offset
                int rowOffset = ((firstY + y) % 2 == 1) ? rowOffset = Tile.OddRowXOffset : 0;

                for (int x = 0; x < squaresAcross; x++)
                {
                    //controlling the depth of isometric tiles depending on the x,y location
                    int mapx = (firstX + x);
                    int mapy = (firstY + y);
                    //get the depth offset depending on the mapx and mapy
                    //increasing mapy primarily decreases depthOffset, followed by increasing mapx decreasing depthOffset slightly
                    float depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);

                    //if mapx hits map size don't render tiles
                    if (mapx >= myMap.MapWidth || mapy >= myMap.MapHeight)
                        continue;

                    #region DRAW BASE TILES
                    //draw base tiles
                    foreach (int tileID in myMap.Rows[mapy].Columns[mapx].BaseTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY)),
                            //get source tile. 
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            1.0f);
                    }
                    #endregion

                    #region STACKED HEIGHT TILES
                    //draw height tiles
                    //keep track of how many heigh tiles drawn
                    int heightRow = 0;
                    foreach (int tileID in myMap.Rows[mapy].Columns[mapx].HeightTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset))),
                            //get tile
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen (the value of heightRowDepthMod
                            depthOffset - ((float)heightRow * heightRowDepthMod));
                        heightRow++;
                    }
                    #endregion

                    #region DRAW TOPPER TILES (SKINS)
                    //draw topper tiles
                    foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].TopperTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset))),
                            //get source tile
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((float)heightRow * heightRowDepthMod));
                    }
                    #endregion

                    #region DETERMINE DRAW DEPTH OF PLAYER
                    if ((mapx == vladMapPoint.X) && (mapy == vladMapPoint.Y))
                        vlad.DrawDepth = depthOffset - (float)(heightRow + 2) * heightRowDepthMod;
                    #endregion

                    #region DRAW MULTI SIZE TILES
                    //draw multi size tiles
                    foreach (Tuple<int, int, int, int> tile in myMap.Rows[mapy].Columns[mapx].MultiSizeTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset - (tile.Item2 * Tile.MultiSizeTileOffset),
                                mapy * Tile.TileStepY - (tile.Item3 * Tile.MultiSizeTileOffset) - (tile.Item4 * Tile.HeightTileOffset))),
                            Tile.GetSourceRectangle(tile.Item1),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((float)tile.Item4 * heightRowDepthMod));
                    }
                    #endregion

                    #region DEBUGGING
                    //debugging tile draw location

                    if (EnableDebugging)
                        spriteBatch.DrawString(
                            pericles6,
                            mapx.ToString() + ", " + mapy.ToString(),
                            camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset + 24,
                                                             mapy * Tile.TileStepY + 48)),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    #endregion
                }
            }
            #endregion

            #region DRAW PLAYER
            //draw player according to where he's standing on
            vlad.Draw(spriteBatch,camera, 0, -myMap.GetOverallHeight(vlad.Position));
            #endregion

            #region DRAW HILIGHT LOCATION OF MOUSE
            Vector2 hilightLoc = camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            Point hilightPoint = myMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));

            //calculate hilight row offset
            int hilightrowOffset = ((hilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

            spriteBatch.Draw(
                hilight,
                camera.WorldToScreen(
                    new Vector2(
                        hilightPoint.X * Tile.TileStepX + hilightrowOffset,
                //add 2 as image is only half of actual tiles
                        (hilightPoint.Y + 2) * Tile.TileStepY)),
                new Rectangle(0, 0, 64, 32),
                Color.White * 0.3f,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            #endregion

            #region DRAW IN GAME MESSAGES
            spriteBatch.DrawString(
                            snippets14,
                            "Tile Engine Ver 0.8",
                            new Vector2(500, 390),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
            #endregion

            #region DRAW NPCS
            npc.Draw(spriteBatch,camera, 0, 0);
            #endregion

            spriteBatch.End();

            #region DRAW SUB PROCESS RENDERS
            base.Render(gameTime, spriteBatch, context);
            #endregion
        }
    }
}
