using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; //Needed for Texture2D class


namespace BoogalooGame.Data_Structures
{
    /// <summary>
    /// Stores a GameObject and information about it's spawning properties
    /// </summary>
    class GameObjectInfo
    {
        GameObject obj;
        Vector2 position;
        Sprite sprite; //Debug, may want to replace this with Sprite class
        
        public GameObjectInfo() //Default constructor, if nothing has been specified
        {
            this.obj = null;
            this.position = new Vector2(0.0f, 0.0f); //If nothing has been specified
        }

        public GameObjectInfo(GameObject objct, Vector2 pos, Sprite spr) //Normal constructor. Should always be used
        {
            this.obj = objct;
            this.position = pos;
            this.sprite = spr;
        }
    }
}
