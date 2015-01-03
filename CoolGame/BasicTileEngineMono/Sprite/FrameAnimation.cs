using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BasicTile
{
    /// <summary>
    /// Controls framed animation at a low level
    ///    Control individual frames of an animation and their source rectangle
    ///    Keep track of current frame
    ///    Control the length in seconds to display each frame
    ///    Keep count of how many times animation has played
    ///    Keep a reference to the next animation that should be played after default animation
    /// 
    /// Reference:
    /// http://www.xnaresources.com/default.asp?page=Tutorial:SpriteEngine:1
    /// http://www.xnaresources.com/default.asp?page=Tutorial:SpriteEngine:2
    /// </summary>
    public class FrameAnimation : ICloneable
    {
        #region MEMBERS, PROPERTIES
        // The first frame of the Animation.  We will calculate other frames on the fly based on this frame.
        private Rectangle rectInitialFrame;
        // Number of frames in the Animation
        private int iFrameCount = 1;
        // The frame currently being displayed. This value ranges from 0 to iFrameCount-1
        private int iCurrentFrame = 0;
        // Amount of time (in seconds) to display each frame
        private float fFrameLength = 0.2f;
        // Amount of time that has passed since we last animated
        private float fFrameTimer = 0.0f;
        // The number of times this animation has been played
        private int iPlayCount = 0;
        // The animation that should be played after this animation
        private string sNextAnimation = null;

        /// The number of frames the animation contains
        public int FrameCount
        {
            get { return iFrameCount; }
            set { iFrameCount = value; }
        }
        /// The time (in seconds) to display each frame
        public float FrameLength
        {
            get { return fFrameLength; }
            set { fFrameLength = value; }
        }
        // The frame number currently being displayed
        public int CurrentFrame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount - 1); }
        }
        public int FrameWidth
        {
            get { return rectInitialFrame.Width; }
        }
        public int FrameHeight
        {
            get { return rectInitialFrame.Height; }
        }

        // The rectangle associated with the current animation frame.
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(
                    rectInitialFrame.X + (rectInitialFrame.Width * iCurrentFrame),
                    rectInitialFrame.Y, rectInitialFrame.Width, rectInitialFrame.Height);
            }
        }
        // Count the number of times animation played
        public int PlayCount
        {
            get { return iPlayCount; }
            set { iPlayCount = value; }
        }
        public string NextAnimation
        {
            get { return sNextAnimation; }
            set { sNextAnimation = value; }
        }
        #endregion

        #region CONSTRUCTORS


        // Default: uses default frame speed (0.2f per frame)
        public FrameAnimation(Rectangle FirstFrame, int Frames)
        {
            rectInitialFrame = FirstFrame;
            iFrameCount = Frames;
        }
        // Default + using rectangular dimensions
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
        }
        // Frame Length + rectangular dimensions 
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
        }
        // Frame Length + rectangular dimensions + Next Animation
        public FrameAnimation(int X, int Y,int Width, int Height, int Frames, float FrameLength, string strNextAnimation)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            sNextAnimation = strNextAnimation;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            fFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //when accumulated time exceeds FrameLength, take action
            if (fFrameTimer > fFrameLength)
            {
                fFrameTimer = 0.0f;

                //increment frame and if over total framecount, reset to 0
                iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;
                if (iCurrentFrame == 0)
                    iPlayCount = (int)MathHelper.Min(iPlayCount + 1, int.MaxValue);
            }
        }
        object ICloneable.Clone()
        {
            return new FrameAnimation(this.rectInitialFrame.X, this.rectInitialFrame.Y,
                                      this.rectInitialFrame.Width, this.rectInitialFrame.Height,
                                      this.iFrameCount, this.fFrameLength, sNextAnimation);
        }
    }
}
