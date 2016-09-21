using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BasicTileEngineMono
{
    /// <summary>
    /// MobileSprite class: 
    ///    Provides interface to SpriteAnimation
    ///    Move sprites at defined speed towards targeted point
    ///    Allow "path" of points to be assigned to sprite
    ///    Provide collision information
    /// References:
    /// http://www.xnaresources.com/default.asp?page=Tutorial:SpriteEngine:4
    /// </summary>
    public class MobileSprite
    {
        #region MEMBERS PROPERTIES
        // The SpriteAnimation object that holds the graphical and animation data for this object
        SpriteAnimation asSprite;
        public SpriteAnimation Sprite
        {
            get { return asSprite; }
        }
        public Vector2 Position
        {
            get { return asSprite.Position; }
            set { asSprite.Position = value; }
        }
        public Vector2 Target
        {
            get { return v2Target; }
            set { v2Target = value; }
        }

        // A queue of pathing vectors to allow the sprite to move along a path
        Queue<Vector2> queuePath = new Queue<Vector2>();
        // The location the sprite is currently moving towards
        Vector2 v2Target;
        // The speed at which the sprite will close with it's target
        float fSpeed = 1f;
        public float Speed
        {
            get { return fSpeed; }
            set { fSpeed = value; }
        }
        // These two integers represent a clipping range for determining bounding-box style
        // collisions.  They return the bounding box of the sprite trimmed by a horizonal and verticle offset to get a collision cushion
        int iCollisionBufferX = 0;
        int iCollisionBufferY = 0;

        public int HorizontalCollisionBuffer
        {
            get { return iCollisionBufferX; }
            set { iCollisionBufferX = value; }
        }
        public int VerticalCollisionBuffer
        {
            get { return iCollisionBufferY; }
            set { iCollisionBufferY = value; }
        }
        public Rectangle BoundingBox
        {
            get { return asSprite.BoundingBox; }
        }
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    asSprite.BoundingBox.X + iCollisionBufferX,
                    asSprite.BoundingBox.Y + iCollisionBufferY,
                    asSprite.Width - (2 * iCollisionBufferX),
                    asSprite.Height - (2 * iCollisionBufferY));
            }
        }
        public Vector2 Delta
        {
            get 
            {
                Vector2 delta = new Vector2(v2Target.X - asSprite.X, v2Target.Y - asSprite.Y);
                delta.Normalize();
                return delta; 
            }
        }

        //for smoothing out the angles for a given mobile sprite
        Queue<float> lastframesangles = new Queue<float>();

        //getting the directions on an isometric map
        public IsometricDirections HeadDirections
        {
            get
            {
                //calculate angle between vladMobile heading direction and a vector facing N
                Double thisAngRad = MobileSprite.signedAngle(Delta, new Vector2(0, -1));

                //smoothing function (average over a few frames, so we won't get jaggy animations)
                if (!Double.IsNaN(thisAngRad))
                {
                    if (lastframesangles.Count < 10)
                        lastframesangles.Enqueue((float)thisAngRad);
                    else
                    {
                        lastframesangles.Dequeue();
                        lastframesangles.Enqueue((float)thisAngRad);
                        thisAngRad = lastframesangles.Average();
                    }
                }

                if (Math.PI / 8 > thisAngRad && thisAngRad > -Math.PI / 8)
                    return IsometricDirections.N;
                
                //isometric angles is "flatter" than it looks
                //eaxctly 67.5 deg won't work so make it abit bigger than (3/8) * pi
                if (-3.5 * Math.PI / 8 < thisAngRad && thisAngRad < -Math.PI / 8)
                    return IsometricDirections.NE;
                
                //exactly (5/8) * pi won't work so make it a bit smaller
                if (-4.9 * Math.PI / 8 < thisAngRad && thisAngRad < -3.5 * Math.PI / 8)
                    return IsometricDirections.E;
                
                if (-7.5 * Math.PI / 8 < thisAngRad && thisAngRad < -4.9 * Math.PI / 8)
                    return IsometricDirections.SE;
                
                if (7 * Math.PI / 8 < thisAngRad || thisAngRad < -7.5 * Math.PI / 8)
                    return IsometricDirections.S;
                
                if (7 * Math.PI / 8 > thisAngRad && thisAngRad > 4.9 * Math.PI / 8)
                    return IsometricDirections.SW;
                
                if (4.9 * Math.PI / 8 > thisAngRad && thisAngRad > 3.5 * Math.PI / 8)
                    return IsometricDirections.W;
                
                if (3.5 * Math.PI / 8 > thisAngRad && thisAngRad > Math.PI / 8)
                    return IsometricDirections.NW;

                return IsometricDirections.UNDEFINED;
            }
        }

        #region BOOLEAN CHECKS
        // Determine the status of the sprite.  An inactive sprite will not be updated but will be drawn.
        bool bActive = true;
        // Determines if the sprite should track towards a v2Target.  If set to false, the sprite
        // will not move on it's own towards v2Target, and will not process pathing information
        bool bMovingTowardsTarget = true;
        // Determines if the sprite will follow the path in it's Path queue.  If true, when the sprite
        // has reached v2Target the next path node will be pulled from the queue and set as the new v2Target.
        bool bPathing = true;
        // If true, any pathing node popped from the Queue will be placed back onto the end of the queue
        bool bLoopPath = true;
        // If true, the sprite can collide with other objects.  Note that this is only provided as a flag
        // for testing with outside code.
        bool bCollidable = true;
        // If true, the sprite will be drawn to the screen
        bool bVisible = true;
        // If true, the sprite will be deactivated when the Pathing Queue is empty.
        bool bDeactivateAtEndOfPath = false;
        // If true, bVisible will be set to false when the Pathing Queue is empty.
        bool bHideAtEndOfPath = false;

        //path related boolean properties
        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }
        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        #region UNRERFERENCED
        public bool IsPathing
        {
            get { return bPathing; }
            set { bPathing = value; }
        }
        public bool DeactivateAfterPathing
        {
            get { return bDeactivateAtEndOfPath; }
            set { bDeactivateAtEndOfPath = value; }
        }
        public bool LoopPath
        {
            get { return bLoopPath; }
            set { bLoopPath = value; }
        }
        public bool HideAtEndOfPath
        {
            get { return bHideAtEndOfPath; }
            set { bHideAtEndOfPath = value; }
        }
        public bool IsMoving
        {
            get { return bMovingTowardsTarget; }
            set { bMovingTowardsTarget = value; }
        }
        public bool IsCollidable
        {
            get { return bCollidable; }
            set { bCollidable = value; }
        }
        #endregion
        #endregion

        #region END PATH ANIMATION
        // If set, when the Pathing Queue is empty, the named animation will be set as the
        // current animation on the sprite.
        string sEndPathAnimation = null;
        public string EndPathAnimation
        {
            get { return sEndPathAnimation; }
            set { sEndPathAnimation = value; }
        }
        #endregion

        #endregion

        #region CONSTRUCTOR
        public MobileSprite(Texture2D texture)
        {
            asSprite = new SpriteAnimation(texture);
        }
        #endregion

        #region MEMBERS
        public void AddPathNode(Point node)
        {
            queuePath.Enqueue(new Vector2(node.X, node.Y));
        }
        public void AddPathNode(Vector2 node)
        {
            queuePath.Enqueue(node);
        }
        public void AddPathNode(int X, int Y)
        {
            queuePath.Enqueue(new Vector2(X, Y));
        }
        public void ClearPathNodes()
        {
            queuePath.Clear();
        }
        public void Update(GameTime gameTime)
        {
            if (bActive && bMovingTowardsTarget)
            {
                if (!(v2Target == null))
                {
                    // Get a vector pointing from the current location of the sprite to the destination.
                    Vector2 Delta = new Vector2(v2Target.X - asSprite.X, v2Target.Y - asSprite.Y);

                    //Debug.WriteLine(signedAngle(this.Delta, new Vector2(0,-1)) );

                    //if distance is greater than speed for within an update cannot be reached
                    if (Delta.Length() > Speed)
                    {
                        Delta.Normalize();
                        Delta *= Speed;
                        Position += Delta;
                    }
                    else
                    {
                        //if distance is less than speed, we can be already sitting at that point
                        if (v2Target == asSprite.Position)
                        {
                            //if sprite will follow path in its queue
                            if (bPathing)
                            {
                                //queue is not empty
                                if (queuePath.Count > 0)
                                {
                                    v2Target = queuePath.Dequeue();
                                    
                                    if (bLoopPath)
                                        queuePath.Enqueue(v2Target);
                                }
                                //queue is empty
                                else
                                {
                                    //check if there is an animation for end of path and also that current animation is not the same as the end path animation
                                    //then, set the animation by the end path animation
                                    if (!(sEndPathAnimation == null) && 
                                        !(Sprite.CurrentAnimation == sEndPathAnimation))
                                            Sprite.CurrentAnimation = sEndPathAnimation;

                                    if (bDeactivateAtEndOfPath)
                                        IsActive = false;

                                    if (bHideAtEndOfPath)
                                        IsVisible = false;
                                }
                            }
                        }
                            //the other possibility is we are close and should go to point directly
                        else
                            asSprite.Position = v2Target;
                    }
                }
            }
            if (bActive)
                asSprite.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if (bVisible)
            {
                asSprite.Draw(spriteBatch, XOffset, YOffset);
            }
        }

        public static float signedAngle(Vector2 v1, Vector2 v2)
        {
            float perpDot = v1.X * v2.Y - v1.Y * v2.X;

            return (float)Math.Atan2(perpDot, Vector2.Dot(v1,v2));
        }
        #endregion
    }
}
