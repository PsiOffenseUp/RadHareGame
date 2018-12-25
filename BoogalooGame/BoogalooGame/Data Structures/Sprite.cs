using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //Needed for Texture2D class
using Microsoft.Xna.Framework;

namespace BoogalooGame
{
    public class Sprite
    {
        public Texture2D texture; //The actual image. DEBUG May change to private later
        public string path = "";
        int width, height;
        float scale; //Size of the object

        //Default constructor, make a sprite, but do nothing with it
        public Sprite()
        {
            this.path = "UI/carrot";
            this.width = 32;
            this.height = 32;
            this.scale = 1.0f;
        }

        //constructor to load the sprite immediately
        public Sprite(Game1 game, string spr_path) 
        {
            this.texture = game.Content.Load<Texture2D>(spr_path);
            this.width = this.texture.Width;
            this.height = this.texture.Height;
            this.scale = 1.0f;
            this.path = spr_path;
        }

        //Methods

        /// <summary>
        /// Loads a sprite to the game, which must be passed in
        /// </summary>
        /// <param name="game"></param>
        public void loadSprite(Game1 game)
        {
            this.texture = game.Content.Load<Texture2D>(path);
            this.width = this.texture.Width;
            this.height = this.texture.Height;
        }

    }
}
