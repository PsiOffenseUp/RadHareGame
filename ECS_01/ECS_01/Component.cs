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
            sprite.Image = GameObject.game.Content.Load<Texture2D>("MissingTexture");
            
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
    #region Physics/Collision
    //------------------------Physics Stuff-----------------------------
    /// <summary>
    /// Hitbox for whatever this GameObject is. Used for checking collision. Please note that the vertices should be stored in order of connectivity.
    /// So 0 should be connected to 1, 1 to 2, 2 to 3, but not 3 to 5 or 2 to 6. The last element will be connected to the 0th element
    /// </summary>
    public class Hitbox : Component
    {
        public enum Type { RECTANGLE, TRIANGLE };
        public readonly Vector2[] displacement; //Array of the vertices displacement from its parent's current position. These will be what is set at construction time, and then will be used to update the vertices.
        public Vector2[] vertices { get; private set; } //Array of the vertices for this hitbox. This will be represented as as it's actual coordinates, and should be updated every frame

        //DEBUG May want to store edges and their normals (Both as Vector2) in order to minimize computation in the collision checking

        //Methods
        public override void Update(GameTime gameTime)
        {
            //Update the hitbox coordinates as necessary
            Vector2 parentPos = gameObject.transform.GetPosition(); //Get the Vector2 for the parent's position, so we don't have to keep doing a call for it
            for (int i = 0; i < this.displacement.Length; i++)
                this.vertices[i] = parentPos + this.displacement[i];

            base.Update(gameTime);
        }

        //Constructors
        public Hitbox() : base() { displacement = new Vector2[4]; displacement[0] = new Vector2(0, 0); displacement[1] = new Vector2(1, 0); displacement[2] = new Vector2(0, 1); displacement[3] = new Vector2(1, 1); vertices = new Vector2[4]; }
        public Hitbox(Vector2[] displacement, GameObject gameObject = null) : base() { this.displacement = displacement; this.vertices = new Vector2[displacement.Length]; this.gameObject = gameObject; }
    }

    /// <summary>
    /// Component class to handle an object having physics. This includes having gravity and friction, but not collision. Giving
    /// an object collision can be done with the CollisionComponent class.
    /// </summary>
    public class PhysicsComponent : Component
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
        public PhysicsComponent(float gravity = 1.0f, float airFriction = 1.0f, float groundFriction = 1.0f) : base() { this.gravity = gravity; this.airFriction = airFriction; this.groundFriction = groundFriction; velocity = new Vector2(0.0f, 0.0f); }

    }

    /// <summary>
    /// Component class that enables and handles collisions for a GameObject. Can use to check for collision between objects. Marking the class
    /// as solid will make it so that objects cannot pass through one another when they collide.
    /// </summary>
    public class CollisionComponent : Component
    {
        //-----------------------------------------Member variables--------------------------------------
        bool isSolid;
        PhysicsComponent physics; //Reference to a PhysicsManager component for the game object
        Hitbox hitbox;

        public List<GameObject> collidingObjects{ get; private set; } //List of all objects currently with this component's parent colliding on this frame

        //----------------------------------------------Methods------------------------------------------
        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            collidingObjects.Clear(); //Delete all of the elements in the collidingPairs list. DEBUG May want to optimize this, and instead only remove objects that are no longer relevant, to avoid causing garbage collection often.
            updateCollisions();
            handleCollisions();
            base.Update(gameTime);
        }

        //***********************Collision checks************************

        //Checks for collision between hitbox1 and hitbox2, and returns the result. This will use the Separating Axis Theorem (SAT) to check for collision. Basically, we will take the normal to each edge
        //(Pair of vertices), and then project all the vertices of both shapes onto each normal. We can then check if there is overlap. If a single projection does not overlap, we know that the hitboxes
        //are not colliding.
        static bool checkCollision(Hitbox hitbox1, Hitbox hitbox2)
        {
            //DEBUG Finish implementing SAT
            float minProjection1, minProjection2, maxProjection1, maxProjection2, tempScalar; //Used for checking overlap. Taking projection involves taking dot product between vectors, which will return a scalar. These will hold the 4 relevant scalars.
            Vector2 normalVector; //Vector normal to the edge currently being checked
            Vector2 point2;

            //Go through each hitbox
            for (int i = 0; i < hitbox1.vertices.Length; i++) //Project onto all of the sides in the first polygon
            {
                //Find the normal vector to the current edge being checked. All vectors will be relative to the current vertex being checked (Basically, it's like the origin)
                if (i == hitbox1.vertices.Length - 1) //If we're at the end of the vertices array, loop back around for the last pair
                    point2 = hitbox1.vertices[0];
                else
                    point2 = hitbox1.vertices[i + 1];

                normalVector = takeNormal(hitbox1.vertices[i], point2);

                //Now, project all of the points onto the normal
                minProjection1 = minProjection2 = 0xFFFFFFFF; //Set min to big number, max to small number, since (as far as I know), we can't operate on a spliced list, so optimization isn't really an option.
                maxProjection1 = maxProjection2 = -0xFFFFFFFF;

                foreach (Vector2 vertex in hitbox1.vertices) //Project for hitbox1/shape1
                {
                    tempScalar = Vector2.Dot(vertex - hitbox1.vertices[i], normalVector); //Taking dot product between vertex and normalVector should return projection length from origin (hitbox.vertices[i], for this)
                    if (tempScalar < minProjection1) //If the scalar computed is the smallest one yet, set it to be the min of the projections for hitbox1
                        minProjection1 = tempScalar;
                    if (tempScalar > maxProjection1) //If the scalar computed is the greatest one yet, set it to be the max of the projections for hitbox1
                        maxProjection1 = tempScalar;
                }

                foreach (Vector2 vertex in hitbox2.vertices) //Project for hitbox2/shape2
                {
                    tempScalar = Vector2.Dot(vertex - hitbox1.vertices[i], normalVector); //Taking dot product between vertex and normalVector should return projection length from origin (hitbox.vertices[i], for this)
                    if (tempScalar < minProjection2) //If the scalar computed is the smallest one yet, set it to be the min of the projections for hitbox2
                        minProjection2 = tempScalar;
                    if (tempScalar > maxProjection2) //If the scalar computed is the greatest one yet, set it to be the max of the projections for hitbox2
                        maxProjection2 = tempScalar;
                }

                //Finally, let's make sure the projections overlap. If they don't, we can bail out of the whole function, because these hitboxes don't collide. 
                //Using negation of logic to check for collision (using DeMorgan's law) to find when it should be false
                if ((minProjection1 >= maxProjection2 || minProjection2 >= maxProjection1) && (minProjection2 >= maxProjection1 || minProjection1 >= maxProjection2))
                    return false;

            }

            for (int i = 0; i < hitbox2.vertices.Length; i++) //Project onto all of the sides in the second polygon. Copy and pasted from above code
            {
                //Find the normal vector to the current edge being checked. All vectors will be relative to the current vertex being checked (Basically, it's like the origin)
                if (i == hitbox2.vertices.Length - 1) //If we're at the end of the vertices array, loop back around for the last pair
                    point2 = hitbox2.vertices[0];
                else
                    point2 = hitbox2.vertices[i + 1];

                normalVector = takeNormal(hitbox2.vertices[i], point2);

                //Now, project all of the points onto the normal
                minProjection1 = minProjection2 = 0xFFFFFFFF; //Set min to big number, max to small number, since (as far as I know), we can't operate on a spliced list, so optimization isn't really an option.
                maxProjection1 = maxProjection2 = -0xFFFFFFFF;

                foreach (Vector2 vertex in hitbox1.vertices) //Project for hitbox1/shape1
                {
                    tempScalar = Vector2.Dot(vertex - hitbox2.vertices[i], normalVector); //Taking dot product between vertex and normalVector should return projection length from origin (hitbox.vertices[i], for this)
                    if (tempScalar < minProjection1) //If the scalar computed is the smallest one yet, set it to be the min of the projections for hitbox1
                        minProjection1 = tempScalar;
                    if (tempScalar > maxProjection1) //If the scalar computed is the greatest one yet, set it to be the max of the projections for hitbox1
                        maxProjection1 = tempScalar;
                }

                foreach (Vector2 vertex in hitbox2.vertices) //Project for hitbox2/shape2
                {
                    tempScalar = Vector2.Dot(vertex - hitbox2.vertices[i], normalVector); //Taking dot product between vertex and normalVector should return projection length from origin (hitbox.vertices[i], for this)
                    if (tempScalar < minProjection2) //If the scalar computed is the smallest one yet, set it to be the min of the projections for hitbox2
                        minProjection2 = tempScalar;
                    if (tempScalar > maxProjection2) //If the scalar computed is the greatest one yet, set it to be the max of the projections for hitbox2
                        maxProjection2 = tempScalar;
                }

                //Finally, let's make sure the projections overlap. If they don't, we can bail out of the whole function, because these hitboxes don't collide. 
                //Using negation of logic to check for collision (using DeMorgan's law) to find when it should be false
                if ((minProjection1 >= maxProjection2 || minProjection2 >= maxProjection1) && (minProjection2 >= maxProjection1 || minProjection1 >= maxProjection2))
                    return false;
            }

            return true;
        }

        //The actual main collision checking function. This should be called in the Update() method every frame. Updates the collidingObjects List to contain all of the obvjects that collide with the one that has this component.
        void updateCollisions()
        {
            List<GameObject> objects = GameObject.game.gameObjects;

            for (int i = 0; i < objects.Count; i++) //Go through the remaining GameObjects after this one, so that we can compare each pair of GameObjects in this region
            {
                try
                {
                    if ((objects[i] != this.gameObject) && checkCollision(this.gameObject.GetComponent<Hitbox>(), objects[i].GetComponent<Hitbox>())) //If these two objects are colliding. Also make sure we don't check for collision with the object itself
                        collidingObjects.Add(objects[i]); //Then we will add it as a pair of colliding objects to the collidingPairs List
                }
                        
                catch (NullReferenceException) //Likely will get here if there object does not have a hitbox
                {
                    Console.WriteLine("Object encountered without a hitbox.");
                }
            }

        }

        //Things that all game objects that have collision should do. For instance, solid objects should not pass through each other
        void handleCollisions()
        {
            foreach (GameObject collision in collidingObjects)
            {
                //DEBUg, let's just make sure it picks up on collisions
                Console.WriteLine(collision.ToString());
            }
        }

        //Finds the normal vector between the given two points. The vector is relative to point1 as a starting point.
        private static Vector2 takeNormal(Vector2 point1, Vector2 point2)
        {
            return new Vector2(point1.Y - point2.Y, point2.X - point1.X);
        }

        //---------------------------------------------------Constructors-----------------------------------------------------
        public CollisionComponent() : base() { this.isSolid = false; collidingObjects = new List<GameObject>(); }
        public CollisionComponent (PhysicsComponent physics, Hitbox hitbox) : base() { this.physics = physics; this.isSolid = true; this.hitbox = hitbox; collidingObjects = new List<GameObject>(); }
        public CollisionComponent(GameObject parent, bool isSolid = false) : base() { this.physics = parent.GetComponent<PhysicsComponent>(); this.hitbox = parent.GetComponent<Hitbox>(); this.isSolid = isSolid; gameObject = parent; collidingObjects = new List<GameObject>(); } //This constructor should get relevant component references from its parent

    }
    #endregion

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
