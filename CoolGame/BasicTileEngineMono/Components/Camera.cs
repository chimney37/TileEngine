using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTileEngineMono.Components
{
    /// <summary>
    /// Camera is the view of the user application. This can pan
    /// </summary>
    public class Camera
    {
        Vector2 _location;
        public Vector2 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = new Vector2(
                    MathHelper.Clamp(value.X, 0f, WorldWidth - ViewWidth),
                    MathHelper.Clamp(value.Y, 0f, WorldHeight - ViewHeight));
            }
        }

        public int ViewWidth { get; set; }
        public int ViewHeight { get; set; }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }
        private Rectangle Bounds { get; set; }


        float _scale;
        public float Scale 
        { 
            get
            {
                return _scale;
            }
            set
            {
                _scale = MathHelper.Clamp(value, 1.0f, 2.0f);
            } 
        }

        private Matrix _transform;

        public Camera(Viewport viewport, Vector2 baseViewOffset, Rectangle WorldBounds, float Scale)
        {
            Bounds = viewport.Bounds;
            ViewWidth = Bounds.Width;
            ViewHeight = Bounds.Height;
            DisplayOffset = baseViewOffset;
            WorldWidth = WorldBounds.Width;
            WorldHeight = WorldBounds.Height;
            this.Scale = Scale;
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
        public Vector2 ScreenToWorld(Point screenPosition)
        {
            return ScreenToWorld(new Vector2(screenPosition.X, screenPosition.Y));
        }

        public void Move(Vector2 offset)
        {
            Location += offset;
        }

        public Matrix GetTransformation()
        {
            _transform =
                Matrix.CreateTranslation(new Vector3(-Location.X + DisplayOffset.X, -Location.Y + DisplayOffset.Y, 0)) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 1)
                );

            return _transform;
        }
    }
}
