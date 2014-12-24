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
        AbstractMonoGameProcessFactory getFactory();
        GameMessageBox getMessageBox(string Content,string Title = "Message:", int X = 100, int Y = 100);
    }


    public abstract class GameProcess
    {
        protected bool IsAlive = true;
        private Stack<GameProcess> ProcessStack = new Stack<GameProcess>();
        private Queue<GameProcess> ProcessQueue = new Queue<GameProcess>();

        public GameProcess()
        {
        }
        protected bool IsEmptySubProcessStack()
        {
            return ProcessStack.Count > 0 ? false : true;
        }
        protected bool IsEmptySubProcessQueue()
        {
            return ProcessQueue.Count > 0 ? false : true;
        }

        protected void PushProcess(GameProcess gameProcess)
        {
            ProcessStack.Push(gameProcess);
        }
        protected void PopProcess()
        {
            ProcessStack.Pop();
        }
        protected void Enqueue(GameProcess gameProcess)
        {
            ProcessQueue.Enqueue(gameProcess);
        }
        protected void Dequeue(GameProcess gameProcess)
        {
            ProcessQueue.Dequeue();
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
                    PopProcess();
            }

            foreach (GameProcess g in ProcessQueue)
                g.Update(gameTime, context);
        }
        public virtual void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            //Sub-Process Stack Render Ops
            if (!IsEmptySubProcessStack())
                ProcessStack.Peek().Render(gameTime, spriteBatch, context);

            foreach (GameProcess g in ProcessQueue)
                g.Render(gameTime, spriteBatch,context);
        }
    }

}
