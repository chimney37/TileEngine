using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BasicTile
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileMap myMap = new TileMap();
        int squaresAcross = 25;
        int squaresDown = 30;

        //for isometric map support
        //this isometric set, is by 64x64,but the image only occupies bottom 32 pixels of tile.
        //http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:4
        //Base offset controls the screen coordinate offset from the top left of the screen coordinates
        int baseOffsetX = 0;
        int baseOffsetY = -32;
        float heightRowDepthMod = 0.0000001f;

        //debugging tile locations on map
        SpriteFont pericles6;
        bool EnableDebugging = true;
        KeyboardState oldState;

        public Game1()
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

            oldState = Keyboard.GetState();

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

            //Load tile map
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");

            //load fonts
            pericles6 = Content.Load<SpriteFont>(@"Fonts\Pericles6");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            


            //move the camera around using keys
            //camera moves in increments of (+-2)
            //note: clamp to keep X and Y values within pre-defined ranges
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (myMap.MapWidth - squaresAcross) * Tile.TileWidth);

            if (ks.IsKeyDown(Keys.Right))
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (myMap.MapWidth - squaresAcross) * Tile.TileWidth);

            if (ks.IsKeyDown(Keys.Up))
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (myMap.MapHeight - squaresDown) * Tile.TileWidth);

            if (ks.IsKeyDown(Keys.Down))
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (myMap.MapHeight - squaresDown) * Tile.TileWidth);

            if (ks.IsKeyUp(Keys.Delete) && oldState.IsKeyDown(Keys.Delete))
                EnableDebugging = !EnableDebugging;


            oldState = ks;

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

            //camera moves by increments of a sub tile step
            //calculate offset by the amount the camera shifted
            //example, if camera is at (0.0), just draw tile at (0,0)
            //but if camera is pointing at halfway through a tile to the right (16,0)
            //we need to shift everything we draw 16 px to left, so right part of tile shows up at upper left of screen
            Vector2 squareOffset = new Vector2(Camera.Location.X % Tile.TileStepX, Camera.Location.Y % Tile.TileStepY);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            //compute the max depth count to draw any map (for 50x50 map this would be (51 + 51*64)*10=33150)
            float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;
            float depthOffset;

            //zig-zag rendering approach
            //http://stackoverflow.com/questions/892811/drawing-isometric-game-worlds
            //

            //draw the tile map. Tells XNA that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            for (int y = 0; y < squaresDown; y++)
            {
                //for supporting hexagonal and isometric maps, odd row must be offset
                int rowOffset = ((firstY + y) % 2 == 1) ? rowOffset = Tile.OddRowXOffset : 0;

                for (int x = 0; x < squaresAcross; x++)
                {
                    //controlling the depth of isometric tiles depending on the x,y location
                    int mapx = (firstX + x);
                    int mapy = (firstY + y);
                    depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);

                    #region DRAW BASE TILES
                    //draw base tiles
                    foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].BaseTiles)
                    {                       
                        spriteBatch.Draw(
                            Tile.TileSetTexture,

                            //1st rectangle determines where on the screen the tile will be drawn
                            //offset moves the tile drawing by the amount calculated to account for camera being between whole tile markers
                            new Rectangle((x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX,
                                (y * Tile.TileStepY) - offsetY + baseOffsetY,
                                Tile.TileWidth,
                                Tile.TileWidth),

                            //Use to get the kind of tile. 
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
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
                            new Rectangle(
                                (x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX,

                                //each time draw, need to move the y by the value of Tile.HeightTileOffset times how many times height tile drawn
                                (y * Tile.TileStepY) - offsetY + baseOffsetY - (heightRow * Tile.HeightTileOffset),
                                Tile.TileWidth,
                                Tile.TileHeight),
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
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
                            new Rectangle(
                                (x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX,
                                (y * Tile.TileStepY) - offsetY + baseOffsetY - (heightRow * Tile.HeightTileOffset),
                                Tile.TileWidth,
                                Tile.TileHeight),
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((float)heightRow * heightRowDepthMod));
                    }
                    #endregion

                    #region DRAW MULTI SIZE TILES
                    //draw multi size tiles
                    foreach(Tuple<int,int,int,int> tile in myMap.Rows[y + firstY].Columns[x + firstX].MultiSizeTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Rectangle(
                                //we need to offset by both x and y depending on which part of object is drawn
                                (x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX - (tile.Item2 * Tile.MultiSizeTileOffset),
                                (y * Tile.TileStepY) - offsetY + baseOffsetY - (tile.Item3 * Tile.MultiSizeTileOffset) - (tile.Item4 * Tile.HeightTileOffset),
                                Tile.TileWidth,
                                Tile.TileHeight),
                            Tile.GetSourceRectangle(tile.Item1),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            //
                            depthOffset - ((float)tile.Item4 * heightRowDepthMod));
                    }
                    #endregion

                    #region DEBUGGING
                    //debugging tile draw location

                    if(EnableDebugging)
                        spriteBatch.DrawString(pericles6, 
                            (x + firstX).ToString() + ", " + (y + firstY).ToString(),
                            new Vector2((x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX + 24,
                                        (y * Tile.TileStepY) - offsetY + baseOffsetY + 48), 
                                        Color.White, 
                                        0f, 
                                        Vector2.Zero, 
                                        1.0f, 
                                        SpriteEffects.None, 
                                        0.0f);
                    #endregion
                }
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
