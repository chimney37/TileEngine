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
using BasicTileEngineMono.Components;

namespace BasicTileEngineMono
{

    public class GameMessageBox : GameProcess, ICloneable
    {
        GameInput gameInput;

        Texture2D MessageBoxTexture;
        protected int Width { get; set; }
        protected int Height { get; set; }
        protected float TransparencyFactor { get; set; }
        protected float maxLineWidth { get; set; }

        protected SpriteFont Messagerical;
        protected Color FontColor { get; set; }
        protected float FontSizeScale { get; set; }


        public string TextContent { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2 ClickOffset { get; set; }
        public Rectangle DestinationRectangle;
        private Rectangle SourceRectangle;

        public override void Initialize(Game game)
        {
            this.TextContent = "";
            this.Title = "";
            this.X = 100;
            this.Y = 100;

            FontColor = Color.Black;
            TransparencyFactor = 0.5f;
            FontSizeScale = 0.5f;

            //initialize input handler
            gameInput = new GameInput();
            gameInput._buttonEnter_PR = new MessageBoxCloseCommand();
            gameInput._mouseLeft_PR_S = new MessageBoxCloseOnClickCommand(gameInput);
            gameInput._mouseLeft_P = new MessageBoxGetClickOffsetCommand(gameInput);
            gameInput._mouseLeft_P_Hld = new MessageBoxMoveCommand(gameInput);
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            MessageBoxTexture = Content.Load<Texture2D>(@"Textures\UI\MessageBox1");
            Messagerical = Content.Load<SpriteFont>(@"Fonts\MessagFont");

            //get message box dimensions
            this.Width = MessageBoxTexture.Width;
            this.Height = MessageBoxTexture.Height;
            SourceRectangle = new Rectangle(0, 0, Width, Height);

            this.maxLineWidth = this.Width * 0.9f;
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            gameInput.HandleInput(ref CommandQueue);
            while (CommandQueue.Count() > 0)
            {
                Command cmd = CommandQueue.Dequeue();

                if (cmd != null)
                {
                    cmd.Execute(this);
                }
            }
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            this.DestinationRectangle = new Rectangle(X, Y, Width, Height);

            //adjust text length and new lines
            string TextContent = WrapJPText();


            //draw the messagebox frame
            spriteBatch.Begin();
            spriteBatch.Draw(MessageBoxTexture, DestinationRectangle, SourceRectangle, Color.White * TransparencyFactor);
            spriteBatch.End();


            spriteBatch.Begin();
            spriteBatch.DrawString(
                            Messagerical,
                            "ID: " + Id + ":" + this.Title,
                            new Vector2(X + 10, Y + 19),
                            FontColor,
                            0f,
                            Vector2.Zero,
                            FontSizeScale,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.DrawString(
                            Messagerical,
                            TextContent,
                            new Vector2(X + 15, Y + 35),
                            FontColor,
                            0f,
                            Vector2.Zero,
                            FontSizeScale,
                            SpriteEffects.None,
                            0.0f);

            spriteBatch.End();
        }

        //Wraps JP Text (should be able to use for CJK text)
        //Reference: http://stackoverflow.com/questions/15986473/how-do-i-implement-word-wrap
        private string WrapJPText()
        {
            char[] words = this.TextContent.ToCharArray();
            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;
            foreach (char c in words)
            {
                Vector2 charLength = Messagerical.MeasureString(c.ToString()) * FontSizeScale;
                if (lineWidth + charLength.X >= maxLineWidth)
                {
                    sb.Append("\n" + c);
                    lineWidth = charLength.X;
                }
                else
                {
                    sb.Append(c);
                    lineWidth += charLength.X;
                }
            }

            string TextContent = sb.ToString();
            return TextContent;
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
            this.Id++;
            return this.MemberwiseClone();
        }
    }

}
