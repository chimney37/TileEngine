#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace BasicTile
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

        //debugging tile locations on map
        SpriteFont pericles6;
        bool EnableDebugging = false;
        KeyboardState oldState;

        //highlighting tiles
        Texture2D hilight;

        //example of movable player
        Texture2D playerTexture;
        AnimatedSprite playerAnimatedSprite;
        Vector2 playerPosition = new Vector2(15, 15);
        #endregion

        //player character
        SpriteAnimation vlad;
        Point vladMapPoint;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //intialize old keyboard state
            oldState = Keyboard.GetState();

            //make mouse cursor visible within game window
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Load tiles
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");

            //load example player texture
            playerTexture = Content.Load<Texture2D>(@"Textures\Characters\SmileyWalk");
            playerAnimatedSprite = new AnimatedSprite(playerTexture, 4, 4);

            //load fonts
            pericles6 = Content.Load<SpriteFont>(@"Fonts\Pericles6");

            //initialize TileMap
            myMap = new TileMap(
                Content.Load<Texture2D>(@"Textures\TileSets\mousemap"),
                Content.Load<Texture2D>(@"Textures\TileSets\part9_slopemaps"));

            //initialize camera
            Camera.ViewWidth = this.graphics.PreferredBackBufferWidth;
            Camera.ViewHeight = this.graphics.PreferredBackBufferHeight;
            Camera.WorldWidth = ((myMap.MapWidth - 2) * Tile.TileStepX);
            Camera.WorldHeight = ((myMap.MapHeight - 2) * Tile.TileStepY);
            Camera.DisplayOffset = new Vector2(baseOffsetX, baseOffsetY);

            //intiliaze highlight
            hilight = Content.Load<Texture2D>(@"Textures\TileSets\hilight");

            //create sprite and animations for player
            vlad = new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48"));

            //TODO: maybe enuming the strings for runtime safety
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

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            string animation = "";

            KeyboardState ks = Keyboard.GetState();

            #region SET DIRECTION AND MOVEMENT VECTORS PER KEY PRESS TYPE
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
                MathHelper.Clamp(vlad.Position.X, vlad.DrawOffset.X + 64, Camera.WorldWidth - vlad.DrawOffset.X),
                MathHelper.Clamp(vlad.Position.Y, vlad.DrawOffset.Y + 128, Camera.WorldHeight - vlad.DrawOffset.Y));
            #endregion

            //get the map cell where player is
            vladMapPoint = myMap.WorldToMapCell(new Point((int)vlad.Position.X, (int)vlad.Position.Y));

            #region AUTO SCROLLING AS PLAYER APPROACH EDGE
            Vector2 testPos = Camera.WorldToScreen(vlad.Position);

            if (testPos.X < 100)
                Camera.Move(new Vector2(testPos.X - 100, 0));

            if (testPos.X > (Camera.ViewWidth - 100))
                Camera.Move(new Vector2(testPos.X - (Camera.ViewWidth - 100), 0));

            if (testPos.Y < 100)
                Camera.Move(new Vector2(0, testPos.Y - 100));

            if (testPos.Y > (Camera.ViewHeight - 100))
                Camera.Move(new Vector2(0, testPos.Y - (Camera.ViewHeight - 100)));

            #endregion

            vlad.Update(gameTime);
            
            #region TOGGLE DEBUGGING COORDINATES SHOW ON/OFF
            if (ks.IsKeyUp(Keys.Delete) && oldState.IsKeyDown(Keys.Delete))
                EnableDebugging = !EnableDebugging;
            oldState = ks;
            #endregion

            //update animation
            playerAnimatedSprite.Update();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            //Camera is intiially at 0,0 and camera can move around
            //we need to enables pan to the location of the map where the camera points
            //firstX and firstY are map square coordinates of the game map that camera points at
            Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.TileStepX, Camera.Location.Y / Tile.TileStepY);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //compute the max depth count to draw any map (for 50x50 map this would be (51 + 51*64)*10=33150)
            float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;

            //zig-zag rendering approach
            //http://stackoverflow.com/questions/892811/drawing-isometric-game-worlds

            //begin draw the tile map. Tells XNA that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

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
                            Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY)),
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
                            Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset))),
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
                            Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset))),
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
                            Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset - (tile.Item2 * Tile.MultiSizeTileOffset),
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
                            Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset + 24,
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

            #region DRAW PLAYER
            //draw player according to where he's standing on
            vlad.Draw(spriteBatch, 0, -myMap.GetOverallHeight(vlad.Position));
            #endregion

            #region DRAW HILIGHT LOCATION OF MOUSE
            Vector2 hilightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            Point hilightPoint = myMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));

            //calculate hilight row offset
            int hilightrowOffset = ((hilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

            spriteBatch.Draw(
                hilight,
                Camera.WorldToScreen(
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

            spriteBatch.End();

            #region DRAW NPCS
            playerAnimatedSprite.Draw(spriteBatch, playerPosition);
            #endregion

            base.Draw(gameTime);
        }
    }
}
