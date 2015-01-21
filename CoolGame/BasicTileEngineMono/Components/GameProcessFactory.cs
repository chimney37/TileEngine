using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using BasicTileEngineMono.Components;

namespace BasicTileEngineMono
{
    /// <summary>
    /// Factory Design Pattern
    /// </summary>
    public abstract class AbstractMonoGameProcessFactory
    {
        public static AbstractMonoGameProcessFactory getFactory(string classname)
        {
            AbstractMonoGameProcessFactory factory = null;
            try
            {
                // http://msdn.microsoft.com/en-us/library/wccyzw83(v=vs.110).aspx
                // create an instance of the specified type using the type's default constructor
                factory = (AbstractMonoGameProcessFactory)Activator.CreateInstance(Type.GetType(classname));
            }
            catch (Exception e)
            {
                Trace.WriteLine("Cannot find class : {0}", e.InnerException.Message);
            }

            return factory;
        }

        public abstract void Create();
        public abstract void Initialize(Game game);
        public abstract void LoadContent(ContentManager content, GraphicsDeviceManager graphics);
        public abstract GameProcess GetGameProcess(Type typeOfGameProcess);
        public abstract GameMessageBox GameMessageBox(string Content, string Title = "Message:", int X = 100, int Y = 100);
        public abstract GameText GameText(string Text, int X, int Y);
    }

    public class GameProcessFactory : AbstractMonoGameProcessFactory
    {
        private List<GameProcess> ProcessList = new List<GameProcess>();

        #region FACTORY METHODS
        public override void Create()
        {
            ProcessList.Add(new GameCore());
            ProcessList.Add(new GameMapEditor());
            ProcessList.Add(new GameMenu());
            ProcessList.Add(new GameMessageBox());
            ProcessList.Add(new GameText());
        }

        public override void Initialize(Game game)
        {
            foreach (GameProcess g in ProcessList)
                g.Initialize(game);
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            foreach (GameProcess g in ProcessList)
                g.LoadContent(content, graphics);   
        }

        public override GameProcess GetGameProcess(Type typeOfGameProcess)
        {
            return ProcessList.Find(x => x.GetType() == typeOfGameProcess);
        }

        public override GameMessageBox GameMessageBox(string Content, string Title = "Message:", int X = 100, int Y = 100)
        {
            GameMessageBox messageBox = (GameMessageBox)((GameMessageBox)GetGameProcess(typeof(GameMessageBox))).Clone();
            messageBox.Set(Content, Title, X, Y);
            return messageBox;
        }
        public override GameText GameText(string Text, int X, int Y)
        {
            GameText gameText = (GameText)((GameText)GetGameProcess(typeof(GameText))).Clone();
            gameText.Set(Text, X, Y);
            return gameText;
        }
        #endregion
    }
}
