using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework; //Needed for Rectangle and Vector2 classes
using Microsoft.Xna.Framework.Graphics; //Needed for Texture2D class
using BoogalooGame;

namespace BoogalooGame
{
    public abstract class GameObject
    {
        //--------------Fields--------------
        public Vector2 position; //X and y coordinate of the character
        public Sprite sprite;
        private Rectangle hitbox;
        public Color hitboxColor;

        public int priority; //Aids in how likely an object is to be grabbed
        public float xspeed, yspeed;
        private float weight, fall_speed; //How fast an object will fall
        public long id;

        private bool grounded; //Tells whether or not the object is on the ground
        private bool grabbable; //Can the object be grabbed?
        public bool collision_left, collision_right, collision_above, collision_below; //Checks for collision with floors and walls

        public static long id_count; //How many ids have been created. DEBUG May need to set a cap to avoid future overflow
        private static IDictionary<long, GameObject> object_dict; //Holds all active objects
        public static Regulator controller; //Controls most aspects of the game

        public static Rectangle debugVisualizer = new Rectangle();

        //---------------Constructors------------

        //Static constructor
        static GameObject()
        {
            object_dict = new Dictionary<long, GameObject>();
            id_count = 0;
            controller = new Regulator();
        }

        //Default constructor
        public GameObject()
        {
            position = new Vector2(0.0f, 0.0f);
            sprite = new Sprite(); // Default sprite for all Gameobjects
            
            weight = 0.25f;
            fall_speed = 10.0f;
            grounded = false;
            xspeed = 0.0f;
            yspeed = 0.0f;
            hitbox = new Rectangle(0, 0, 32, 32);
            grabbable = false;
            collision_left = false;
            collision_right = false;
            collision_above = false;
            collision_below = false;

            //Add this item to the dictionary of active items
            this.id = id_count; //Set the id to the current id count
            id_count++; //Increase the amount of things which have been spawned

        }

        public GameObject(float xpos, float ypos)
        {
            position = new Vector2(xpos, ypos);
            sprite = new Sprite(); // Default sprite for all Gameobjects

            weight = 0.25f;
            fall_speed = 10.0f;
            grounded = false;
            xspeed = 0.0f;
            yspeed = 0.0f;
            hitbox = new Rectangle((int)xpos, (int)ypos, 32, 32);
            grabbable = false;
            collision_left = false;
            collision_right = false;
            collision_above = false;
            collision_below = false;

            //Add this item to the dictionary of active items
            this.id = id_count; //Set the id to the current id count
            id_count++; //Increase the amount of things which have been spawned
        }

        public GameObject (string sprite_path)
        {
            position = new Vector2(0.0f, 0.0f);
            sprite = new Sprite();
            sprite.path = sprite_path;

            weight = 0.25f;
            fall_speed = 10.0f;
            grounded = false;
            xspeed = 0.0f;
            yspeed = 0.0f;
            hitbox = new Rectangle(0, 0, sprite.Width, sprite.Height);
            grabbable = false;
            collision_left = false;
            collision_right = false;
            collision_above = false;
            collision_below = false;

            //Add this item to the dictionary of active items
            this.id = id_count; //Set the id to the current id count
            id_count++; //Increase the amount of things which have been spawned
        }

        public GameObject(string sprite_path, float xpos, float ypos)
        {
            position = new Vector2(xpos, ypos);
            sprite = new Sprite();
            sprite.path = sprite_path;

            weight = 0.25f;
            fall_speed = 10.0f;
            grounded = false;
            xspeed = 0.0f;
            yspeed = 0.0f;
            hitbox = new Rectangle((int)ypos, (int)xpos, sprite.Width, sprite.Height);
            grabbable = false;
            collision_left = false;
            collision_right = false;
            collision_above = false;
            collision_below = false;

            //Add this item to the dictionary of active items
            this.id = id_count; //Set the id to the current id count
            id_count++; //Increase the amount of things which have been spawned
        }

        //------------------------------Gets and sets---------------------------
        public bool IsGrounded
        {
            get { return this.grounded; }
            set { this.grounded = value; }
        }

        public float Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }

        public Rectangle Hitbox
        {
            get { return this.hitbox; }
        }

        public static IDictionary<long, GameObject> ActiveObjects
        {
            get { return object_dict; }
        }

        //----------------Function implementation-------------

        //Puts the item into the object dictionary
        public void Load(Game1 game)
        {
            object_dict.Add(new KeyValuePair<long, GameObject>(this.id, this));
            this.sprite.loadSprite(game);
            this.hitbox.Width = sprite.Width;
            this.hitbox.Height = sprite.Height;
        }

        public void Unload()
        {
            object_dict.Remove(this.id);
        }

