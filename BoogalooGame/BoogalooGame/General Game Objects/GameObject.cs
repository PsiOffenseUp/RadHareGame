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
            this.position.X = xpos;
            this.position.Y = ypos;
        }

        //--------------Gets and sets---------------
       public bool IsGrounded
        {
            get { return this.grounded; }
        }

        public float Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }

        //----------------Function implementation-------------

        //Puts the item into the object dictionary
        public void Load()
        {
            object_dict.Add(new KeyValuePair<long, GameObject>(this.id, this));
        }

        public void Unload()
        {
            object_dict.Remove(this.id);
        }

        public void setPosition(float xPos, float yPos)
        {
            this.position.X = xPos;
            this.position.Y = yPos;
        }

        private GameObject checkCollision()
        {
            //Check for any collision
            GameObject colliding_object = null;

            //Reset all collision before checking
            this.collision_right = false;
            this.collision_left = false;
            this.collision_above = false;
            this.collision_below = false;

            foreach (KeyValuePair<long, GameObject> entry in object_dict)
            {
                
                if (this.position.Y <= entry.Value.position.Y + entry.Value.hitbox.Height) //Collision above
                {
                    if (entry.Value is Collision)
                        this.collision_above = true;
                    else
                        colliding_object = entry.Value;

                }

                if (this.position.X <= entry.Value.position.X + entry.Value.hitbox.Width) //Collision to the left
                {
                    if (entry.Value is Collision)
                        this.collision_left = true;
                    else
                        colliding_object = entry.Value; //Set the colliding object. This may be overwritten later. Make sure not to return, as this will cause floor and wall collisions to turn off
                }

                if (this.position.X + this.hitbox.Width >= entry.Value.position.X) //Collision to the right
                {
                    if (entry.Value is Collision) //If the item collided with is an example of collision, mark collision 
                        this.collision_right = true;
                    else
                        colliding_object = entry.Value;
                }

                if (this.position.Y + this.hitbox.Height >= entry.Value.position.Y) //Collision below
                {
                    if (entry.Value is Collision)
                    {
                        this.collision_below = true;
                        this.grounded = true;
                    }
                    else
                        colliding_object = entry.Value;
                }
            }

            return colliding_object; //Return null if not colliding with anything except for normal collision tiles
        }

        public void Update(GameTime gameTime)
        {
            //Update the speed
            if (!this.grounded)
            {
                this.yspeed += this.weight; //While not on the ground, make the object affected by gravity.
                if (this.yspeed > this.fall_speed)
                    this.yspeed = this.fall_speed;
            }

            else if (this.grounded)
                this.yspeed = 0;

            //Update position, then update the hitbox to be at this position
            this.position.X += xspeed;
            this.position.Y += yspeed;

            this.checkCollision(); //May need to do something with the object collided with

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
