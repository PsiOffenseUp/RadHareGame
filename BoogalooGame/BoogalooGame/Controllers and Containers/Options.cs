using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; //For reading from files
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input; //Needed for the controls

namespace BoogalooGame
{
    /// <summary>
    /// Container class to hold active options
    /// </summary>
    public class Options
    {
        public bool debug; //Should debug features be used?

        //Related to controls
        public bool RIGHT, LEFT , UP, DOWN, JUMP, ACTION, PAUSE, SELECT, DEBUG;
        private Keys rKey, lKey, uKey, dKey, jKey, aKey; //Used to figure out what key should be used for right, left, up, down, jump, and action
        private Buttons rBut, lBut, uBut, dBut, jBut, aBut; //Same as above, but for controller
        const float dead_zone = 0.5f; //Dead zone for the controllers. I think it should be between 0 and 1.



        public Options()
        {
            debug = false;

            //Set controls to be no buttons pressed on startup
            RIGHT = false;
            LEFT = false;
            UP = false;
            DOWN = false;
            JUMP = false;
            ACTION = false;
            DOWN = false;

            readControlOptions(); //Load controller options from a file

            //Should read from a file instead for default constructor, DEBUG
        }

        /// <summary>
        /// Resets the controls to the defualt in both the file and the actual controls
        /// </summary>
        public void resetControls()
        {
            rKey = Keys.D;
            lKey = Keys.A;
            uKey = Keys.W;
            dKey = Keys.S;
            jKey = Keys.Space;
            aKey = Keys.LeftShift;

            rBut = Buttons.DPadRight;
            lBut = Buttons.DPadLeft;
            uBut = Buttons.DPadUp;
            dBut = Buttons.DPadDown;
            jBut = Buttons.A;
            aBut = Buttons.X;

            //Overwrite file
            string path = System.IO.Directory.GetCurrentDirectory() + "/controls.txt";

            using (StreamWriter w_file = File.CreateText(path))
            {
                w_file.WriteLine("D");
                w_file.WriteLine("A");
                w_file.WriteLine("W");
                w_file.WriteLine("S");
                w_file.WriteLine("Sp"); //Jump
                w_file.WriteLine("LS"); //Action

                w_file.WriteLine("DR");
                w_file.WriteLine("DL");
                w_file.WriteLine("DU");
                w_file.WriteLine("DD");
                w_file.WriteLine("A");
                w_file.WriteLine("X");
            }
        }

        /// <summary>
        /// Takes the string input and then gives the key that correponds to it.
        /// Useful for when reading control settings from a file
        /// </summary>
        /// <param name="input"></param>
        /// <returns>"Returns the correct key based on the input string"</returns>
        public static Keys getKeyFromString(string input)
        {
            switch (input)
            {
                case "Q":
                    return Keys.Q;
                case "W":
                    return Keys.W;
                case "E":
                    return Keys.E;
                case "R":
                    return Keys.R;
                case "T":
                    return Keys.T;
                case "Y":
                    return Keys.Y;
                case "U":
                    return Keys.U;
                case "I":
                    return Keys.I;
                case "O":
                    return Keys.O;
                case "P":
                    return Keys.P;
                case "A":
                    return Keys.A;
                case "S":
                    return Keys.S;
                case "D":
                    return Keys.D;
                case "F":
                    return Keys.F;
                case "G":
                    return Keys.G;
                case "H":
                    return Keys.H;
                case "J":
                    return Keys.J;
                case "K":
                    return Keys.K;
                case "L":
                    return Keys.L;
                case "Z":
                    return Keys.Z;
                case "X":
                    return Keys.X;
                case "C":
                    return Keys.C;
                case "V":
                    return Keys.V;
                case "B":
                    return Keys.B;
                case "N":
                    return Keys.N;
                case "M":
                    return Keys.M;
                case "1":
                    return Keys.D1;
                case "2":
                    return Keys.D2;
                case "3":
                    return Keys.D3;
                case "4":
                    return Keys.D4;
                case "5":
                    return Keys.D5;
                case "6":
                    return Keys.D6;
                case "7":
                    return Keys.D7;
                case "8":
                    return Keys.D8;
                case "9":
                    return Keys.D9;
                case "0":
                    return Keys.D0;
                case "N1":
                    return Keys.NumPad1;
                case "N2":
                    return Keys.NumPad2;
                case "N3":
                    return Keys.NumPad3;
                case "N4":
                    return Keys.NumPad4;
                case "N5":
                    return Keys.NumPad5;
                case "N6":
                    return Keys.NumPad6;
                case "N7":
                    return Keys.NumPad7;
                case "N8":
                    return Keys.NumPad8;
                case "N9":
                    return Keys.NumPad9;
                case "N0":
                    return Keys.NumPad0;
                case "Sp":
                    return Keys.Space;
                case "LS":
                    return Keys.LeftShift;
                case "RS":
                    return Keys.RightShift;
                case "LC":
                    return Keys.LeftControl;
                case "RC":
                    return Keys.RightControl;
                case "En":
                    return Keys.Enter;
                default:
                    return Keys.Space;
            }
        }

