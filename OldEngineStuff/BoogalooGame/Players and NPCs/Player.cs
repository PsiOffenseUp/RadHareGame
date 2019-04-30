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
        const float maxRunSpeed = 4.2f; //maxRunSpeed2 is for when the action button is held while running
        const float jumpHeight = 5.6f;
        const float air_friction = 1.8f;
        const float ground_friction = 1.65f;
        const float jumpGain = 0.12f; //How much extra height is gained by holding down jump. Decreases a bit on the way down

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
            bool jumpBefore = cntrl.JUMP;
            float friction;

            cntrl.readInputs();

            if (this.IsGrounded)
                friction = ground_friction;
            else
            {
                if (this.isMovingDown())
                    friction = air_friction;
                else
                    friction = 0.85f * air_friction;
            }

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

            if (cntrl.JUMP && IsGrounded) //&& !jumpBefore) Add back when collision is fixed. DEBUG
            {
                this.IsGrounded = false;
                this.yspeed = -1*jumpHeight;
            }

            if (cntrl.JUMP && !IsGrounded)
            {
                if (this.isMovingUp())
                    this.yspeed -= jumpGain;
                else
                    this.yspeed -= 0.2f * jumpGain;
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
