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

    public class GameMessageBox : GameProcess, ICloneable
    {
        public int ID { get; set; }
        KeyboardState oldKBState;
        MouseState oldMouseState;

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

        Point firstMouseClickPosition = new Point(0, 0);
        protected Vector2 ClickOffset = new Vector2(0, 0);
        protected bool FirstTime = true;

        Rectangle destinationRectangle;

        public override void Initialize(Game game)
        {
            this.TextContent = "";
            this.Title = "";
            this.X = 100;
            this.Y = 100;

            FontColor = Color.Black;
            TransparencyFactor = 0.5f;
            FontSizeScale = 0.5f;

            //intialize old keyboard state
            oldKBState = Keyboard.GetState();
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            MessageBoxTexture = Content.Load<Texture2D>(@"Textures\UI\MessageBox1");
            Messagerical = Content.Load<SpriteFont>(@"Fonts\MessagFont");
        }

        public override void Update(GameTime gameTime, Context context)
        {
            //Reference:
            //http://stackoverflow.com/questions/9712932/2d-xna-game-mouse-clicking
            //Exit this GameProcess, back
            KeyboardState ks = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            var mousePosition = new Point(mouseState.X, mouseState.Y);
            
            //check if key pressed
            if (ks.IsKeyUp(Keys.B) && oldKBState.IsKeyDown(Keys.B))
                this.IsAlive = false;

            //detect first mouse click on box
            if (destinationRectangle.Contains(mousePosition) &&
                mouseState.LeftButton == ButtonState.Pressed &&
                FirstTime)
            {
                firstMouseClickPosition = mousePosition;
                ClickOffset = new Vector2(firstMouseClickPosition.X, firstMouseClickPosition.Y) - new Vector2(X, Y);
                FirstTime = false;
            }

            //check if mouse clicked on location
            if (destinationRectangle.Contains(mousePosition) &&
                mouseState.LeftButton == ButtonState.Released &&
                oldMouseState.LeftButton == ButtonState.Pressed &&
                !MouseMoved(firstMouseClickPosition,mousePosition))
                this.IsAlive = false;

       
            //TODO: implement draggable messageBox, need to update firstMouseClickPosition
            if (destinationRectangle.Contains(mousePosition) &&
                mouseState.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Pressed)
            {
                this.X = mousePosition.X - (int)ClickOffset.X;
                this.Y = mousePosition.Y - (int)ClickOffset.Y;

                destinationRectangle.X = this.X;
                destinationRectangle.Y = this.Y;
            }

            //reset firstMouseClickLocation
            if (mouseState.LeftButton == ButtonState.Released &&
                oldMouseState.LeftButton == ButtonState.Pressed)
            {
                FirstTime = true;
            }

            oldKBState = ks;
            oldMouseState = mouseState;
        }

        private bool MouseMoved(Point one, Point two)
        {
            if (Math.Abs(one.X - two.X) > 5 || Math.Abs(one.Y - two.Y) > 5)
                return true;
            else
                return false;
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            //get message box dimensions
            Width = MessageBoxTexture.Width;
            Height = MessageBoxTexture.Height;

            this.maxLineWidth = this.Width * 0.9f;

            Rectangle sourceRectangle = new Rectangle(0, 0, Width, Height);
            destinationRectangle = new Rectangle(X, Y, Width, Height);

            //adjust text length and new lines
            string TextContent = WrapJPText();


            //draw the messagebox frame
            spriteBatch.Begin();
            spriteBatch.Draw(MessageBoxTexture, destinationRectangle, sourceRectangle, Color.White * TransparencyFactor);
            spriteBatch.End();


            spriteBatch.Begin();
            spriteBatch.DrawString(
                            Messagerical,
                            "ID: " + ID + ":" + this.Title,
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
            this.ID++;
            return this.MemberwiseClone();
        }
    }

}
