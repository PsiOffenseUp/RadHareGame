using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BoogalooGame
{

    class Collision:GameObject
    {
        private Rectangle hitbox;
        private Vector2 position;

        //-------------------------Constructors-------------------
        //If not provided a sprite path to load, will automatically load the test tile "block-192"
        public Collision() : base("tiles/block-192")
        {
            this.hitbox = new Rectangle(0, 0, 32, 32);
            this.position = new Vector2(0.0f, 0.0f);
            this.hitboxColor = Color.Blue;
        }

        public Collision(float x, float y) : base("tiles/block-192")
        {
            this.position = new Vector2(x, y);
            this.hitbox = new Rectangle((int)x, (int)y, 32, 32);
            this.hitboxColor = Color.Blue;
        }

        public Collision(float x, float y, int width, int height) : base("tiles/block-192")
        {
            this.position = new Vector2(x, y);
            this.hitbox = new Rectangle((int)x, (int)y, width, height);
            this.hitboxColor = Color.Blue;
        }

        public Collision(string sprite_path) : base(sprite_path)
        {
            //Just use the parent's constructor
            this.hitboxColor = Color.Blue;
        }

        //Does the same as the other position and size constructor, but also sets the sprite, too
        public Collision(float x, float y, int width, int height, string sprite_path) : base(sprite_path)
        {
            this.position = new Vector2(x, y);
            this.hitbox = new Rectangle((int)x, (int)y, width, height);
            this.hitboxColor = Color.Blue;
        }
    }
}
