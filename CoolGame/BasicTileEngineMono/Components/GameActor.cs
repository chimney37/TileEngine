using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    public class GameActor : GameObject
    {
        #region PROPERTIES
        MobileSprite actor;
        public MobileSprite ActorMobileSprite
        {
            get { return actor; }
        }
        public Vector2 MoveVector { get; set; }
        public string Animation { get; set; }
        //for movement via A* 
        public Point ActorMapCellPosition { get; set; }
        List<PathNode> pathNodes = new List<PathNode>();
        public List<PathNode> SearchPath
        {
            get { return pathNodes; }
            set { pathNodes = value; }
        }
        #endregion

        //TODO: add actor states, moving, stationary, etc.

        public GameActor(Texture2D texture)
        {
            actor = new MobileSprite(texture);
        }

        public void Update(GameTime gameTime, TileMap tileMap)
        {

            if (ActorMobileSprite.IsActive)
            {
                string animation = "";
                string endanimation = "";

                IsometricDirections dir = ActorMobileSprite.HeadDirections;

                switch (dir)
                {
                    case IsometricDirections.N:
                        animation = "WalkNorth";
                        endanimation = "IdleNorth";
                        break;
                    case IsometricDirections.NE:
                        animation = "WalkNorthEast";
                        endanimation = "IdleNorthEast";
                        break;
                    case IsometricDirections.E:
                        animation = "WalkEast";
                        endanimation = "IdleEast";
                        break;
                    case IsometricDirections.SE:
                        animation = "WalkSouthEast";
                        endanimation = "IdleSouthEast";
                        break;
                    case IsometricDirections.S:
                        animation = "WalkSouth";
                        endanimation = "IdleSouth";
                        break;
                    case IsometricDirections.SW:
                        animation = "WalkSouthWest";
                        endanimation = "IdleSouthWest";
                        break;
                    case IsometricDirections.W:
                        animation = "WalkWest";
                        endanimation = "IdleWest";
                        break;
                    case IsometricDirections.NW:
                        animation = "WalkNorthWest";
                        endanimation = "IdleNorthWest";
                        break;
                }

                if (ActorMobileSprite.Sprite.CurrentAnimation != animation)
                {
                    ActorMobileSprite.Sprite.CurrentAnimation = animation;
                    ActorMobileSprite.EndPathAnimation = endanimation;
                }

                ActorMobileSprite.Update(gameTime);
            }
            //case for direct sprite control : to not interfere with mobile sprite updates in the same loop (active -> mobile sprite is animating)
            else
            {
                if (MoveVector.Length() != 0)
                {
                    ActorMobileSprite.Sprite.MoveBy((int)MoveVector.X, (int)MoveVector.Y);
                    if (ActorMobileSprite.Sprite.CurrentAnimation != Animation)
                        ActorMobileSprite.Sprite.CurrentAnimation = Animation;
                }
                else
                {
                    ActorMobileSprite.Sprite.CurrentAnimation = "Idle" + ActorMobileSprite.Sprite.CurrentAnimation.Substring(4);
                }

                ActorMobileSprite.Sprite.Update(gameTime);

                //reset move vector since we don't want this actor to move forever
                MoveVector = Vector2.Zero;
            }


            //update the map cell where player is
            ActorMapCellPosition = tileMap.WorldToMapCell(new Point((int)ActorMobileSprite.Position.X, (int)ActorMobileSprite.Position.Y));

        }
    }
}
