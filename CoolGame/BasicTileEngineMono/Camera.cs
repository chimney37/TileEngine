using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BasicTile
{
    /// <summary>
    /// Camera is the view of the user application. This can pan
    /// </summary>
    public class Camera
    {
        Vector2 location;
        public Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = new Vector2(
                    MathHelper.Clamp(value.X, 0f, WorldWidth - ViewWidth),
                    MathHelper.Clamp(value.Y, 0f, WorldHeight - ViewHeight));
            }
        }
        public Vector2 Origin { get; set; }

        public int ViewWidth { get; set; }
        public int ViewHeight { get; set; }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }
        public float Scale { get; set; }

        private Matrix _transform;

        public Camera()
        {
            Origin = Vector2.Zero;
        }


        //for global base offsets
        public Vector2 DisplayOffset { get; set; }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition , GetTransformation());
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition , Matrix.Invert(GetTransformation()) );
        }

        public void Move(Vector2 offset)
        {
            Location += offset;
        }

        public Matrix GetTransformation()
        {
            _transform =
                Matrix.CreateTranslation(new Vector3(-Location.X + DisplayOffset.X, -Location.Y + DisplayOffset.Y, 0)) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 1));

            return _transform;
        }
    }
}
