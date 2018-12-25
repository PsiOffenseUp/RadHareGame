using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// In charge of game management. Things like enemy count, saving, and loading. Also draws everything to the screen
/// </summary>

namespace BoogalooGame
{
    public sealed class Regulator //sealed modifer does the same thing as "final", basically
    {
        //----------------Fields----------------
        public Options options;

        //---------------Constructors------------

        //Default constructor
        public Regulator()
        {
            options = new Options();
        }

        //----------------Methods-----------------

        //Is Debug mode activated?
        public bool isDebug()
        {
            return options.debug;
        }

        //Toggle whether or not debug mode is activated
        public void toggleDebug()
        {
            if (options.debug)
                options.debug = false;
            else
                options.debug = true;
        }

        //Draw all object which have been loaded into the game DEBUG
    }
}
