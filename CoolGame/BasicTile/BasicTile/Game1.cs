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
        int squaresAcross = 10;
        int squaresDown = 10;

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

            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part1_tileset");

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

            //move the camera around
            //note: clamp to keep X and Y values within pre-defined ranges
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (myMap.MapWidth - squaresAcross) * 32);

            if (ks.IsKeyDown(Keys.Right))
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (myMap.MapWidth - squaresAcross) * 32);

            if (ks.IsKeyDown(Keys.Up))
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (myMap.MapHeight - squaresDown) * 32);

            if (ks.IsKeyDown(Keys.Down))
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (myMap.MapHeight - squaresDown) * 32);

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
            spriteBatch.Begin();

            //each tile is 32x32 px
            //this points to the map square coordinates that points tot he tiles that camera is pointing at
            Vector2 firstSquare = new Vector2(Camera.Location.X / 32, Camera.Location.Y / 32);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //if the camera moves in increments of less than one tile, we need to offset it by the amount the camera shifted
            Vector2 squareOffset = new Vector2(Camera.Location.X % 32, Camera.Location.Y % 32);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < squaresDown; y++)
            {
                for (int x = 0; x < squaresAcross; x++)
                {
                    spriteBatch.Draw(
                        Tile.TileSetTexture,

                        //1st rectangle determines where on the screen the tile will be drawn
                        //offset moves the tile drawing by the amount calculated to account for camera being between whole tile markers
                        new Rectangle((x * 32) - offsetX, (y * 32) - offsetY, 32, 32),

                        //Use the get the kind of tile. 
                        //rows and columns are offset by the first X and Y.
                        Tile.GetSourceRectangle(myMap.Rows[y + firstY].Columns[x + firstX].TileID),
                        Color.White);
                }
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}