        /// <summary>
        /// Returns the button on a gamepad based on the 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Buttons getButtonFromString(string input)
        {
            switch(input)
            {
                case "A":
                    return Buttons.A;
                case "B":
                    return Buttons.B;
                case "X":
                    return Buttons.X;
                case "Y":
                    return Buttons.Y;
                case "R":
                    return Buttons.RightTrigger;
                case "L":
                    return Buttons.LeftTrigger;
                case "ZR":
                    return Buttons.RightShoulder;
                case "ZL":
                    return Buttons.LeftShoulder;
                case "DL":
                    return Buttons.DPadLeft;
                case "DR":
                    return Buttons.DPadRight;
                case "DU":
                    return Buttons.DPadUp;
                case "DD":
                    return Buttons.DPadDown;
                case "S":
                    return Buttons.Start;
                case "Se":
                    return Buttons.Back;
                default:
                    return Buttons.A;
            }
        }

        /// <summary>
        /// Reads the control options from a file
        /// </summary>
        public void readControlOptions()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/controls.txt";

            if (!File.Exists(path)) //Make sure the options file exists, if not, write a default one
            {
                using (StreamWriter w_file = File.CreateText(path))  //Create the file
                {
                    w_file.WriteLine("D");
                    w_file.WriteLine("A");
                    w_file.WriteLine("W");
                    w_file.WriteLine("S");
                    w_file.WriteLine("Sp"); //Jump
                    w_file.WriteLine("LS"); //Action

                    w_file.WriteLine("DR");
                    w_file.WriteLine("DL");
                    w_file.WriteLine("DU");
                    w_file.WriteLine("DD");
                    w_file.WriteLine("A");
                    w_file.WriteLine("X");
                }

            }

            //Read from the options file
            using (StreamReader r_file = File.OpenText(path))
            {
                /*Order for reading is:
                Right key, left key, up key, down key, Jump key, Action key,
                right button, left button, up button, down button, jump button, action button
                */
                rKey = getKeyFromString(r_file.ReadLine());
                lKey = getKeyFromString(r_file.ReadLine());
                uKey = getKeyFromString(r_file.ReadLine());
                dKey = getKeyFromString(r_file.ReadLine());
                jKey = getKeyFromString(r_file.ReadLine());
                aKey = getKeyFromString(r_file.ReadLine());

                rBut = getButtonFromString(r_file.ReadLine());
                lBut = getButtonFromString(r_file.ReadLine());
                uBut = getButtonFromString(r_file.ReadLine());
                dBut = getButtonFromString(r_file.ReadLine());
                jBut = getButtonFromString(r_file.ReadLine());
                aBut = getButtonFromString(r_file.ReadLine());
            }
        }

        /// <summary>
        /// Read inputs from a controller or the keyboard
        /// </summary>
        public void readInputs()
        {
            //Read the inputs to start with
            KeyboardState ks = Keyboard.GetState(); //Read the keyboard state

            //Define the controls as booleans
            RIGHT = ks.IsKeyDown(rKey);
            LEFT = ks.IsKeyDown(lKey);
            UP = ks.IsKeyDown(uKey);
            DOWN = ks.IsKeyDown(dKey);
            JUMP = ks.IsKeyDown(jKey);
            ACTION = ks.IsKeyDown(aKey);
            PAUSE = ks.IsKeyDown(Keys.Enter);
            DEBUG = ks.IsKeyDown(Keys.B) && ks.IsKeyDown(Keys.S);

            //Check if controller inputs should be applied as well
            GamePadCapabilities caps = GamePad.GetCapabilities(Microsoft.Xna.Framework.PlayerIndex.One);
            if (caps.IsConnected)
            {
                GamePadState gpState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

                RIGHT = RIGHT || gpState.ThumbSticks.Left.X > dead_zone || gpState.IsButtonDown(rBut);
                LEFT = LEFT || gpState.ThumbSticks.Left.X < -1*dead_zone || gpState.IsButtonDown(lBut);
                UP = UP || gpState.ThumbSticks.Left.Y < -1 * dead_zone || gpState.IsButtonDown(uBut);
                DOWN = DOWN || gpState.ThumbSticks.Left.Y > dead_zone || gpState.IsButtonDown(dBut);
                JUMP = JUMP || gpState.IsButtonDown(jBut);
                ACTION = ACTION || gpState.IsButtonDown(aBut);
            }
        }
    }
}
