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

        public virtual void Start() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void End() { }
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
            if (keys.IsKeyDown(Keys.Right) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(new Vector2(0.1f, 0.0f));
                Console.WriteLine(gameObject.GetComponent<Camera>().Zoom);
            }
            if (keys.IsKeyDown(Keys.Left) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(new Vector2(-0.1f, 0.0f));
                Console.WriteLine(gameObject.GetComponent<Camera>().Zoom);
            }
            if (keys.IsKeyDown(Keys.Down) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(new Vector2(0.0f, -0.1f));
                Console.WriteLine(gameObject.GetComponent<Camera>().Zoom);
            }
            if (keys.IsKeyDown(Keys.Up) && gameObject.GetComponent<Camera>().Exists())
            {
                gameObject.GetComponent<Camera>().ChangeZoom(new Vector2(0.0f, 0.1f));
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

        public Vector2 Zoom;
        public bool SmoothFollow;
        public enum RelativeTo { WORLD, GAMEOBJECT };
        public RelativeTo relativeTo;

        public override void Start()
        {
            Zoom = new Vector2(1.0f, 1.0f);
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

        public void ChangeZoom(Vector2 deltaZoom)
        {
            if (Zoom.X + deltaZoom.X < 0.1f) Zoom.X = 0.1f;
            else if (Zoom.X + deltaZoom.X > 10.0f) Zoom.X = 10.0f;
            if (Zoom.Y + deltaZoom.Y < 0.1f) Zoom.Y = 0.1f;
            else if (Zoom.Y + deltaZoom.Y > 10.0f) Zoom.Y = 10.0f;
            else Zoom += deltaZoom;
        }
    }
    //------------------------Physics Stuff-----------------------------
    /// <summary>
    /// Component class to handle an object having physics. This includes having gravity and friction, but not collision. Giving
    /// an object collision can be done with the CollisionComponent class.
    /// </summary>
    public class PhysicsManager : Component
    {
        //Member variables
        private float gravity; //How fast the object will fall. Basically, the number of pixels it will fall per frame.
        public float Gravity { get { return this.gravity; } set { this.gravity = value; } }

        //Methods
        public override void Update(GameTime gameTime)
        {
            //Adjust the object this component is attached to's position
            Vector2 newPos = this.gameObject.transform.GetPosition() + new Vector2(0.0f, gravity);
            this.gameObject.transform.SetPosition(newPos);
            base.Update(gameTime);
        }

        //Constructors
        public PhysicsManager() { this.gravity = 1.0f; }
    }

    /// <summary>
    /// Component class that enables and handles collisions for a GameObject. Can use to check for collision between objects. Marking the class
    /// as solid will make it so that objects cannot pass through one another when they collide.
    /// </summary>
    public class CollisionComponent : Component
    {
        //Member variables
        bool isSolid;
        PhysicsManager physics; //Reference to a PhysicsManager component for the game object

        //Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //Constructors
        public CollisionComponent() { this.isSolid = false;}
        public CollisionComponent(PhysicsManager physics) { this.physics = physics; this.isSolid = true; }

    }

    //---------------------------UI STUFF--------------------------------
    public class MouseTracker : Component
    {
        public Vector2 Size { private set; get; }
        public bool Hovering, Clicked;
        

        public override void Start()
        {
            Size = new Vector2(0, 0);
            Hovering = Clicked = false;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            SetState();
            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }

        public void SetSize(Vector2 v)
        {
            Size = v;
        }

        public void SetState()
        {
            MouseState mState = Mouse.GetState();
            if (mState.Position.X >= gameObject.transform.GetPosition().X && mState.Position.X <= gameObject.transform.GetPosition().X + Size.X && mState.Position.Y >= gameObject.transform.GetPosition().Y && mState.Position.Y <= gameObject.transform.GetPosition().Y + Size.Y)
            {
                Hovering = true;
                if (mState.LeftButton == ButtonState.Pressed && !Clicked) Clicked = true;
                if (mState.LeftButton == ButtonState.Released) Clicked = false;
            }
            else Hovering = false;
        }
    }

    public delegate void ButtonClicked();

    public class ButtonController : Component
    {
        Texture2D[] Sprites; //Holds the Pressed/Unpressed Button Images
        MouseTracker mTracker; //For checking for mouse position and clicks
        SpriteRenderer sRenderer; //For setting the button sprite.
        public event ButtonClicked buttonClicked;

        bool ContinuousAction; //specifies if the button action is continuously called, or just called once.
        bool ActionPerformed;

        public override void Start()
        {
            ContinuousAction = false; ActionPerformed = false;
            mTracker = (gameObject.GetComponent<MouseTracker>().Exists()) ? gameObject.GetComponent<MouseTracker>() : null;
            sRenderer = (gameObject.GetComponent<SpriteRenderer>().Exists()) ? gameObject.GetComponent<SpriteRenderer>() : null;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (mTracker.Clicked)
            {
                sRenderer.sprite.Image = Sprites[1];
                if(!ContinuousAction)
                {
                    if(!ActionPerformed)
                    {
                        buttonClicked?.Invoke(); //call the buttons function
                        ActionPerformed = true;
                    }
                }
                else
                {
                    buttonClicked?.Invoke(); //call the buttons function
                }
            }
            else sRenderer.sprite.Image = Sprites[0];
        }

        public override void End()
        {
            base.End();
        }

        public void SetSprites(Texture2D[] sprites)
        {
            Sprites = sprites;
            sRenderer.sprite.Image = Sprites[0];
        }
    }

    public class Clickable : Component
    {
        private Vector2 Size;
        private bool Clicked;

        public override void Start()
        {
            Size = new Vector2(200, 100);
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            SetState();
            base.Update(gameTime);
        }
        
        private void SetState()
        {
            MouseState mouseState = Mouse.GetState();
            Clicked = (mouseState.LeftButton == ButtonState.Pressed);
        }

        public bool GetClickState()
        {
            return Clicked;
        }

        public override void End()
        {
            base.End();
        }
    }
}
