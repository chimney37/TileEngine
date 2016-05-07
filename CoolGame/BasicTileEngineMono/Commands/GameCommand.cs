using System;
using System.Diagnostics;
using System.Linq;
using BasicTileEngineMono.Components;
using Microsoft.Xna.Framework;

namespace BasicTileEngineMono.Commands
{
    //the basis for Command pattern
    public abstract class Command
    {
        public abstract void Execute(Object obj);
        public virtual void Undo() { }
    }

    //Zooming Commands
    public class ZoomInCommand: Command
    {
        public override void Execute(Object obj)
        {
            //http://stackoverflow.com/questions/4926677/c-sharp-as-cast-vs-classic-cast

            if (obj is Camera)
            {
                Camera camera = (Camera)obj;
                camera.Scale += 0.1f;
            }
        }
    }
    public class ZoomOutCommand : Command
    {
        public override void Execute(Object obj)
        {
            if (obj is Camera)
            {
                Camera camera = (Camera)obj;
                camera.Scale -= 0.1f;
            }
        }
    }

    //Debugging Commands
    public class DebuggingToggleCommand : Command
    {
        public override void Execute(object obj)
        {
            if (obj is GameCore)
            {
                GameCore gameCore = (GameCore)obj;
                gameCore.EnableDebugging = !gameCore.EnableDebugging;

            }
        }
    }

    //State Changer Commands <T> target state
    public class StateChangeToCommand<T> : Command
    {
        readonly GameProcess _gameProcess;

        public StateChangeToCommand(GameProcess gameProcess)
        {
            _gameProcess = gameProcess;
        }

        public override void Execute(object obj)
        {
            //http://stackoverflow.com/questions/4963160/how-to-determine-if-a-type-implements-an-interface-with-c-sharp-reflection
            if(obj is IContext)
            {
                //only if blocking stack is empty
                if (_gameProcess.IsEmptySubProcessStack())
                {
                    IContext context = (IContext)obj;
                    context.ChangeState(typeof(T));
                }
            }
        }
    }
    
    //Message Box Commands
    public class MessageBoxCommand : Command
    {
        private GameProcess gameProcess;
        private string Title {get;set;}
        private string Content {get;set;}
        private int X {get;set;}
        private int Y {get;set;}

        public MessageBoxCommand(GameProcess gameProcess, string Title, string Content, int X, int Y)
        {
            this.gameProcess = gameProcess;
            this.Title = Title;
            this.Content =Content;
            this.X = X;
            this.Y = Y;
        }

        public MessageBoxCommand(GameProcess gameProcess, string Title, string Content, Vector2 ScreenPos)
        {
            this.gameProcess = gameProcess;
            this.Title = Title;
            this.Content = Content;
            this.X = (int)ScreenPos.X;
            this.Y = (int)ScreenPos.Y;
        }

        public override void Execute(object obj)
        {
            if(obj is IContext)
            {
                GameMessageBox message = (obj as IContext).GetFactory().GameMessageBox(Content, Title, X, Y);
                gameProcess.Push(message);
            }
        }
    }
    public class MessageBoxCloseCommand : Command
    {
        public override void Execute(object obj)
        {
            if(obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                mbox.IsAlive = false;
            }
        }
    }
    public class MessageBoxCloseOnClickCommand : MessageBoxCloseCommand
    {
        GameInput input;
        public MessageBoxCloseOnClickCommand(GameInput input)
        {
            Debug.Assert(input != null);

            this.input = input; 
        }

