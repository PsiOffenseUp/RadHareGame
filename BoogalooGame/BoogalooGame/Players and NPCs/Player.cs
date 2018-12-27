using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input; //Needed for the controls
using Microsoft.Xna.Framework; //Needed for Texture2D and Color classes

namespace BoogalooGame
{
    public class Player : Character
    {
        public int HP;
        const float runSpeed = 0.08f;
        const float maxRunSpeed = 4.0f; //maxRunSpeed2 is for when the action button is held while running
        const float jumpHeight = 8.0f;
        const float air_friction = 1.85f;
        const float ground_friction = 1.65f;

        //---------------------Constructors-----------------

        //Default constructor
        public Player()
        {
            this.HP = 4;
            this.hitboxColor = Color.Green;
        }

        public Player(float xpos, float ypos)
        {
            this.position.X = xpos;
            this.position.Y = ypos;
            this.HP = 4;
            this.hitboxColor = Color.Green;
        }

        public Player(string sprite_path) : base(sprite_path)
        {
            //Just use the super class's constructor that loads the sprite

            this.HP = 4;
            this.hitboxColor = Color.Green;
        }
        
        //----------------Gets and sets------------------

        public new string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }


        /// <summary>
        /// Read the controls, and the figure out what to do with them
        /// </summary>
        public void readControls()
        {
            //Read all of the inputs from the controller and keyboard
            Options cntrl = controller.options;
            bool debug_before = cntrl.DEBUG;
            float friction;

            cntrl.readInputs();

            if (this.IsGrounded)
                friction = ground_friction;
            else
                friction = air_friction;

            //----------Movement-----------
            //Movement on the ground
            if (cntrl.LEFT)
            {
                this.xspeed -= runSpeed;
                if (this.isMovingRight()) //If currently moving to the right, add a little speed boost
                    this.xspeed -= friction*runSpeed;
            }

            if (cntrl.RIGHT)
            {
                this.xspeed += runSpeed;
                if(this.isMovingLeft()) //If currently moving to the right, add a little speed boost
                    this.xspeed += friction * runSpeed;
            }

            if (!cntrl.RIGHT && !cntrl.LEFT && xspeed != 0) //If neither left or right are being held
            {
                
                if (this.isMovingRight())
                    this.xspeed -= 0.95f*friction*runSpeed;

                else if (this.isMovingLeft())
                    this.xspeed += 0.95f * friction*runSpeed;
                
                if (xspeed < 0.05f && xspeed > -0.05f) //Stop the player if their movement is too slow so they don't slide
                    xspeed = 0;
                    
            }

            if (cntrl.JUMP && IsGrounded)
            {
                this.IsGrounded = false;
                this.yspeed = -1*jumpHeight;
            }

            if (this.xspeed > maxRunSpeed || this.xspeed < -1*maxRunSpeed)
            {
                    if (this.isMovingLeft())
                        this.xspeed = -1*maxRunSpeed;
                    else
                        this.xspeed = maxRunSpeed;
            }


            //Air movement

            //Toggle debug mode
            if (cntrl.DEBUG && !debug_before)
            {
                controller.toggleDebug();
            }
        }
    }
}
