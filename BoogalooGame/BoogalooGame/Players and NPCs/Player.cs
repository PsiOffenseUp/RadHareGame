using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input; //Needed for the controls

namespace BoogalooGame
{
    public class Player : Character
    {
        public int HP;
        public float runSpeed, maxRunSpeed; //maxRunSpeed2 is for when the action button is held while running
        const float jumpHeight = 8.0f;
        const float air_friction = 2.0f;
        const float ground_friction = 1.6f;

        //---------------------Constructors-----------------
        public Player()
        {
            this.HP = 4;
            this.runSpeed = 1.3f;
            this.maxRunSpeed = 4.4f;
        }

        public Player(float xpos, float ypos)
        {
            this.position.X = xpos;
            this.position.Y = ypos;
            this.runSpeed = 0.05f;
            this.maxRunSpeed = 4.2f;
            this.HP = 4;
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
            cntrl.readInputs(); 
            
            //----------Movement-----------
            //Movement on the ground
            if (cntrl.LEFT)
            {
                this.xspeed -= runSpeed;
                if (this.isMovingRight()) //If currently moving to the right, add a little speed boost
                    this.xspeed -= ground_friction*runSpeed;
            }

            if (cntrl.RIGHT)
            {
                this.xspeed += runSpeed;
                if(this.isMovingLeft()) //If currently moving to the right, add a little speed boost
                    this.xspeed += ground_friction * runSpeed;
            }

            if (!cntrl.RIGHT && !cntrl.LEFT && xspeed != 0) //If neither left or right are being held
            {
                
                if (this.isMovingRight())
                    this.xspeed -= 0.95f*ground_friction * runSpeed;

                else if (this.isMovingLeft())
                    this.xspeed += 0.95f * ground_friction * runSpeed;
                
                if (xspeed < 0.03f && xspeed > -0.03f) //Stop the player if their movement is too slow so they don't slide
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
        }
    }
}
