﻿#region Using Statements
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
    public class Game1 : Game, Context
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Stack<GameProcess> ProcessStack = new Stack<GameProcess>();
        private List<GameProcess> states = new List<GameProcess>();
        GameProcess currentState;

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

            states.Add(new GameMenu());
            states.Add(new GameCore());
            states.Add(new GameMessageBox());

            foreach (GameProcess g in states)
                g.Initialize(this);

            currentState = states.Find(x => x.GetType() == typeof(GameMenu));
           
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
            foreach (GameProcess g in states)
                g.LoadContent(this.Content, this.graphics);                     
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
            currentState.Update(gameTime, this);


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


            currentState.Render(gameTime, spriteBatch,this);

            base.Draw(gameTime);
        }

        public void changeState(Type state)
        {
            this.currentState = states.Find(x => x.GetType() == state);
        }

        public GameProcess getState(Type state)
        {
            return states.Find(x => x.GetType() == state);
        }

        public int stackCount()
        {
            return ProcessStack.Count;
        }

        public void popProcess()
        {
            ProcessStack.Pop();
        }

        public void pushProcess(Type GameProcess)
        {
            ProcessStack.Push(states.Find(x => x.GetType() == GameProcess));
        }
        public void Push(GameProcess gameProcess)
        {
            ProcessStack.Push(gameProcess);
        }
        public GameProcess Peek()
        {
            return ProcessStack.Peek();
        }

        public Stack<GameProcess> GetStack()
        {
            return ProcessStack;
        }
    }
}