        public void setPosition(float xPos, float yPos)
        {
            this.position.X = xPos;
            this.position.Y = yPos;

            //Update the hitbox, too
            this.hitbox.X = (int)this.position.X;
            this.hitbox.Y = (int)this.position.Y;
        }

        public bool isMovingLeft()
        {
            if (xspeed < 0.0f)
                return true;
            return false;
        }

        public bool isMovingRight()
        {
            if (xspeed > 0.0f)
                return true;
            return false;
        }

        public bool isMovingUp()
        {
            if (yspeed < 0.0f)
                return true;
            else
                return false;
        }

        public bool isMovingDown()
        {
            if (yspeed > 0.0f)
                return true;
            else
                return false;
        }

        private GameObject doPhysics()
        {
            //Check for any collision
            GameObject collidingObject = null;
            bool wasGrounded = this.grounded;
            bool willCollide = false;

            float oldX = this.position.X;
            float oldY = this.position.Y;
        
            //Apply gravity
            //if (!this.collision_below)
            //{
                this.yspeed += this.weight; //While not on the ground, make the object affected by gravity.
                if (this.yspeed > this.fall_speed)
                    this.yspeed = this.fall_speed;
            //}

            this.position.X = (float)Math.Round(this.position.X + xspeed);
            this.position.Y = (float)Math.Round(this.position.Y + yspeed);

            this.hitbox.X = (int)this.position.X;
            this.hitbox.Y = (int)this.position.Y;

            //Reset all collision before checking
            this.collision_below = false;
            this.collision_above = false;
            this.collision_left = false;
            this.collision_right = false;

            float belowFix = oldY;
            float aboveFix = oldY;
            float leftFix = oldX;
            float rightFix = oldX;

            float objectLeft = this.position.X;
            float objectRight = this.position.X + this.hitbox.Width;
            float objectTop = this.position.Y;
            float objectBottom = this.position.Y + this.hitbox.Height;

            foreach (KeyValuePair<long, GameObject> entry in object_dict)
            {
                if (this.hitbox.Intersects(entry.Value.hitbox))
                {
                    //Possible bug in collision: If character builds up enough speed, they will clip through the collision
                    if (entry.Value is Collision) //If the object collided with is an instance of a collision object, do some checking
                    {

                        //Collision above
                        if (this.position.Y <= entry.Value.position.Y + entry.Value.hitbox.Height && oldY > entry.Value.position.Y)
                        {
                            this.collision_above = true;
                            aboveFix = entry.Value.position.Y + entry.Value.hitbox.Height;
                        }

                        //Collision to the right
                        if (objectRight > entry.Value.position.X && objectLeft < entry.Value.position.Y + entry.Value.hitbox.Width - this.hitbox.Width/2.0f && objectBottom > entry.Value.position.Y)
                        {
                            this.collision_right = true;
                            rightFix = entry.Value.position.X - this.hitbox.Width;
                        }

                        //Collision to the left
                        if (objectLeft < entry.Value.position.X + entry.Value.hitbox.Width && objectRight > entry.Value.position.X + this.hitbox.Width/2.0f && objectBottom > entry.Value.position.Y)
                        {
                            this.collision_left = true;
                            leftFix = entry.Value.position.X + entry.Value.hitbox.Width;
                        }

                        //Collision below
                        if (objectRight - 1 > entry.Value.position.X && objectRight + 5 < entry.Value.position.X + entry.Value.hitbox.Width + this.hitbox.Width && objectBottom >= entry.Value.position.Y && objectTop < entry.Value.position.Y)
                        {
                            this.collision_below = true;
                            belowFix = entry.Value.position.Y - this.hitbox.Height;
                        }

                    }

                    else
                        collidingObject = entry.Value;
                }
            }

            //Move character out of collision
            if (this.collision_above && !this.collision_below)
                this.position.Y = aboveFix;

            else if (!this.collision_above && this.collision_below)
            {
                this.position.Y = belowFix;
                if (!this.grounded)
                    this.grounded = true;
            }

            if (this.collision_left && !this.collision_right)
                this.position.X = leftFix;

            else if (!this.collision_left && this.collision_right)
                this.position.X = rightFix;

            if (!this.collision_below && this.grounded)
                this.grounded = false;

            if (!wasGrounded && this.grounded) //If this is the first frame to touch the ground, reset the object's fall speed
               this.yspeed = this.weight;

            //Now update the hitbox
            this.hitbox.X = (int)this.position.X;
            this.hitbox.Y = (int)this.position.Y;

           return collidingObject; //Return null if not colliding with anything except for normal collision tiles
        }

        public void Update(GameTime gameTime)
        {
            doPhysics();

            //Need to check if an object is on screen to determine if it should be loaded or unloaded DEBUG. 
        }

        public void setSprite(string sprite_path)
        {
            this.sprite.path = sprite_path;
        }

        public void loadSprite(Game1 game)
        {
            this.sprite.loadSprite(game);
        }
    }

}
