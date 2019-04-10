using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoogalooGame.Imports
{
    /// <summary>
    /// The camera for the game. Also handles any transformations through matrix math.
    /// </summary>
    /// 

    //Class is mostly courtesy of David Amador
    public class Camera2D
    {
        protected float zoom; // Camera Zoom
        public Matrix transform; // Matrix Transform
        public Vector2 position; // Camera Position
        protected float rotation; // Camera Rotation

        //Default constructor
        public Camera2D()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        //------------------------Gets and sets-----------------
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        // Get set position
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //-------------------Methods---------------------
        /// <summary>
        /// Moves the camera by the given vector. (First element being x transformation and the second element being the y transformation)
        /// </summary>
        /// <param name="amount"></param>
        public void moveCamera(Vector2 amount)
        {
            position += amount;
        }

        public void setCameraPosition(Vector2 pos)
        {
            this.position = pos;
        }

        /// <summary>
        /// Returns the transformation Matrix, which should be created based on the current zoom, position, and Viewport
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.DisplayMode.Width * 0.5f, graphicsDevice.DisplayMode.Height * 0.5f, 0)); //ViewportWidth and ViewportHeight were here originally
            return transform;
        }

    }
}
