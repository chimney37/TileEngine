using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    public enum GameEvent
    {
        EVENT_ENTITY_REACHED_WALL,
        EVENT_ENTITY_REACHED_SLOPE,
        EVENT_ENTITY_REACHED_WATER,
        EVENT_ENTITY_ON_LAND
    }

    public abstract class GameObserver
    {
        public abstract void OnNotify(object Entity, GameEvent Event);
    }

    public abstract class GameSubject
    {
        List<GameObserver> Observers = new List<GameObserver>();

        public void AddObserver(GameObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(GameObserver observer)
        {
            Observers.Remove(observer);
        }

        protected void Notify(object Entity, GameEvent Event)
        {
            foreach(GameObserver observer in Observers)
            {
                observer.OnNotify(Entity, Event);
            }
        }
    }

    public class GameCoreEventSystem : GameSubject
    {
        public void UpdateEntity(GameCore gCore)
        {
            Point ActorMapPoint = gCore.GameMap.WorldToMapCell(gCore.PlayerActor.Position);

            //Get Player facing location
            IsometricDirections dir = gCore.PlayerActor.HeadDirections;

            MapCell headingCell = gCore.GameMap.GetMapCellAtDirection(dir, ActorMapPoint.X, ActorMapPoint.Y);

            if (headingCell != null)
            {
                foreach (int tileID in headingCell.TopperTiles)
                {
                    string str = gCore.GameMap.GetTileMapLogicalObjName(tileID);
                    if (gCore.GameMap.GetTileMapLogicalObjName(tileID).Contains("WaterTile"))
                        this.Notify(gCore.PlayerActor, GameEvent.EVENT_ENTITY_REACHED_WATER);
                }

                if (headingCell.HeightTiles.Count() == 0)
                    this.Notify(gCore.PlayerActor, GameEvent.EVENT_ENTITY_ON_LAND);
            }
        }
    }

    public class Achievements : GameObserver
    {
        bool entityIsAtWater = true;
        bool entityIsOnLand = true;
        GameCore gCore;

        public Achievements(GameCore gCore)
        {
            this.gCore = gCore;
        }

        public override void OnNotify(object Entity, GameEvent Event)
        {
            switch(Event)
            {
                case GameEvent.EVENT_ENTITY_REACHED_WATER:
                    if(Entity is MobileSprite && entityIsAtWater)
                    {
                        MobileSprite actor = Entity as MobileSprite;

                        Vector2 ScreenPos = gCore.GameCamera.WorldToScreen(actor.Position);
                        ScreenPos.Y -= 50;

                        Command cmd = new MessageBoxCommand(gCore, "ゲームイベント：", "水の中は歩けません。", ScreenPos);

                        gCore.CommandQueue.Enqueue(cmd);
                        entityIsAtWater = false;
                    }
                    break;
                case GameEvent.EVENT_ENTITY_ON_LAND:
                    if (Entity is MobileSprite && entityIsOnLand)
                    {
                        MobileSprite actor = Entity as MobileSprite;

                        Vector2 ScreenPos = gCore.GameCamera.WorldToScreen(actor.Position);
                        ScreenPos.Y -= 50;

                        Command cmd = new MessageBoxCommand(gCore, "ゲームイベント：", "ここは歩けます", ScreenPos);

                        gCore.CommandQueue.Enqueue(cmd);
                        entityIsOnLand = false;
                    }
                    break;
            }
        }
    }
}
