using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTileEngineMono.Components
{
    public class GameText : GameProcess, ICloneable
    {
        protected SpriteFont Snippets14;
        //Screen Coordinates
        Point _position;

        protected float TransparencyFactor { get; set; }
        protected float MaxLineWidth { get; set; }

        public string TextContent { get; set; }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            Snippets14 = content.Load<SpriteFont>(@"Fonts\Snippets14");
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                            Snippets14,
                            this.TextContent,
                            new Vector2(_position.X, _position.Y),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
            spriteBatch.End();

            base.Render(gameTime, spriteBatch, context);
        }

        public void Set(string textContent, int X, int Y)
        {
            this.TextContent = textContent;
            this._position.X = X;
            this._position.Y = Y;
        }
        public object Clone()
        {
            this.Id++;
            return this.MemberwiseClone();
        }
    }
}
