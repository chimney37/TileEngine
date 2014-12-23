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

    public class GameMessageBox : GameMenu, ICloneable
    {
        public int ID { get; set; }
        KeyboardState oldState;

        public string TextContent { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override void Initialize(Game game)
        {
            this.TextContent = "";
            this.Title = "";
            this.X = 100;
            this.Y = 100;


            //intialize old keyboard state
            oldState = Keyboard.GetState();
            base.Initialize(game);
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            base.LoadContent(Content, graphics);
        }

        public override void Update(GameTime gameTime, Context context)
        {
            //Exit this GameProcess, back
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyUp(Keys.B) && oldState.IsKeyDown(Keys.B))
                this.IsAlive = false;

            oldState = ks;
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                            base.snippets14,
                            "Message: " + ID + ":" + this.Title,
                            new Vector2(X + 10, Y + 19),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.DrawString(
                            base.snippets14,
                            this.TextContent,
                            new Vector2(X + 15, Y + 29),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.End();
        }

        public void Set(string TextContent, string Title, int X, int Y)
        {
            this.TextContent = TextContent;
            this.Title = Title;
            this.X = X;
            this.Y = Y;
        }


        public object Clone()
        {
            this.ID++;
            return this.MemberwiseClone();
        }
    }

}
