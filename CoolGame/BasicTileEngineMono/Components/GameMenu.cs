using System.Linq;
using BasicTileEngineMono.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTileEngineMono.Components
{

    public class GameMenu : GameProcess
    {
        //Input Handler
        GameInput _gameInput;

        protected SpriteFont Snippets14;

        //Menu Text
        private const string VersionText = "Tile Engine Menu System Ver 0.2";
        private const string MenuText = "<Enter> Continue to Core Game\n" + "<E> Map Editor Mode\n" + "<A> About\n";


        public override void Initialize(Game game)
        {
            base.Initialize(game);
            _gameInput = new GameInput();

            //assign message box command to a specific key
            MessageBoxCommand messageBox = new MessageBoxCommand(this, "About", "これはタイルエンジンのデモです。著作者：大朏哲明", 200, 200);
            _gameInput.ButtonAPr = messageBox;
            _gameInput.ButtonEnterPr = new StateChangeToCommand<GameCore>(this);
            _gameInput.ButtonEPr = new StateChangeToCommand<GameMapEditor>(this);

            game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            Snippets14 = content.Load<SpriteFont>(@"Fonts\Snippets14");
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            //Check if anything on the Sub-Process Stack

            _gameInput.HandleInput(ref this.CommandQueue);
            while (CommandQueue.Any())
            {
                Command cmd = CommandQueue.Dequeue();

                if (cmd == null) continue;

                cmd.Execute(this);
                cmd.Execute(context);
            }

            base.Update(gameTime, context);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                            Snippets14,
                            VersionText,
                            new Vector2(0, 0),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.DrawString(
                            Snippets14,
                            MenuText,
                            new Vector2(10, 30),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.End();

            base.Render(gameTime, spriteBatch, context);
        }
    }

}
