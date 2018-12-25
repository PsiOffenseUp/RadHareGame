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
        private string name;

        public Player()
        {
            this.HP = 4;
        }

        public Player(float xpos, float ypos)
        {
            this.position.X = xpos;
            this.position.Y = ypos;
        }
        

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
            controller.options.readInputs(); 

            //----------Movement-----------
            //Movement on the ground

            //Air movement

            //Toggle debug mode
        }
    }
}
