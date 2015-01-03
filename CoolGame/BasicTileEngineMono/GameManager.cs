using System;
using System.Collections.Generic;
using System.Collections;
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
        GameProcess getCurrentState();
        void changeState(Type gameProcess);
        AbstractMonoGameProcessFactory getFactory();
        GameMessageBox getMessageBox(string Content,string Title = "Message:", int X = 100, int Y = 100);
    }


    public abstract class GameProcess
    {
        public int ID { get; set; }
        public GameTime GameTime { get; set; }

        public bool IsAlive = true;
        private Stack<GameProcess> ProcessStack = new Stack<GameProcess>();
        private Queue<GameProcess> ProcessQueue = new Queue<GameProcess>();
        private Dictionary<int, GameProcess> ProcessDict = new Dictionary<int, GameProcess>();

        protected KeyboardState ks = Keyboard.GetState();
        protected MouseState ms = Mouse.GetState();
        protected KeyboardState oldState;
        protected MouseState oldMouseState;
        //protected int prevMouseScrollValue;

        public GameProcess()
        {
        }
        public bool IsEmptySubProcessStack()
        {
            return ProcessStack.Count > 0 ? false : true;
        }
        public bool IsEmptySubProcessQueue()
        {
            return ProcessQueue.Count > 0 ? false : true;
        }

        public void Push(GameProcess gameProcess)
        {
            ProcessStack.Push(gameProcess);
        }
        public void Pop()
        {
            ProcessStack.Pop();
        }
        public GameProcess Peek()
        {
            return this.ProcessStack.Peek();
        }
        public void Enqueue(GameProcess gameProcess)
        {
            ProcessQueue.Enqueue(gameProcess);
        }
        public void Dequeue(GameProcess gameProcess)
        {
            if(ProcessQueue.Count() > 0)
                ProcessQueue.Dequeue();
        }
        public void Add(GameProcess gameProcess, int ProcessID)
        {
            if (!ProcessDict.ContainsKey(ProcessID))
                ProcessDict.Add(ProcessID, gameProcess);
        }
        public void Remove(int ProcessID)
        {
            if (ProcessDict.ContainsKey(ProcessID))
                ProcessDict.Remove(ProcessID);
        }


        public abstract void Initialize(Game game);
        public abstract void LoadContent(ContentManager Content, GraphicsDeviceManager graphics);
        public virtual void Update(GameTime gameTime, Context context)
        {
            this.GameTime = gameTime;

            //Sub-Process Stack Operations
            if (!IsEmptySubProcessStack())
            {
                ProcessStack.Peek().Update(gameTime, context);

                if (!ProcessStack.Peek().IsAlive)
                    Pop();
            }

            foreach (GameProcess g in ProcessQueue)
                g.Update(gameTime, context);

            foreach (KeyValuePair<int, GameProcess> kvp in ProcessDict)
                kvp.Value.Update(gameTime, context);
        }
        public virtual void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            //Sub-Process Stack Render Ops
            if (!IsEmptySubProcessStack())
                ProcessStack.Peek().Render(gameTime, spriteBatch, context);

            foreach (GameProcess g in ProcessQueue)
                g.Render(gameTime, spriteBatch,context);

            foreach (KeyValuePair<int, GameProcess> kvp in ProcessDict)
                kvp.Value.Render(gameTime, spriteBatch, context);
        }
    }

}
