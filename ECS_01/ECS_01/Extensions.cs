using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_01
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the first Component of type T from the referenced GameObject. The Component.Exists() should be used in conjunction to prevent operating with a null reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T GetComponent<T>(this GameObject o) where T : Component
        {
            foreach (Component c in o.Components)
            {
                if (c is T)
                {
                    return (c as T);
                }
            }
            return null;
        }
        /// <summary>
        /// Adds a component of type T to the referenced GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        public static void AddComponent<T>(this GameObject o) where T : Component, new()
        {
            T comp = new T();
            comp.gameObject = o;
            comp.Start();
            o.Components.Add(comp);
        }
        /// <summary>
        /// Adds a component of type T to the referenced GameObject. Returns itself for stringing together component edits.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        public static GameObject AttachComponent<T>(this GameObject o) where T : Component, new()
        {
            T comp = new T();
            comp.gameObject = o;
            comp.Start();
            o.Components.Add(comp);
            return o;
        }
        /// <summary>
        /// Adds a pre-made Component to the referenced GameObject.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="component"></param>
        public static void AddComponent(this GameObject o, Component component)
        {
            component.gameObject = o;
            o.Components.Add(component);
        }
        /// <summary>
        /// Removes the first occurrance of a Component type T from the referenced GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        public static void RemoveComponent<T>(this GameObject o) where T : Component
        {
            o.Components.Remove(o.GetComponent<T>());
        }
        /// <summary>
        /// Removes the first occurrance of the pre-made Component of type T from the referenced GameObject.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="component"></param>
        public static void RemoveComponent(this GameObject o, Component component)
        {
            o.Components.Remove(component);
        }
        /// <summary>
        /// Sets the supplied GameObject as a child of the referenced GameObject.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="child"></param>
        public static void SetChild(this GameObject o, GameObject child)
        {
            if (!o.Children.Contains(child))
            {
                child.Parent = o;
                o.Children.Add(child);
            }
        }
        /// <summary>
        /// Removes the supplied child GameObject from the referenced GameObjects Children List.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="child"></param>
        public static void RemoveChild(this GameObject o, GameObject child)
        {
            if (o.Children.Contains(child))
            {
                child.Parent = null;
                o.Children.Remove(child);
            }
        }
        /// <summary>
        /// Transforms the referenced Vector2 from World Space to the supplied GameObject's Object Space.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static Vector2 ToObjectSpace(this Vector2 v, GameObject go)
        {
            return new Vector2(v.X * (float)Math.Cos(go.transform.GetRotation()) - v.Y * (float)Math.Sin(go.transform.GetRotation()), v.X * (float)Math.Sin(go.transform.GetRotation()) + v.Y * (float)Math.Cos(go.transform.GetRotation()));
        }
        /// <summary>
        /// Transforms the referenced Vector2 from the supplied GameObject's Object Space to World Space.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static Vector2 ToWorldSpace(this Vector2 v, GameObject go)
        {
            return Vector2.One; //needs implementing
        }
        /// <summary>
        /// Converts a float value Angle from Radians to Degrees.
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static float ToDegrees(this float Angle)
        {
            return Angle * (180.0f / (float)Math.PI);
        }
        /// <summary>
        /// Converts a float value Angle from Degrees to Radians.
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static float ToRadians(this float Angle)
        {
            return Angle * ((float)Math.PI / 180.0f);
        }
        /// <summary>
        /// Rotates the angle of the supplied Vector2 by the supplied float Angle. The Angle units are specified with the AngleType.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static Vector2 RotateVector(this Vector2 v, float Angle, AngleType angleType)
        {
            return new Vector2(v.X * (float)Math.Cos(Angle) - v.Y * (float)Math.Sin(Angle), v.X * (float)Math.Sin(Angle) + v.Y * (float)Math.Cos(Angle));
        }
        /// <summary>
        /// Returns true if the supplied Component object is not null. Used in conjunction with GameObject.GetComponent<T>().
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Exists(this Component c)
        {
            return (c != null);
        }

        public static T AddService<T>(this ServiceManager sm, Game1 game) where T : ComponentManager, new()
        {
            T manager = new T();
            manager.setGameRef(game);
            manager.SetParent(sm);
            sm.Managers.Add(manager);
            return manager;
        }

        public static T GetService<T>(this ServiceManager sm) where T : ComponentManager
        {
            foreach(ComponentManager cm in sm.Managers)
            {
                if (cm is T) return (cm as T);
            }
            return null;
        }
    }
}
