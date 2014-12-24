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
        protected SpriteFont snippets14;

        public override void Initialize(Game game)
        {
            game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            snippets14 = Content.Load<SpriteFont>(@"Fonts\Snippets14");
        }

        public override void Update(GameTime gameTime, Context context)
        {
            //Check if anything on the Sub-Process Stack
            if (this.IsEmptySubProcessStack())
            {
                //State Changes and Sub-Processes
                KeyboardState ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.Enter))
                    context.changeState(typeof(GameCore));

                if (ks.IsKeyDown(Keys.S))
                {
                    GameMessageBox message = context.getFactory().GameMessageBox("Message Box Example:");
                    this.PushProcess(message);
                }

                if (ks.IsKeyDown(Keys.A))
                {
                    GameMessageBox message = context.getFactory().GameMessageBox("Message Box Example:");
                    this.PushProcess(message);

                    GameMessageBox message2 = context.getFactory().GameMessageBox("Message Box Example:");
                    message2.X = 100;
                    message2.Y = 150;

                    this.PushProcess(message2);
                }
            }

            base.Update(gameTime, context);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            spriteBatch.Begin();
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

            spriteBatch.DrawString(
                            snippets14,
                            "Press Enter to Continue",
                            new Vector2(450, 370),
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
