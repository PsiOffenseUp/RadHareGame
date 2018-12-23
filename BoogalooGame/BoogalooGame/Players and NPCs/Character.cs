using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Classes for player, enemies, npcs, and general game objects

namespace BoogalooGame
{
    /// <summary>
    /// Abstract class for all NPCs, enemies, and Player. Should never be instantiated. Contains basic information like current sprite and position
    /// Default constructor sets position to origin, sprite to carrot, and name to "Default"
    /// </summary>
    public abstract class Character : GameObject
    {
        private string name; //Current sprite path for the object and its name
        private int id;
        private static int count = 0; //How many characters are on screen
        
        public Character()
        {
            this.id = count; //Set the id to the current count, and then increment
            count++;
            this.name = "Default";
        }

        //Access private fields with weird C# feature. Cannot do with normal functions.
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

    }
}
