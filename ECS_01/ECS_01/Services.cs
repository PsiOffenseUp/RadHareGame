using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_01
{
    class Services
    {
        //Need to change the services/ systems to be delegates that the individual components can subscribe/unsubscribe to. This would prevent each system from having to search through the entire gameobject list multiple times to find specific components.


        SpriteBatch sb;
        CameraManager cm;
        Game1 game;

        public Services(SpriteBatch sb, Game1 game)
        {
            this.sb = sb;
            cm = new CameraManager(game);
            cm.SetMainCamera(game.gameObjects[0].GetComponent<Camera>());
        }
        /// <summary>
        /// Recieves all gameObjects and divides them up between all services based on their connected components.
        /// </summary>
        /// <param name="gameObjects"></param>
        public void Routing(List<GameObject> gameObjects)
        {
            List<GameObject> Sprites = new List<GameObject>();


            foreach(GameObject o in gameObjects)
            {
                if (o.GetComponent<SpriteRenderer>().Exists()) Sprites.Add(o);
                if(o.GetComponent<Camera>().Exists() && cm.AllCameras.Count > 0) { cm.SetMainCamera(o.GetComponent<Camera>());}
            }

            SpriteRendering(Sprites);
        }

        public void DrawUI(List<GameObject> o)
        {
            sb.Begin();
            foreach(GameObject obj in o)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sb.Draw(sr.sprite.Image,
                        obj.transform.GetPosition() + sr.sprite.Image.Bounds.Size.ToVector2() / 2.0f,
                        null,
                        null,
                        sr.GetSpriteCenter(),
                        (obj.transform.GetRotation() + sr.sprite.Rotation),
                        (obj.transform.GetScale() * sr.sprite.Scale),
                        null,
                        SpriteEffects.None,
                        sr.sprite.layerDepth);
            }
            sb.End();
        }

        public void SpriteRendering(List<GameObject> objects)
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraManager.GetMatrix());
            foreach(GameObject o in objects)
            {
                SpriteRenderer sr = o.GetComponent<SpriteRenderer>();
                sb.Draw(sr.sprite.Image,
                    o.transform.GetPosition(),
                    null,
                    null,
                    sr.GetSpriteCenter(),
                    (o.transform.GetRotation() + sr.sprite.Rotation),
                    (o.transform.GetScale() * sr.sprite.Scale),
                    null,
                    SpriteEffects.None,
                    sr.sprite.layerDepth);
            }

            sb.End();
        }
    }

    public class SpriteManager //beta manager for sprite drawing through event firing
    {
        SpriteBatch sb;
        List<SpriteRenderer> sprites;

        public SpriteManager(SpriteBatch sb)
        {
            sprites = new List<SpriteRenderer>();
            this.sb = sb;
        }

        public void QueueSprite(SpriteRenderer sr)
        {
            sprites.Add(sr);
        }

        public void Draw()
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraManager.GetMatrix());
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
        }
    }

    public class CameraManager
    {
        public List<Camera> AllCameras;
        public static Camera MainCamera;

        static Game1 Game;

        public CameraManager(Game1 game)
        {
            AllCameras = new List<Camera>();
            Game = game;
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

        public static Matrix GetMatrix()
        {
            GameObject o = MainCamera.gameObject;
            Vector2 Origin = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2) / MainCamera.Zoom;
            Matrix Transform = Matrix.Identity *
                               Matrix.CreateTranslation(-o.transform.GetPosition().X, -o.transform.GetPosition().Y, 0) *
                               ((o.GetComponent<Camera>().relativeTo == Camera.RelativeTo.GAMEOBJECT)? Matrix.CreateRotationZ(0) : Matrix.CreateRotationZ(-o.transform.GetRotation()))* //0 for object revolve around world, -object rotation for world to revolve around object
                               Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                               Matrix.CreateScale(MainCamera.Zoom.X, MainCamera.Zoom.Y, 1.0f);

            return Transform;
        }
    }
}
