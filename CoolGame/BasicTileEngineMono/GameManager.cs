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
    //To access game controller : changing states, getting game functionality etc.
    public interface Context
    {
        void changeState(Type gameProcess);
        GameMessageBox getMessageBox(string Content,string Title = "Message:", int X = 100, int Y = 100);
    }


    public abstract class GameProcess
    {
        protected bool IsAlive = true;
        private Stack<GameProcess> ProcessStack = new Stack<GameProcess>();

        public GameProcess()
        {
        }
        protected bool IsEmptySubProcessStack()
        {
            return ProcessStack.Count > 0 ? false : true;
        }
        protected void PushProcess(GameProcess gameProcess)
        {
            ProcessStack.Push(gameProcess);
        }
        protected void PopProcess()
        {
            ProcessStack.Pop();
        }

        public abstract void Initialize(Game game);
        public abstract void LoadContent(ContentManager Content, GraphicsDeviceManager graphics);
        public virtual void Update(GameTime gameTime, Context context)
        {
            //Sub-Process Stack Operations
            if (!IsEmptySubProcessStack())
            {
                ProcessStack.Peek().Update(gameTime, context);

                if (!ProcessStack.Peek().IsAlive)
                    ProcessStack.Pop();
            }
        }
        public virtual void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            //Sub-Process Stack Render Ops
            if (!IsEmptySubProcessStack())
                ProcessStack.Peek().Render(gameTime, spriteBatch, context);
        }
    }

}
