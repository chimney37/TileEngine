using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BasicTileEngineMono.Components
{
    /// <summary>
    /// Factory Design Pattern
    /// </summary>
    public abstract class AbstractMonoGameProcessFactory
    {
        public static AbstractMonoGameProcessFactory GetFactory(string classname)
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
        public abstract GameMessageBox GameMessageBox(string content, string title = "Message:", int x = 100, int y = 100);
        public abstract GameText GameText(string text, int x, int y);
    }

    public class GameProcessFactory : AbstractMonoGameProcessFactory
    {
        private readonly List<GameProcess> _processList = new List<GameProcess>();

        #region FACTORY METHODS
        public override void Create()
        {
            _processList.Add(new GameCore());
            _processList.Add(new GameMapEditor());
            _processList.Add(new GameMenu());
            _processList.Add(new GameMessageBox());
            _processList.Add(new GameText());
        }

        public override void Initialize(Game game)
        {
            foreach (var g in _processList)
                g.Initialize(game);
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            foreach (var g in _processList)
                g.LoadContent(content, graphics);   
        }

        public override GameProcess GetGameProcess(Type typeOfGameProcess)
        {
            return _processList.Find(x => x.GetType() == typeOfGameProcess);
        }

        public override GameMessageBox GameMessageBox(string content, string title = "Message:", int x = 100, int y = 100)
        {
            GameMessageBox messageBox = (GameMessageBox)((GameMessageBox)GetGameProcess(typeof(GameMessageBox))).Clone();
            messageBox.Set(content, title, x, y);
            return messageBox;
        }
        public override GameText GameText(string text, int x, int y)
        {
            GameText gameText = (GameText)((GameText)GetGameProcess(typeof(GameText))).Clone();
            gameText.Set(text, x, y);
            return gameText;
        }
        #endregion
    }
}
