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

        public Collision()
        {
            this.hitbox = new Rectangle(0, 0, 32, 32);
            this.position = new Vector2(0.0f, 0.0f);
        }

        public Collision(float x, float y)
        {
            this.position = new Vector2(x, y);
            this.hitbox = new Rectangle((int)x, (int)y, 32, 32);
        }

        public Collision(float x, float y, int width, int height)
        {
            this.position = new Vector2(x, y);
            this.hitbox = new Rectangle((int)x, (int)y, width, height);
        }
    }
}
