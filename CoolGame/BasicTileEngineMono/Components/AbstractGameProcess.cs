using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace BasicTileEngineMono.Components
{
    //To access game controller : changing states, getting game functionality etc.
    public interface IContext
    {
        GameProcess GetCurrentState();
        void ChangeState(Type gameProcess);
        AbstractMonoGameProcessFactory GetFactory();
        GameMessageBox GetMessageBox(string content,string title = "Message:", int x = 100, int y = 100);
    }


    public abstract class GameProcess
    {
        public int Id { get; set; }
        public GameTime GameTime { get; set; }
        public IContext GameContext { get; set; }

        public bool IsAlive = true;
        //Event Queue for Commands
        public Queue<Command> CommandQueue = new Queue<Command>();

        //Stack, Queued and Dictionary game Processes
        private readonly Stack<GameProcess> _processStack = new Stack<GameProcess>();
        private readonly Queue<GameProcess> _processQueue = new Queue<GameProcess>();
        private readonly Dictionary<int, GameProcess> _processDict = new Dictionary<int, GameProcess>();

        protected KeyboardState Ks = Keyboard.GetState();
        protected MouseState Ms = Mouse.GetState();
        protected KeyboardState OldKs;
        protected MouseState OldMs;
        //protected int prevMouseScrollValue;

        protected GameProcess()
        {
        }
        public bool IsEmptySubProcessStack()
        {
            return _processStack.Count <= 0;
        }
        public bool IsEmptySubProcessQueue()
        {
            return _processQueue.Count <= 0;
        }

        public void Push(GameProcess gameProcess)
        {
            _processStack.Push(gameProcess);
        }
        public void Pop()
        {
            _processStack.Pop();
        }
        public GameProcess Peek()
        {
            return this._processStack.Peek();
        }
        public void Enqueue(GameProcess gameProcess)
        {
            _processQueue.Enqueue(gameProcess);
        }
        public void Dequeue(GameProcess gameProcess)
        {
            if(_processQueue.Any())
                _processQueue.Dequeue();
        }
        public void Add(GameProcess gameProcess, int processId)
        {
            if (!_processDict.ContainsKey(processId))
                _processDict.Add(processId, gameProcess);
        }
        public void Remove(int processId)
        {
            if (_processDict.ContainsKey(processId))
                _processDict.Remove(processId);
        }


        public virtual void Initialize(Game game)
        {
            //record the Game context
            this.GameContext = game as IContext;

            //make mouse visible
            game.IsMouseVisible = true;
        }
        public abstract void LoadContent(ContentManager content, GraphicsDeviceManager graphics);
        public virtual void Update(GameTime gameTime, IContext context)
        {
            //track the game Time
            this.GameTime = gameTime;

            //Sub-Process Stack Operations
            if (!IsEmptySubProcessStack())
            {
                _processStack.Peek().Update(gameTime, context);

                if (!_processStack.Peek().IsAlive)
                    Pop();
            }

            foreach (var g in _processQueue)
                g.Update(gameTime, context);

            foreach (var kvp in _processDict)
                kvp.Value.Update(gameTime, context);
        }
        public virtual void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            //Sub-Process Stack Render Ops
            if (!IsEmptySubProcessStack())
                _processStack.Peek().Render(gameTime, spriteBatch, context);

            foreach (var g in _processQueue)
                g.Render(gameTime, spriteBatch,context);

            foreach (var kvp in _processDict)
                kvp.Value.Render(gameTime, spriteBatch, context);
        }
    }

}
