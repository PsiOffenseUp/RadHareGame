using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_01
{
    public class Button : GameObject
    {
        public Button(Texture2D[] Sprites, Vector2 pos)
        {
            this.AttachComponent<SpriteRenderer>().AttachComponent<MouseTracker>().AttachComponent<ButtonController>();
            this.GetComponent<ButtonController>().SetSprites(Sprites);
            this.GetComponent<MouseTracker>().SetSize(this.GetComponent<SpriteRenderer>().sprite.Image.Bounds.Size.ToVector2());
            transform.SetPosition(pos);
        }
    }
}
