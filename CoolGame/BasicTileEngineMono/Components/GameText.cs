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
    public class GameText : GameProcess, ICloneable
    {
        protected SpriteFont snippets14;
        //Screen Coordinates
        Point Position;

        protected float TransparencyFactor { get; set; }
        protected float maxLineWidth { get; set; }

        public string TextContent { get; set; }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            snippets14 = Content.Load<SpriteFont>(@"Fonts\Snippets14");
        }

        public override void Update(GameTime gameTime, Context context)
        {
            base.Update(gameTime, context);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                            snippets14,
                            this.TextContent,
                            new Vector2(Position.X, Position.Y),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
            spriteBatch.End();

            base.Render(gameTime, spriteBatch, context);
        }

        public void Set(string TextContent, int X, int Y)
        {
            this.TextContent = TextContent;
            this.Position.X = X;
            this.Position.Y = Y;
        }
        public object Clone()
        {
            this.ID++;
            return this.MemberwiseClone();
        }
    }
}
