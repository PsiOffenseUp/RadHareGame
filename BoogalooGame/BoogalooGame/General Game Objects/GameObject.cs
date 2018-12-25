﻿using System;
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
        //--------------Variables--------------
        public Vector2 position; //X and y coordinate of the character
        public Sprite sprite;
        public float xspeed, yspeed;
        private float weight, fall_speed; //How fast an object will fall
        private bool grounded; //Tells whether or not the object is on the ground
        private Rectangle hitbox;
        private bool grabbable; //Can the object be grabbed?, Does the object have collision?
        public long id;
        public static long id_count = 0; //How many ids have been created. DEBUG May need to set a cap to avoid future overflow
        private static IDictionary<long, GameObject> object_dict = new Dictionary<long, GameObject>(); //Holds all active objects
        public bool collision_left, collision_right, collision_above, collision_below;

        //---------------Constructors------------

        //Default constructor
        public GameObject()
        {
            this.position = new Vector2(0.0f, 0.0f);
            this.sprite.path = "UI/carrot"; //DEBUG Default sprite for all Gameobjects

            
            this.weight = 0.25f;
            this.fall_speed = 10.0f;
            this.grounded = false;
            this.xspeed = 0.0f;
            this.yspeed = 0.0f;
            this.hitbox = new Rectangle(0, 0, 32, 32);
            this.grabbable = false;
            this.collision_left = false;
            this.collision_right = false;
            this.collision_above = false;
            this.collision_below = false;
            

            //Add this item to the dictionary of active items
            this.id = id_count; //Set the id to the current id count
            id_count++; //Increase the amount of things which have been spawned

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