        public override void Execute(object obj)
        {
            if (obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                if(mbox.DestinationRectangle.Contains(input.MousePosition))
                    base.Execute(obj);
            }
        }
    }
    public class MessageBoxGetClickOffsetCommand : Command
    {
        readonly GameInput _input;
        public MessageBoxGetClickOffsetCommand(GameInput input)
        {
            _input = input;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                if (mbox.DestinationRectangle.Contains(_input.MousePosition))
                {
                    mbox.ClickOffset = new Vector2(_input.FirstMouseClickPosition.X, _input.FirstMouseClickPosition.Y) - new Vector2(mbox.X, mbox.Y);
                }
            }
        }
    }
    public class MessageBoxMoveCommand : Command
    {
        readonly GameInput _input;
        public MessageBoxMoveCommand(GameInput input)
        {
            _input = input;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                if (mbox.DestinationRectangle.Contains(_input.MousePosition))
                {
                    mbox.X = _input.MousePosition.X - (int)mbox.ClickOffset.X;
                    mbox.Y = _input.MousePosition.Y - (int)mbox.ClickOffset.Y;

                    mbox.DestinationRectangle.X = mbox.X;
                    mbox.DestinationRectangle.Y = mbox.Y;
                }
            }
        }
    }

    //Marking Commands
    public class MarkWorldPointCommand : Command
    {
        readonly GameInput _input;

        public MarkWorldPointCommand(GameInput input)
        {
            Debug.Assert(input != null);
            _input = input;
        }

        public override void Execute(object obj)
        {
            if(obj is GameCore)
            {
                GameCore gCore = obj as GameCore;

                Vector2 hilightLoc = gCore.GameCamera.ScreenToWorld(new Vector2(_input.MousePosition.X, _input.MousePosition.Y));
                //get map cell coordinates of mouse point in Update

                gCore.MapPoints.Dequeue();
                gCore.MapPoints.Enqueue(gCore.GameMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y)));
            }
        }
    }

    //Distance Finder Command
    public class CalculateDistanceDebuggerCommand : Command
    {
        readonly GameCore _gCore;
        public CalculateDistanceDebuggerCommand(GameCore gCore)
        {
            _gCore = gCore;
        }

        public override void Execute(object obj)
        {
            if(obj is IContext)
            {
                MessageBoxCommand cmd = new MessageBoxCommand(
                    _gCore,
                    "L0 Tile Distance:",
                    string.Format("({0},{1}) -> ({2},{3}) Distance={4}",
                    _gCore.MapPoints.ElementAt(0).X,
                    _gCore.MapPoints.ElementAt(0).Y,
                    _gCore.MapPoints.ElementAt(1).X,
                    _gCore.MapPoints.ElementAt(1).Y,
                    TileMap.L0TileDistance(_gCore.MapPoints.ElementAt(0), _gCore.MapPoints.ElementAt(1))),
                    0,
                    0
                    );

                cmd.Execute(obj);
            }
        }
    }

    public class MoveGameActorCommand : Command
    {
        readonly Vector2 _moveVector;
        readonly string _animation;
        readonly GameCore _gCore;

        public MoveGameActorCommand(GameCore gCore,Vector2 moveDir, string animation)
        {
            _gCore = gCore;
            _moveVector = moveDir;
            _animation = animation;
        }

        public override void Execute(object obj)
        {
            if (obj is GameActor)
            {
                GameActor actor = obj as GameActor;

                actor.MoveVector = _moveVector;
                actor.Animation = _animation;

                //check walkability
                if (_gCore.GameMap.GetCellAtWorldPoint(actor.ActorMobileSprite.Position + _moveVector).Walkable == false)
                    actor.MoveVector = Vector2.Zero;

                //prevent movement into positions which have abrupt changes in height
                if (Math.Abs(_gCore.GameMap.GetOverallHeight(actor.ActorMobileSprite.Position) - _gCore.GameMap.GetOverallHeight(actor.ActorMobileSprite.Position + _moveVector)) > 10)
                    actor.MoveVector = Vector2.Zero;
            }
        }
    }

    public class MoveGameActorToPositionCommand : Command
    {
        readonly GameActor _actor;

        public MoveGameActorToPositionCommand(GameActor actor)
        {
            Debug.Assert(actor != null);
            _actor = actor;
        }

        public override void Execute(object obj)
        {
            if (obj is GameCore)
            {
                GameCore gCore = obj as GameCore;

                //activate mobile sprite
                _actor.ActorMobileSprite.IsActive = true;
                _actor.ActorMobileSprite.Target = _actor.ActorMobileSprite.Position;

                //obtain start and end coordinates
                Point start = gCore.GameMap.WorldToMapCell(_actor.ActorMobileSprite.Position);
                Point end = gCore.GameMap.WorldToMapCell(gCore.GameCamera.ScreenToWorld(gCore.GameInput.MousePosition));

                Debug.WriteLine(string.Format("s:({0},{1})", start.X, start.Y));
                Debug.WriteLine(string.Format("e:({0},{1})", end.X, end.Y));

                //clear current existing path (if navigating in progress)
                _actor.ActorMobileSprite.ClearPathNodes();

                //set up new path finder
                PathFinder p = new PathFinder(gCore.GameMap);

                if (p.Search(start.X, start.Y, end.X, end.Y, gCore.GameMap))
                {
                    _actor.SearchPath = p.PathResult();

                    foreach (PathNode n in _actor.SearchPath)
                    {
                        Vector2 nodevec = gCore.GameCamera.ScreenToWorld(gCore.GameMap.MapCellToScreen(gCore.GameCamera, n.X, n.Y));

                        Debug.WriteLine(string.Format("({0},{1})", nodevec.X, nodevec.Y));
                        //TODO: A probable issue with mapcell -> screen coordinates 
                        _actor.ActorMobileSprite.AddPathNode(nodevec);
                    }
                    _actor.ActorMobileSprite.DeactivateAfterPathing = true;
                }
            }
        }
    }

    //MapEditor: Configuration Command
    public class LoadConfigFileCommand : Command
    {
        public override void Execute(object obj)
        {
            if(obj is GameMapEditor)
            {
                GameMapEditor gmEditor = obj as GameMapEditor;

                gmEditor.GameMap.RegisterConfigurationFile();
            }
        }
    }

    //MapEditor: Get TIleIndex COmmands
    public class UpdateTileIndexCmd : Command
    {
        public override void Execute(object obj)
        {
            if (obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;

                gME.GameMap.OldTileIdx = gME.GameMap.TileIndex;
            }
        }
    }

    public class MoveTileIndexCmd : Command
    {
        readonly TileIndexDir _dir;
        public MoveTileIndexCmd(TileIndexDir dir)
        {
            _dir = dir;
        }

        public override void Execute(object obj)
        {
            if(obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;

                Command cmd = new UpdateTileIndexCmd();
                cmd.Execute(obj);

                switch(_dir)
                {
                    case TileIndexDir.Left:
                        gME.GameMap.GetLeftTileIndex();
                        break;
                    case TileIndexDir.Right:
                        gME.GameMap.GetRightTileIndex();
                        break;
                    case TileIndexDir.Up:
                        gME.GameMap.GetUpTileIndex();
                        break;
                    case TileIndexDir.Down:
                        gME.GameMap.GetDownTileIndex();
                        break;
                }

            }
        }

        public enum TileIndexDir
        {
            Left,
            Right,
            Up,
            Down
        }
    }
    public class IncrementTileTypeIndexCmd : Command
    {
        public override void Execute(object obj)
        {
            if(obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;
                gME.CurrentTileType = (TileType)(++gME.TileTypeIndex % Enum.GetNames(typeof(TileType)).Length);
            }
        }
    }
    public class AddTileToMapCmd : Command
    {
        public override void Execute(object obj)
        {
            if (obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;
                switch (gME.CurrentTileType)
                {
                    case TileType.Base:
                        gME.GameMap.AddBaseTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case TileType.Height:
                        gME.GameMap.AddHeightTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case TileType.Topper:
                        gME.GameMap.AddTopperTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case TileType.Multi:
                        gME.GameMap.AddMultiTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        //gME.GameMap.
                        break;
                }
            }
        }
    }
    public class RemoveTileMapCmd : Command
    {
        public override void Execute(object obj)
        {
            if (obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;
                switch (gME.CurrentTileType)
                {
                    case TileType.Base:
                        gME.GameMap.RemoveBaseTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case TileType.Height:
                        gME.GameMap.RemoveHeightTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case TileType.Topper:
                        gME.GameMap.RemoveTopperTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case TileType.Multi:
                        gME.GameMap.RemoveMultiTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                }
            }
        }
    }
    public class SetPreviewCursor : Command
    {
        readonly bool _value;
        public SetPreviewCursor(bool value)
        {
            _value = value;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;
                gME.ShowPreviewMode = _value;
            }
        }
    }
}
