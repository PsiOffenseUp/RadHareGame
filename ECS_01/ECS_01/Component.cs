using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ECS_01
{
    public class Component
    {
        bool Enabled;
        public GameObject gameObject; //reference to container gameobject

        public Component()
        {
            Enabled = true;
        }

        //Public Member Functions
        public bool IsEnabled()
        {
            return Enabled;
        }

        public void SetActive(bool enabled)
        {
            Enabled = enabled;
        }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void End()
        {

        }
    }

    public class SpriteRenderer : Component
    {
        public SpriteRenderer()
        {
            
        }

        public struct Sprite
        {
            public Texture2D Image;
            public Vector2 Scale;
            public float Rotation;
            public int layerDepth;
        }

        public Sprite sprite;

        public override void Start()
        {
            sprite.Image = null;
            sprite.Scale = Vector2.One;
            sprite.Rotation = 0.0f;
            sprite.layerDepth = 0;

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }

        public Vector2 GetSpriteCenter()
        {
            return new Vector2(sprite.Image.Width / 2.0f, sprite.Image.Height / 2.0f);
        }
    }

    public class InputController : Component
    {

        public struct KeyBind //beta feature, not currently in use
        {
            public string ActionName;
            public Keys key;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {

            KeyboardState keys = Keyboard.GetState();
            Vector2 move = new Vector2(0, 0);

            if(keys.IsKeyDown(Keys.W))
            {
                move.Y += -1.0f;
            }
            if (keys.IsKeyDown(Keys.A))
            {
                move.X += -1.0f;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                move.Y += 1.0f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                move.X += 1.0f;
            }
            if (keys.IsKeyDown(Keys.Q))
            {
                gameObject.transform.Rotate(-1.0f, AngleType.DEGREES);
            }
            if (keys.IsKeyDown(Keys.E))
            {
                gameObject.transform.Rotate(1.0f, AngleType.DEGREES);
            }
            if (keys.IsKeyDown(Keys.Z))
            {
                gameObject.transform.Scale(new Vector2(1.01f, 1.01f));
            }
            if (keys.IsKeyDown(Keys.C))
            {
                gameObject.transform.Scale(new Vector2(0.99f, 0.99f));
            }
            if (keys.IsKeyDown(Keys.Up) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(0.1f);
                Console.WriteLine(gameObject.GetComponent<Camera>().Zoom);
            }
            if (keys.IsKeyDown(Keys.Down) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(-0.1f);
                Console.WriteLine(gameObject.GetComponent<Camera>().Zoom);
            }
            if (keys.IsKeyDown(Keys.X) && gameObject.GetComponent<Camera>().Exists()) //still need to implement a KeyPressed function to prevent excessive cycling.
            {
                gameObject.GetComponent<Camera>().relativeTo = (gameObject.GetComponent<Camera>().relativeTo == Camera.RelativeTo.WORLD) ? Camera.RelativeTo.GAMEOBJECT : Camera.RelativeTo.WORLD;
            }

            gameObject.transform.Translate(move, RelativeTo.OBJECT);

            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }
    }

    /// <summary>
    /// Camera component. Attach to a GameObject to have it followed by the viewport.
    /// </summary>
    public class Camera : Component
    {

        public float Zoom;
        public bool SmoothFollow;
        public enum RelativeTo { WORLD, GAMEOBJECT };
        public RelativeTo relativeTo;

        public override void Start()
        {
            Zoom = 1.0f;
            SmoothFollow = false;
            relativeTo = RelativeTo.WORLD;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }

        public void ChangeZoom(float deltaZoom)
        {
            if (Zoom + deltaZoom < 0.1f) Zoom = 0.1f;
            else if (Zoom + deltaZoom > 10.0f) Zoom = 10.0f;
            else Zoom += deltaZoom;
        }
    }
}
