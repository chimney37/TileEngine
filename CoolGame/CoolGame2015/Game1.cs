using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BasicTileEngineMono.Components;

namespace BasicTileEngineMono
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game, IContext
    {
        public GraphicsDeviceManager Graphics;
        SpriteBatch _spriteBatch;

        readonly AbstractMonoGameProcessFactory _gameFactory;
        GameProcess _currentState;

        public Game1()
            : base()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 800,
                PreferredBackBufferWidth = 1000
            };

            Content.RootDirectory = "Content";

            _gameFactory = AbstractMonoGameProcessFactory.getFactory("BasicTileEngineMono.GameProcessFactory");
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

            _gameFactory.Create();
            _gameFactory.Initialize(this);

            _currentState = _gameFactory.GetGameProcess(typeof(GameMenu));
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _gameFactory.LoadContent(Content, Graphics);
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
            _currentState.Update(gameTime, this);


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


            _currentState.Render(gameTime, _spriteBatch,this);

            base.Draw(gameTime);
        }


        public void ChangeState(Type gameProcess)
        {
            _currentState = _gameFactory.GetGameProcess(gameProcess);
        }
        public GameProcess GetCurrentState()
        {
            return _currentState;
        }
        public AbstractMonoGameProcessFactory GetFactory()
        {
            return _gameFactory;
        }

        public GameMessageBox GetMessageBox(string content,string title="Message:",int x=100, int y=100 )
        {
            return _gameFactory.GameMessageBox(content, title, x, y);
        }
    }
}