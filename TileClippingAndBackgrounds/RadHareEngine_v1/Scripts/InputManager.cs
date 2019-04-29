using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RadHareEngine_v1.Scripts
{
    public class InputManager
    {
        public static bool[] Keys = new bool[6];

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[0] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W)) ? true : false;
            Keys[1] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)) ? true : false;
            Keys[2] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S)) ? true : false;
            Keys[3] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) ? true : false;
            Keys[4] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q)) ? true : false;
            Keys[5] = (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) ? true : false;
        }
    }
}
