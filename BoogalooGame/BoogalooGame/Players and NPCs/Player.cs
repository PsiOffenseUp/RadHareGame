using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
