using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BoogalooGame.Data_Structures
{
    /// <summary>
    /// Holds information about position of items, enemies, and blocks
    /// </summary>
    class Layer
    {
        private List<GameObjectInfo> collision_list; //List of all the collision
        private List<Sprite> sprite_list; //List of all sprites which should not have any actual code
    }
}
