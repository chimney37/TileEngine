using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
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
    class MobileSprite
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
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (bVisible)
            {
                asSprite.Draw(spriteBatch, 0, 0);
            }
        }
        #endregion
    }
}
