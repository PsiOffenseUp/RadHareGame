using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_01
{
    public class ServiceManager //Service Manager "Manager of Managers"
    {
        public List<ComponentManager> Managers;
        public static Game1 game;

        public ServiceManager()
        {
            Managers = new List<ComponentManager>();

            /*******Add Services Here*******/

            this.AddService<SpriteManager>(game);
            this.AddService<TextManager>(game);
            this.AddService<CameraManager>(game); this.GetService<CameraManager>().SetScreenSize(new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        }

        public void Update()
        {
            foreach (ComponentManager cm in Managers)
            {
                foreach (GameObject go in game.gameObjects)
                {
                    foreach (Component c in go.Components)
                    {
                        if (cm.compType == c.GetType())
                        {
                            cm.Components.Add(c);
                        }
                    }
                }

                cm.Update();
                cm.Components.Clear();
            }
        }
    }

    public class SpriteManager : ComponentManager
    {
        public SpriteManager()
        {
            
            compType = typeof(SpriteRenderer);
        }

        public override void Update()
        {
            DrawSprite();
            base.Update();
        }

        public void DrawSprite()
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, sm.GetService<CameraManager>().GetMatrix());

            for (int i = 0; i < Components.Count; i++)
            {
                SpriteRenderer sr = Components[i] as SpriteRenderer;
                sb.Draw(sr.sprite.Image,
                    sr.gameObject.transform.GetPosition(),
                    null,
                    null,
                    sr.GetSpriteCenter(),
                    (sr.gameObject.transform.GetRotation() + sr.sprite.Rotation),
                    (sr.gameObject.transform.GetScale() * sr.sprite.Scale),
                    null,
                    SpriteEffects.None,
                    sr.sprite.layerDepth);
            }
            sb.End();
            Components.Clear();
        }
    }

    public class TextManager : ComponentManager //Displays the Text from a TextDisplayer Component
    {
        public TextManager()
        {
            compType = typeof(TextDisplayer);
        }

        public override void Update()
        {
            base.Update();
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
            compType = typeof(Camera);
        }

        public override void Update()
        {
            base.Update();
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
        public Type compType;
        protected Game1 game;
        protected SpriteBatch sb;
        public ServiceManager sm;
        public List<Component> Components;

        public ComponentManager()
        {
            Components = new List<Component>();
        }

        public virtual void Update() //used if the manager performs actions during the update loop
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
