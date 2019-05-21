﻿using System;
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
            if(keys.IsKeyDown(Keys.R) && gameObject.Children.Count > 0)
            {
                gameObject.Children.Clear();
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
    /// Hitbox for whatever this GameObject is. Used for checking collision. Please note that the vertices should be stored in order of connectivity.
    /// So 0 should be connected to 1, 1 to 2, 2 to 3, but not 3 to 5 or 2 to 6. The last element will be connected to the 0th element
    /// </summary>
    public class Hitbox : Component
    {
        public enum Type { RECTANGLE, TRIANGLE };
        public readonly Vector2[] displacement; //Array of the vertices displacement from its parent's current position. These will be what is set at construction time, and then will be used to update the vertices.
        public Vector2[] vertices { get; private set; }; //Array of the vertices for this hitbox. This will be represented as as it's actual coordinates, and should be updated every frame. 

        //DEBUG May want to store edges and their normals (Both as Vector2) in order to minimize computation in the collision checking

        //Methods
        public override void Update(GameTime gameTime)
        {
            //Update the hitbox coordinates as necessary
            Vector2 parentPos = gameObject.transform.GetPosition(); //Get the Vector2 for the parent's position, so we don't have to keep doing a reference to it
            for (int i = 0; i < this.displacement.Length; i++)
                this.vertices[i] = parentPos + this.displacement[i];

            base.Update(gameTime);
        }

        //Constructors
        Hitbox() { displacement = new Vector2[4]; displacement[0] = new Vector2(0, 0); displacement[1] = new Vector2(1, 0); displacement[2] = new Vector2(0, 1); displacement[3] = new Vector2(1, 1); vertices = new Vector2[4]; }
        Hitbox(Vector2[] vertices) { this.displacement = vertices; this.vertices = new Vector2[vertices.Length]; }
    }

    /// <summary>
    /// Component class to handle an object having physics. This includes having gravity and friction, but not collision. Giving
    /// an object collision can be done with the CollisionComponent class.
    /// </summary>
    public class PhysicsManager : Component
    {
        //Member variables
        private float gravity; //How fast the object will fall. Basically, the number of pixels it will fall per frame.
        public float Gravity { get { return this.gravity; } set { this.gravity = value; } }
        private float airFriction, groundFriction;
        private Vector2 velocity;

        //Methods
        public override void Update(GameTime gameTime)
        {
            //Adjust the object this component is attached to's position
            Vector2 newPos = this.gameObject.transform.GetPosition() + new Vector2(0.0f, gravity);
            this.gameObject.transform.SetPosition(newPos);
            base.Update(gameTime);
        }

        #region Setting and getting methods
        void setAirFriction(float airFriction) { this.airFriction = airFriction; }
        void setGroundFriction(float groundFriction) { this.groundFriction = groundFriction; }
        void setVelocity(Vector2 velocity) { this.velocity = velocity; }
        void setVelocity(float Xvelocity, float Yvelocity) { this.velocity = new Vector2(Xvelocity, Yvelocity); }
        float getAirFriction() { return this.airFriction; }
        float getGroundFriction() { return this.groundFriction; }
        Vector2 getVelocity() { return this.velocity; }
        float getHorizontalVelocity() { return this.velocity.X; }
        float getVerticalVelocity() { return this.velocity.Y; }

        bool isGoingUp() { return Math.Sign(this.velocity.Y) == -1; }
        bool isGoingDown() { return Math.Sign(this.velocity.Y) == 1; }
        bool isGoingLeft() { return Math.Sign(this.velocity.X) == -1; }
        bool isGoingRight() { return Math.Sign(this.velocity.X) == 1; }
        #endregion

        //Constructors
        public PhysicsManager() { this.gravity = 1.0f; this.airFriction = 1.0f; this.groundFriction = 1.0f; velocity = new Vector2(0.0f, 0.0f); }

    }

    /// <summary>
    /// Component class that enables and handles collisions for a GameObject. Can use to check for collision between objects. Marking the class
    /// as solid will make it so that objects cannot pass through one another when they collide.
    /// </summary>
    public class CollisionComponent : Component
    {
        //-----------------------------------------Member variables--------------------------------------
        bool isSolid;
        PhysicsManager physics; //Reference to a PhysicsManager component for the game object

        public static List<Tuple<GameObject, GameObject>> collidingPairs { get; private set; } //List of all object pairs currently colliding on this frame

        //----------------------------------------------Methods------------------------------------------
        public override void Start()
        {
            collidingPairs = new List<Tuple<GameObject, GameObject>>();
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            collidingPairs.Clear(); //Delete all of the elements in the collidingPairs list. DEBUG May want to optimize this, and instead only remove pairs that are no longer relevant, to avoid causing garbage collection often.
            updateCollisions();
            base.Update(gameTime);
        }

        //***********************Collision checks************************

        /// <summary>
        /// Goes through the GameObjects currently active, and assigns them to smaller regions. This way, we only have to check collision between objects in the same region. This should significantly
        /// cut down on the number of comparisons required for collision checks. Could optimize with an R-tree?
        /// </summary>
        /// <returns>Returns a list of regions, each containing a list of objects within those regions. </returns>
        static List<List<GameObject>> getCollisionRegions()
        {
            List<List<GameObject>> viableCollisions = new List<List<GameObject>>();
            List<GameObject> currentRegion = new List<GameObject>(); ;

            //DEBUG, just so this still compiles. Will finish writing later.
            viableCollisions.Add(currentRegion);

            return viableCollisions;
        }

        //Checks for collision between hitbox1 and hitbox2, and returns the result. This will use the Separating Axis Theorem (SAT) to check for collision. Basically, we will take the normal to each edge
        //(Pair of vertices), and then project all the vertices of both shapes onto each normal. We can then check if there is overlap. If a single projection does not overlap, we know that the hitboxes
        //are not colliding.
        static bool checkCollision(Hitbox hitbox1, Hitbox hitbox2)
        {
            //DEBUG Finish implementing SAT
            float minProjection1, minProjection2, maxProjection1, maxProjection2; //Used for checking overlap. Taking projection involves taking dot product between vectors, which will return a scalar. These will hold the 4 relevant scalars.

            //Go through each hitbox
            for (int i = 0; i < hitbox1.vertices.Length; i++) //Project onto all of the sides in the first polygon
            {

            }

            for (int i = 0; i < hitbox2.vertices.Length; i++) //Project onto all of the sides in the second polygon
            {

            }

            return true;
        }

        //Finds any objects which collide with collisionObject, and then returns them. If no collisions are found, returns null. Searches through the collidingPairs list for the collisionObject
        static List<GameObject> findCollidingObjects(GameObject collisionObject)
        {
            return null;
        }

        //The actual main collision checking function. This should be called in the Update() method every frame.
        static void updateCollisions()
        {
            List<List<GameObject>> collisionRegions = getCollisionRegions(); //Get the viable collision regions to simplify collision checking

            foreach (List<GameObject> region in collisionRegions) //Go through each region, so that we can check the collision between any objects in that region
            {
                for (int i = 0; i < region.Count - 1; i++) //Go through each GameObject
                {
                    for (int j = i + 1; j < region.Count; j++) //Go through the remaining GameObjects after this one, so that we can compare each pair of GameObjects in this region
                    {
                        if(checkCollision(region[i].GetComponent<Hitbox>(), region[j].GetComponent<Hitbox>())) //If these two objects are colliding
                            collidingPairs.Add(new Tuple<GameObject,GameObject>(region[i], region[j])); //Then we will add it as a pair of colliding objects to the collidingPairs List
                    }
                }
            }
        }

        //Finds the normal vector between the given two points. The vector is relative to point1 as a starting point.
        private static Vector2 takeNormal(Vector2 point1, Vector2 point2)
        {
            return new Vector2(point1.Y - point2.Y, point2.X - point1.X);
        }

        //---------------------------------------------------Constructors-----------------------------------------------------
        public CollisionComponent() { this.isSolid = false;}
        public CollisionComponent(PhysicsManager physics, Hitbox hitbox) { this.physics = physics; this.isSolid = true; this.hitbox = hitbox; }
        public CollisionComponent(GameObject parent) { } //This constructor should get relevant component references from its parent

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
