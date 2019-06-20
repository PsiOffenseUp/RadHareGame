using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ECS_01
{
    public enum RelativeTo { WORLD, OBJECT };
    public enum AngleType { RADIANS, DEGREES };

    public class GameObject
    {
        public static Game1 game; //Reference to the game

        public class Transform
        {
            Vector2 position, scale;
            float Rotation;
            public GameObject gameObject;

            public Transform(GameObject go)
            {
                position = Vector2.Zero;
                scale = Vector2.One;
                Rotation = 0.0f;
                gameObject = go;
            }
            #region Public Member Functions
            public void Translate(Vector2 t, RelativeTo relativeTo = RelativeTo.WORLD)
            {
                if (relativeTo == RelativeTo.WORLD)
                {
                    position += t;
                    foreach(GameObject o in this.gameObject.Children)
                    {
                        o.transform.position += t;
                    }
                }
                else
                {
                    //Rotational correction code
                    position += t.ToObjectSpace(gameObject);
                    foreach(GameObject o in this.gameObject.Children)
                    {
                        o.transform.position += t.ToObjectSpace(gameObject);
                    }
                }
            }
            public void SetPosition(Vector2 p)
            {
                position = p;
            }
            public void Scale(Vector2 s)
            {
                scale *= s;
                foreach(GameObject o in gameObject.Children)
                {
                    o.transform.scale *= s;
                }
            }
            public void SetScale(Vector2 s)
            {
                scale = s;
            }
            public void Rotate(float angle, AngleType aType)
            {
                if(aType == AngleType.RADIANS)
                {
                    Rotation += angle;
                    foreach(GameObject o in gameObject.Children)
                    {
                        //some sort of rotation matrix? 
                    }
                }
                else
                {
                    Rotation += angle * ((float)Math.PI / 180.0f);
                    foreach (GameObject o in gameObject.Children)
                    {
                        //some sort of rotation matrix?
                    }
                }
            }
            public void SetRotation(float angle, AngleType aType)
            {
                if(aType == AngleType.RADIANS)
                    Rotation = angle;
                else
                {
                    Rotation = angle * ((float)Math.PI / 180.0f);
                }
            }
            public Vector2 GetPosition()
            {
                return position;
            }
            public Vector2 GetScale()
            {
                return scale;
            }
            public float GetRotation()
            {
                return Rotation;
            }
            public Vector2 Forward()
            {
                return new Vector2(1, 0).RotateVector(Rotation, AngleType.DEGREES);
            }
            public Vector2 Backward()
            {
                return new Vector2(1, 0).RotateVector(Rotation + (float)Math.PI, AngleType.DEGREES);
            }
            public Vector2 Left()
            {
                return new Vector2(1, 0).RotateVector(Rotation - ((float)Math.PI / 2.0f), AngleType.DEGREES);
            }
            public Vector2 Right()
            {
                return new Vector2(1, 0).RotateVector(Rotation - (3.0f * (float)Math.PI / 2.0f), AngleType.DEGREES);
            }
            #endregion
        }

        public Transform transform;
        public List<Component> Components;
        public List<GameObject> Children;
        public GameObject Parent;

        //--------------------------------------Constructors-------------------------------

        public GameObject()
        {
            transform = new Transform(this);
            Components = new List<Component>();
            Children = new List<GameObject>();
            Parent = null;
        }

        //-----------------------------------------Methods---------------------------------------
        public void Load()
        {
            game.gameObjects.Add(this); //Add this object to the list of currently active objects
        }

        public void Unload()
        {
            game.gameObjects.Remove(this);
        }
    }
}
