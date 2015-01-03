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

    public class GameMenu : GameProcess
    {
        //Input Handler
        GameInput gameInput;

        protected SpriteFont snippets14;

        //Menu Text
        string VersionText = "Tile Engine Menu System Ver 0.2";
        string MenuText = "<Enter> Continue to Core Game\n" +
                            "<E> Map Editor Mode\n" +
                            "<A> About\n";
                            

        public override void Initialize(Game game)
        {
            gameInput = new GameInput();

            //assign message box command to a specific key
            MessageBoxCommand messageBox = new MessageBoxCommand(this, "About", "これはタイルエンジンのデモです。著作者：大朏哲明", 200, 200);
            gameInput._buttonA_PR = messageBox;
            gameInput._buttonEnter_PR = new StateChangeToCommand<GameCore>(this);
            gameInput._buttonE_PR = new StateChangeToCommand<GameMapEditor>(this);

            game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            snippets14 = Content.Load<SpriteFont>(@"Fonts\Snippets14");
        }

        public override void Update(GameTime gameTime, Context context)
        {
            //Check if anything on the Sub-Process Stack

            Queue<Command> cmds = gameInput.HandleInput();
            foreach (Command cmd in cmds)
            {
                if (cmd != null)
                {
                    cmd.Execute(this);
                    cmd.Execute(context);
                }
            }

            base.Update(gameTime, context);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                            snippets14,
                            VersionText,
                            new Vector2(0, 0),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.DrawString(
                            snippets14,
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
