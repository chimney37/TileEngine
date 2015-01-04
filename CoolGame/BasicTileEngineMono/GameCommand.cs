﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BasicTile
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
        GameProcess gameProcess;

        public StateChangeToCommand(GameProcess gameProcess)
        {
            this.gameProcess = gameProcess;
        }

        public override void Execute(object obj)
        {
            //http://stackoverflow.com/questions/4963160/how-to-determine-if-a-type-implements-an-interface-with-c-sharp-reflection
            if(obj is Context)
            {
                //only if blocking stack is empty
                if (this.gameProcess.IsEmptySubProcessStack())
                {
                    Context context = (Context)obj;
                    context.changeState(typeof(T));
                }
            }
        }
    }
    
    //Message Box Commands
    public class MessageBoxCommand : Command
    {
        protected GameProcess gameProcess;
        protected string Title {get;set;}
        protected string Content {get;set;}
        protected int X {get;set;}
        protected int Y {get;set;}

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
            if(obj is Context)
            {
                GameMessageBox message = (obj as Context).getFactory().GameMessageBox(Content, Title, X, Y);
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
        GameInput input;
        public MessageBoxGetClickOffsetCommand(GameInput input)
        {
            this.input = input;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                if (mbox.DestinationRectangle.Contains(input.MousePosition))
                {
                    mbox.ClickOffset = new Vector2(input.FirstMouseClickPosition.X, input.FirstMouseClickPosition.Y) - new Vector2(mbox.X, mbox.Y);
                }
            }
        }
    }
    public class MessageBoxMoveCommand : Command
    {
        GameInput input;
        public MessageBoxMoveCommand(GameInput input)
        {
            this.input = input;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMessageBox)
            {
                GameMessageBox mbox = (GameMessageBox)obj;
                if (mbox.DestinationRectangle.Contains(input.MousePosition))
                {
                    mbox.X = input.MousePosition.X - (int)mbox.ClickOffset.X;
                    mbox.Y = input.MousePosition.Y - (int)mbox.ClickOffset.Y;

                    mbox.DestinationRectangle.X = mbox.X;
                    mbox.DestinationRectangle.Y = mbox.Y;
                }
            }
        }
    }

    //Marking Commands
    public class MarkWorldPointCommand : Command
    {
        GameInput input;

        public MarkWorldPointCommand(GameInput input)
        {
            Debug.Assert(input != null);

            this.input = input;
        }

        public override void Execute(object obj)
        {
            if(obj is GameCore)
            {
                GameCore gCore = obj as GameCore;

                Vector2 hilightLoc = gCore.GameCamera.ScreenToWorld(new Vector2(input.MousePosition.X, input.MousePosition.Y));
                //get map cell coordinates of mouse point in Update

                gCore.MapPoints.Dequeue();
                gCore.MapPoints.Enqueue(gCore.GameMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y)));
            }
        }
    }

    //Distance Finder Command
    public class CalculateDistanceDebuggerCommand : Command
    {
        GameCore gCore;
        public CalculateDistanceDebuggerCommand(GameCore gCore)
        {
            this.gCore = gCore;
        }

        public override void Execute(object obj)
        {
            if(obj is Context)
            {
                MessageBoxCommand cmd = new MessageBoxCommand(
                    gCore,
                    "L0 Tile Distance:",
                    string.Format("({0},{1}) -> ({2},{3}) Distance={4}",
                    gCore.MapPoints.ElementAt(0).X,
                    gCore.MapPoints.ElementAt(0).Y,
                    gCore.MapPoints.ElementAt(1).X,
                    gCore.MapPoints.ElementAt(1).Y,
                    TileMap.L0TileDistance(gCore.MapPoints.ElementAt(0), gCore.MapPoints.ElementAt(1))),
                    0,
                    0
                    );

                cmd.Execute(obj);
            }
        }
    }

    //Actor Control Command
    public class MoveActorCommand : Command
    {
        MobileSprite mobileSprite;
        Vector2 moveDir;
        string animation;

        public MoveActorCommand(MobileSprite mobileSprite, Vector2 moveDir, string animation)
        {
            Debug.Assert(mobileSprite != null);

            this.mobileSprite = mobileSprite;
            this.moveDir = moveDir;
            this.animation = animation;
        }

        public override void Execute(object obj)
        {
            if (obj is GameCore)
            {
                GameCore gCore = obj as GameCore;

                //set animation
                gCore.Animation = this.animation;
                gCore.MoveVector = this.moveDir;

                //check walkability
                if (gCore.GameMap.GetCellAtWorldPoint(mobileSprite.Position + moveDir).Walkable == false)
                    gCore.MoveVector = Vector2.Zero;

                //prevent movement into positions which have abrupt changes in height
                if (Math.Abs(gCore.GameMap.GetOverallHeight(mobileSprite.Position) - gCore.GameMap.GetOverallHeight(mobileSprite.Position + moveDir)) > 10)
                    gCore.MoveVector = Vector2.Zero;
            }
        }
    }

    public class MoveActorToPositionCommand : Command
    {
        MobileSprite mobileSprite;
 
        public MoveActorToPositionCommand(MobileSprite mobileSprite)
        {
            Debug.Assert(mobileSprite != null);

            this.mobileSprite = mobileSprite;
        }

        public override void Execute(object obj)
        {
            if (obj is GameCore)
            {
                GameCore gCore = obj as GameCore;

                //activate mobile sprite
                mobileSprite.IsActive = true;

                mobileSprite.Target = mobileSprite.Position;

                //obtain start and end coordinates
                Point start = gCore.GameMap.WorldToMapCell(mobileSprite.Position);
                Point end = gCore.GameMap.WorldToMapCell(gCore.GameCamera.ScreenToWorld(gCore.gameInput.MousePosition));

                Debug.WriteLine(string.Format("s:({0},{1})", start.X, start.Y));
                Debug.WriteLine(string.Format("e:({0},{1})", end.X, end.Y));

                //clear current existing path (if navigating in progress)
                mobileSprite.ClearPathNodes();

                //set up new path finder
                PathFinder p = new PathFinder(gCore.GameMap);

                if (p.Search(start.X, start.Y, end.X, end.Y, gCore.GameMap))
                {
                    gCore.SearchPath = p.PathResult();

                    foreach (PathNode n in gCore.SearchPath)
                    {
                        Vector2 nodevec = gCore.GameCamera.ScreenToWorld(gCore.GameMap.MapCellToScreen(gCore.GameCamera, n.X, n.Y));

                        Debug.WriteLine(string.Format("({0},{1})", nodevec.X, nodevec.Y));
                        //TODO: A probable issue with mapcell -> screen coordinates 
                        mobileSprite.AddPathNode(nodevec);
                    }
                    mobileSprite.DeactivateAfterPathing = true;
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
        TileIndexDir dir;
        public MoveTileIndexCmd(TileIndexDir dir)
        {
            this.dir = dir;
        }

        public override void Execute(object obj)
        {
            if(obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;

                Command cmd = new UpdateTileIndexCmd();
                cmd.Execute(obj);

                switch(dir)
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

                gME.CurrentTileType = (GameMapEditor.TileType)(++gME.TileTypeIndex % Enum.GetNames(typeof(GameMapEditor.TileType)).Length);
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
                    case GameMapEditor.TileType.Base:
                        gME.GameMap.AddBaseTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case GameMapEditor.TileType.Height:
                        gME.GameMap.AddHeightTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case GameMapEditor.TileType.Topper:
                        gME.GameMap.AddTopperTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    case GameMapEditor.TileType.Multi:
                        gME.GameMap.AddMultiTile(gME.HiLightPoint.X, gME.HiLightPoint.Y, gME.GameMap.TileIndex);
                        break;
                    default:
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
                    case GameMapEditor.TileType.Base:
                        gME.GameMap.RemoveBaseTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case GameMapEditor.TileType.Height:
                        gME.GameMap.RemoveHeightTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case GameMapEditor.TileType.Topper:
                        gME.GameMap.RemoveTopperTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    case GameMapEditor.TileType.Multi:
                        gME.GameMap.RemoveMultiTile(gME.HiLightPoint.X, gME.HiLightPoint.Y);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class SetPreviewCursor : Command
    {
        bool value;
        public SetPreviewCursor(bool value)
        {
            this.value = value;
        }

        public override void Execute(object obj)
        {
            if (obj is GameMapEditor)
            {
                GameMapEditor gME = obj as GameMapEditor;

                gME.ShowPreviewMode = value;
            }
        }
    }

}
