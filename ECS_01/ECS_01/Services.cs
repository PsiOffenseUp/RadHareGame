using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_01
{
    //public void DrawUI(List<GameObject> o)
    //{
    //    sb.Begin();
    //    foreach(GameObject obj in o)
    //    {
    //        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
    //        sb.Draw(sr.sprite.Image,
    //                obj.transform.GetPosition() + sr.sprite.Image.Bounds.Size.ToVector2() / 2.0f,
    //                null,
    //                null,
    //                sr.GetSpriteCenter(),
    //                (obj.transform.GetRotation() + sr.sprite.Rotation),
    //                (obj.transform.GetScale() * sr.sprite.Scale),
    //                null,
    //                SpriteEffects.None,
    //                sr.sprite.layerDepth);
    //    }
    //    sb.End();
    //}

    public class ServiceManager //Service Manager "Manager of Managers"
    {
        public List<ComponentManager> Managers;
        Game1 game;

        public ServiceManager(Game1 gameRef)
        {
            game = gameRef;
            Managers = new List<ComponentManager>();
            this.AddService<SpriteManager>(game);
            this.AddService<TextManager>(game);
            this.AddService<CameraManager>(game); this.GetService<CameraManager>().SetScreenSize(new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        }
        /// <summary>
        /// Returns a list of all gameObjects that contain a Component of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<GameObject> GetObjectComponentList<T>(Component c)
        {
            return game.gameObjects.FindAll((x) => x.Components.);
        }

        public void PerformService()
        {
            foreach(ComponentManager cm in Managers)
            {
                cm.Update();
            }
        }
    }

    public class SpriteManager : ComponentManager
    {
        List<SpriteRenderer> sprites;

        public SpriteManager()
        {
            sprites = new List<SpriteRenderer>();
        }

        public override void Update(Component c)
        {
            foreach(GameObject o in game.gameObjects)
            {
                if(o.GetComponent<SpriteRenderer>().Exists())
                {
                    sprites.Add(o.GetComponent<SpriteRenderer>());
                }
            }
            base.Update(c);
        }

        public override void Draw(Component c)
        {
            DrawSprite();
            base.Draw(c);
        }

        public void QueueSprite(SpriteRenderer sr)
        {
            sprites.Add(sr);
        }

        public void DrawSprite()
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, sm.GetService<CameraManager>().GetMatrix());
            for (int i = 0; i < sprites.Count; i++)
            {
                GameObject o = sprites[i].gameObject;
                sb.Draw(sprites[i].sprite.Image,
                    o.transform.GetPosition(),
                    null,
                    null,
                    sprites[i].GetSpriteCenter(),
                    (o.transform.GetRotation() + sprites[i].sprite.Rotation),
                    (o.transform.GetScale() * sprites[i].sprite.Scale),
                    null,
                    SpriteEffects.None,
                    sprites[i].sprite.layerDepth);
            }
            sb.End();
            sprites.Clear();
        }
    }

    public class TextManager : ComponentManager //Displays the Text from a TextDisplayer Component
    {
        public TextManager()
        {

        }

        public override void Update(Component c)
        {
            base.Update(c);
        }

        public override void Draw(Component c)
        {
            base.Draw(c);
        }

        public void DrawText(TextDisplayer td)
        {
            sb.Begin();
            sb.DrawString(td.Font, td.Text, td.gameObject.transform.GetPosition() + td.Offset, td.Color);
            sb.End();
        }
    }

    public class CameraManager : ComponentManager
    {
        public List<Camera> AllCameras;
        public static Camera MainCamera;
        public Vector2 ScreenSize;

        public CameraManager()
        {
            AllCameras = new List<Camera>();
        }

        public override void Update(Component c)
        {
            base.Update(c);
        }

        public override void Draw(Component c)
        {
            base.Draw(c);
        }

        public void AddCamera(Camera cam)
        {
            AllCameras.Add(cam);
        }

        public void ClearCameras()
        {
            AllCameras.Clear();
        }

        public void SetMainCamera(Camera cam)
        {
            if(cam.Exists())    MainCamera = cam;
        }

        public void SetScreenSize(Vector2 size)
        {
            ScreenSize = size;
        }

        public Matrix GetMatrix()
        {
            GameObject o = MainCamera.gameObject;
            Vector2 Origin = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2) / MainCamera.Zoom;
            Matrix Transform = Matrix.Identity *
                               Matrix.CreateTranslation(-o.transform.GetPosition().X, -o.transform.GetPosition().Y, 0) *
                               ((o.GetComponent<Camera>().relativeTo == Camera.RelativeTo.GAMEOBJECT)? Matrix.CreateRotationZ(0) : Matrix.CreateRotationZ(-o.transform.GetRotation()))* //0 for object revolve around world, -object rotation for world to revolve around object
                               Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                               Matrix.CreateScale(MainCamera.Zoom.X, MainCamera.Zoom.Y, 1.0f);

            return Transform;
        }
    }

    public abstract class ComponentManager
    {
        protected Game1 game;
        protected SpriteBatch sb;
        public ServiceManager sm;

        public ComponentManager()
        {
            
        }

        public virtual void Update(Component c) //used if the manager performs actions during the update loop
        {

        }

        public virtual void Draw(Component c) //used if the manager performs actions during the draw loop
        {

        }

        public void SetParent(ServiceManager sm)
        {
            this.sm = sm;
        }

        public void setGameRef(Game1 game)
        {
            this.game = game;
            this.sb = game.GetSpriteBatch();
        }
    }
}
