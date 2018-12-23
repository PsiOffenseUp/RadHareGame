using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoogalooGame
{
    public abstract class GameObject
    {
        //Variables
        public Vector2 position; //X and y coordinate of the character
        public string sprite_path;
        public Texture2D sprite;
        public float xspeed, yspeed;
        private float weight, fall_speed; //How fast an object will fall
        private bool grounded; //Tells whether or not the object is on the ground
        private Rectangle hitbox;
        private bool grabbable; //Can the object be grabbed


        //Functions (Methods)

        public GameObject()
        {
            this.position = new Vector2(0.0f, 0.0f);
            this.sprite_path = "UI/carrot";

            this.weight = 0.25f;
            this.fall_speed = 10.0f;
            this.grounded = false;
            this.xspeed = 0.0f;
            this.yspeed = 0.0f;
            this.hitbox = new Rectangle(0, 0, 32, 32);
            this.grabbable = false;
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

        public void Update(GameTime gameTime)
        {
            if (!this.IsGrounded)
            {
                this.yspeed += this.weight; //While not on the ground, make the object affected by gravity.
                if (this.yspeed > this.fall_speed)
                    this.yspeed = this.fall_speed;
            }

            this.position.X += xspeed;
            this.position.Y += yspeed;
        }
    }

}
