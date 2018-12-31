using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BoogalooGame.Data_Structures;

namespace BoogalooGame
{
    /// <summary>
    /// Holds information about position of items, enemies, and blocks
    /// </summary>
    class Layer
    {

        //For each field, the first dimension is the x grid location and the second dimension is the y grid location
        //Each "grid space" is 32 x 32 pixels.
        private Collision[][] collisionList; //List of all the collision
        private Sprite[][] spriteList; //List of all sprites which should not have any actual code
        private Enemy[][] enemyList;


    }
}
