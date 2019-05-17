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

    public class TextRenderer : GameObject
    {
        string Text;
        SpriteFont Font;

        public TextRenderer()
        {

        }
    }

    public class TextDisplayer : Component
    {
        public string Text { private set; get; }
        public SpriteFont Font { private set; get; }

        public TextDisplayer()
        {

        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }
    }
}
