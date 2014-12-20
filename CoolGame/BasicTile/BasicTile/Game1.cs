using System;
using System.Collections.Generic;
using System.Linq;
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
        int squaresDown = 25;

        //for isometric map support
        int baseOffsetX = -32;
        int baseOffsetY = -64;
        float heightRowDepthMod = 0.0000001f;

        //debuggint tile locations on map
        SpriteFont pericles6;

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

            //draw the tile map
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //this points to the map square coordinates that points tot he tiles that camera is pointing at
            Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.TileStepX, Camera.Location.Y / Tile.TileStepY);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //
            float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;
            float depthOffset;

            //if the camera moves in increments of less than one tile, we need to offset it by the amount the camera shifted
            Vector2 squareOffset = new Vector2(Camera.Location.X % Tile.TileStepX, Camera.Location.Y % Tile.TileStepY);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < squaresDown; y++)
            {
                int rowOffset = 0;
                if ((firstY + y) % 2 == 1)
                    rowOffset = Tile.OddRowXOffset;

                for (int x = 0; x < squaresAcross; x++)
                {

                    int mapx = (firstX + x);
                    int mapy = (firstY + y);
                    depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);


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

#if DEBUG
                    //debugging tile draw location
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
#endif
                }
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
