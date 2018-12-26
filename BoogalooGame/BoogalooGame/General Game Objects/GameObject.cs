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

        public float xspeed, yspeed;
        private float weight, fall_speed; //How fast an object will fall
        public long id;

        private bool grounded; //Tells whether or not the object is on the ground
        private bool grabbable; //Can the object be grabbed?
        public bool collision_left, collision_right, collision_above, collision_below; //Checks for collision with floors and walls

        public static long id_count; //How many ids have been created. DEBUG May need to set a cap to avoid future overflow
        private static IDictionary<long, GameObject> object_dict; //Holds all active objects
        public static Regulator controller; //Controls most aspects of the game


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

        private GameObject checkCollision()
        {
            //Check for any collision
            GameObject colliding_object = null;

            bool wasGrounded = this.grounded; //Used to check if the object was grounded on the previous frame

            //Reset all collision before checking
            this.collision_right = false;
            this.collision_left = false;
            this.collision_above = false;
            this.collision_below = false;

            foreach (KeyValuePair<long, GameObject> entry in object_dict)
            {
                if (this.hitbox.Intersects(entry.Value.hitbox)) //Only check if they are actually colliding
                {
                    if (this.position.Y <= entry.Value.position.Y + entry.Value.hitbox.Height) //Collision above
                    {
                        if (entry.Value is Collision)
                        {
                            this.collision_above = true;

                            //Check if the objects are pretty close to horizontally aligned. DEBUG May need a better fix for this
                            if (this.position.X <= entry.Value.position.X + entry.Value.hitbox.Width && this.position.X >= entry.Value.position.X)
                                this.position.Y = entry.Value.position.Y + entry.Value.hitbox.Height + 0.001f;
                        }
                        else
                            colliding_object = entry.Value;

                    }

                    if (this.position.X <= entry.Value.position.X + entry.Value.hitbox.Width) //Collision to the left
                    {
                        if (entry.Value is Collision)
                        {
                            this.collision_left = true;

                            if (this.position.Y <= entry.Value.position.Y + entry.Value.hitbox.Height && this.position.Y >= entry.Value.position.Y)
                                this.position.X = entry.Value.position.X + entry.Value.hitbox.Width;
                        }
                        else
                            colliding_object = entry.Value; //Set the colliding object. This may be overwritten later. Make sure not to return, as this will cause floor and wall collisions to turn off
                    }

                    if (this.position.X + this.hitbox.Width >= entry.Value.position.X) //Collision to the right
                    {
                        if (entry.Value is Collision) //If the item collided with is an example of collision, mark collision 
                        {
                            this.collision_right = true;

                            if (this.position.Y <= entry.Value.position.Y + entry.Value.hitbox.Height && this.position.Y >= entry.Value.position.Y)
                                this.position.X = entry.Value.position.X - this.hitbox.Width;
                        }
                        else
                            colliding_object = entry.Value;
                    }

                    if (this.position.Y + this.hitbox.Height >= entry.Value.position.Y) //Collision below
                    {
                        if (entry.Value is Collision)
                        {
                            this.collision_below = true;
                            if (this.position.X <= entry.Value.position.X + entry.Value.hitbox.Width && this.position.X >= entry.Value.position.X)
                                this.position.Y = entry.Value.position.Y - this.hitbox.Height;

                            if (!this.grounded)
                                this.grounded = true;
                        }
                        else
                            colliding_object = entry.Value;
                    }
                }
            }
            
            if (!collision_below) //Set the object to be in the air if it does not have collision directly below it
                this.grounded = false;

           if (!wasGrounded && this.grounded) //If this is the first frame that the object is hitting the ground, set yspeed to 0
                this.yspeed = 0;

            return colliding_object; //Return null if not colliding with anything except for normal collision tiles
        }

        public void Update(GameTime gameTime)
        {
            bool wasGrounded = this.grounded;
            this.checkCollision(); //May need to do something with the object collided with

            //Update the speed

            if (!this.grounded)
            {
                this.yspeed += this.weight; //While not on the ground, make the object affected by gravity.
                if (this.yspeed > this.fall_speed)
                    this.yspeed = this.fall_speed;
            }

            //Update position
            this.position.X += xspeed;
            this.position.Y += yspeed;

            //Update the hitbox
            this.hitbox.X = (int)this.position.X;
            this.hitbox.Y = (int)this.position.Y;
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
