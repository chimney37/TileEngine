using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BasicTile
{
    /// <summary>
    /// High Level Sprite Animation class. Delegates lower level manipulations using the FrameAnimation class
    /// THis class is responsible for:
    ///    Tracking position of sprite on screen
    ///    Holdin actual sprite sheet that sprite is drawn from
    ///    Managing all frame animations associated with sprite
    ///    Handle automatic rotation if turned on
    /// References: 
    /// http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:8
    /// http://www.xnaresources.com/default.asp?page=Tutorial:SpriteEngine:3
    /// </summary>
    class SpriteAnimation
    {
        #region PROPERTIES
        // The texture that holds the images for this sprite
        Texture2D t2dTexture;
        // True if animations are being played
        bool bAnimating = true;
        // If set to anything other than Color.White, will colorize
        // the sprite with that color.
        Color colorTint = Color.White;
        // Screen Position of the Sprite
        Vector2 v2Position = new Vector2(0, 0);
        Vector2 v2LastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        Dictionary<string, FrameAnimation> faAnimations = new Dictionary<string, FrameAnimation>();
        // Which FrameAnimation from the dictionary above is playing
        string sCurrentAnimation = null;
        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        bool bRotateByPosition = false;
        // How much the sprite should be rotated by when drawn
        // Value is in Radians, and 0 indicates no rotation.
        float fRotation = 0f;
        // Calcualted center of the sprite
        Vector2 v2Center;
        // Calculated width and height of the sprite
        int iWidth;
        int iHeight;

        // Vector2 representing the World Coordinate position of the sprite's upper left corner pixel.
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2LastPosition = v2Position;
                v2Position = value;

                UpdateRotation();
            }
        }
        /// The X position of the sprite's upper left corner pixel.
        public int X
        {
            get { return (int)v2Position.X; }
            set
            {
                v2LastPosition.X = v2Position.X;
                v2Position.X = value;
                UpdateRotation();
            }
        }
        /// The Y position of the sprite's upper left corner pixel.
        public int Y
        {
            get { return (int)v2Position.Y; }
            set
            {
                v2LastPosition.Y = v2Position.Y;
                v2Position.Y = value;
                UpdateRotation();
            }
        }
        /// Width (in pixels) of the sprite animation frames
        public int Width
        {
            get { return iWidth; }
        }
        /// Height (in pixels) of the sprite animation frames
        public int Height
        {
            get { return iHeight; }
        }
        // If true, the sprite will automatically rotate in the direction of motion whenever the sprite's Position changes.
        public bool AutoRotate
        {
            get { return bRotateByPosition; }
            set { bRotateByPosition = value; }
        }
        /// The degree of rotation (in radians) to be applied to the sprite when drawn.
        public float Rotation
        {
            get { return fRotation; }
            set { fRotation = value; }
        }
        /// Screen coordinates of the bounding box surrounding this sprite
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, iWidth, iHeight); }
        }
        // The texture associated with this sprite.  All FrameAnimations will be relative to this texture.
        public Texture2D Texture
        {
            get { return t2dTexture; }
        }
        // Color value to tint the sprite with when drawing.  Color.White (the default) indicates no tinting.
        public Color Tint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }
        // True if the sprite is (or should be) playing animation frames.  If this value is set
        // to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation in order to be displayed.
        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }
        // The FrameAnimation object of the currently playing animation
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(sCurrentAnimation))
                    return faAnimations[sCurrentAnimation];
                else
                    return null;
            }
        }
        //The string name of the currently playing animaton.  Setting the animation resets the CurrentFrame and PlayCount properties to zero.
        public string CurrentAnimation
        {
            get { return sCurrentAnimation; }
            set
            {
                if (faAnimations.ContainsKey(value))
                {
                    sCurrentAnimation = value;
                    faAnimations[sCurrentAnimation].CurrentFrame = 0;
                    faAnimations[sCurrentAnimation].PlayCount = 0;
                }
            }
        }

        public Vector2 DrawOffset { get; set; }
        public float DrawDepth { get; set; }
        #endregion

        #region CONSTRUCTOR
        public SpriteAnimation(Texture2D Texture)
        {
            t2dTexture = Texture;
            DrawOffset = Vector2.Zero;
            DrawDepth = 0.0f;
        }
        #endregion

        #region METHODS

        //get rotational angle using arctangent on current and last positions
        void UpdateRotation()
        {
            if (bRotateByPosition)
            {
                fRotation = (float)Math.Atan2(v2Position.Y - v2LastPosition.Y, v2Position.X - v2LastPosition.X);
            }
        }
        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            faAnimations.Add(Name, new FrameAnimation(X, Y, Width, Height, Frames, FrameLength));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }
        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, string NextAnimation)
        {
            faAnimations.Add(Name, new FrameAnimation(X, Y, Width, Height, Frames, FrameLength, NextAnimation));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }
        public FrameAnimation GetAnimationByName(string Name)
        {
            if (faAnimations.ContainsKey(Name))
            {
                return faAnimations[Name];
            }
            else
            {
                return null;
            }
        }
        public void MoveBy(int x, int y)
        {
            v2LastPosition = v2Position;
            v2Position.X += x;
            v2Position.Y += y;
            UpdateRotation();
        }
        public void Update(GameTime gameTime)
        {
            // Don't do anything if the sprite is not animating
            if (bAnimating)
            {
                // If there is not a currently active animation
                if (CurrentFrameAnimation == null)
                {
                    // Make sure we have an animation associated with this sprite
                    if (faAnimations.Count > 0)
                    {
                        // Set the active animation to the first animation
                        // associated with this sprite
                        string[] sKeys = new string[faAnimations.Count];
                        faAnimations.Keys.CopyTo(sKeys, 0);
                        CurrentAnimation = sKeys[0];
                    }
                    else
                    {
                        return;
                    }
                }

                // Run the Animation's update method
                CurrentFrameAnimation.Update(gameTime);

                // Check to see if there is a "followup" animation named for this animation
                // why follow up?
                // cannon sprite on the screen. It has an animation loop running where it is billowing smoke, or some such. 
                // Now you want the cannon to fire, so you have a nice animation where, just like in the cartoons, 
                // the cannon scrunches up and BOOM! Elongates and fires a cannon ball. 
                // Now what? Well, lets say you have defined two FrameAnimations, one called "idle" and one called "fire!". 
                // If you set the NextAnimation property of "fire!" to "idle", 
                // when you trigger "fire!" it will play the animation and then go right back to idle 
                // instead of looping the fire animation over and over again. 
                if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    // If there is, see if the currently playing animation has
                    // completed a full animation loop                         
                    // If it has, set up the next animation
                    if (CurrentFrameAnimation.PlayCount > 0)
                        CurrentAnimation = CurrentFrameAnimation.NextAnimation;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if (bAnimating)
                spriteBatch.Draw(
                    t2dTexture,
                    v2Position + v2Center + DrawOffset + new Vector2(XOffset, YOffset),
                    CurrentFrameAnimation.FrameRectangle, 
                    colorTint,
                    fRotation, 
                    v2Center,
                    1.0f, 
                    SpriteEffects.None,
                    DrawDepth);
        }
        #endregion
    }
}
