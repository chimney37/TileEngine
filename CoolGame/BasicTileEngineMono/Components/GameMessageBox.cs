using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTileEngineMono.Components
{

    public class GameMessageBox : GameProcess, ICloneable
    {
        GameInput _gameInput;

        Texture2D _messageBoxTexture;
        protected int Width { get; set; }
        protected int Height { get; set; }
        protected float TransparencyFactor { get; set; }
        protected float MaxLineWidth { get; set; }

        protected SpriteFont Messagerical;
        protected Color FontColor { get; set; }
        protected float FontSizeScale { get; set; }


        public string TextContent { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2 ClickOffset { get; set; }
        public Rectangle DestinationRectangle;
        private Rectangle _sourceRectangle;

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
            _gameInput = new GameInput();
            _gameInput._buttonEnter_PR = new MessageBoxCloseCommand();
            _gameInput._mouseLeft_PR_S = new MessageBoxCloseOnClickCommand(_gameInput);
            _gameInput._mouseLeft_P = new MessageBoxGetClickOffsetCommand(_gameInput);
            _gameInput._mouseLeft_P_Hld = new MessageBoxMoveCommand(_gameInput);
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            _messageBoxTexture = content.Load<Texture2D>(@"Textures\UI\MessageBox1");
            Messagerical = content.Load<SpriteFont>(@"Fonts\MessagFont");

            //get message box dimensions
            this.Width = _messageBoxTexture.Width;
            this.Height = _messageBoxTexture.Height;
            _sourceRectangle = new Rectangle(0, 0, Width, Height);

            this.MaxLineWidth = this.Width * 0.9f;
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            _gameInput.HandleInput(ref CommandQueue);
            while (CommandQueue.Any())
            {
                Command cmd = CommandQueue.Dequeue();

                if (cmd != null)
                    cmd.Execute(this);
            }
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            this.DestinationRectangle = new Rectangle(X, Y, Width, Height);

            //adjust text length and new lines
            string textContent = WrapJpText();


            //draw the messagebox frame
            spriteBatch.Begin();
            spriteBatch.Draw(_messageBoxTexture, DestinationRectangle, _sourceRectangle, Color.White * TransparencyFactor);
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
                            textContent,
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
        private string WrapJpText()
        {
            char[] words = this.TextContent.ToCharArray();
            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;
            foreach (char c in words)
            {
                Vector2 charLength = Messagerical.MeasureString(c.ToString()) * FontSizeScale;
                if (lineWidth + charLength.X >= MaxLineWidth)
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

            return sb.ToString();
        }

        public void Set(string textContent, string title, int x, int y)
        {
            this.TextContent = textContent;
            this.Title = title;
            this.X = x;
            this.Y = y;
        }


        public object Clone()
        {
            this.Id++;
            return this.MemberwiseClone();
        }
    }

}
