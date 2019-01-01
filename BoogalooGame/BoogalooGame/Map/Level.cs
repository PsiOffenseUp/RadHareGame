using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MonoGame.Extended.Tiled;

//using System.Xml;

namespace BoogalooGame
{
    class Level
    {
        Layer layer; //Holds all the enemies, background objects, and collision
        private long width, height; //Length of width of the map
        Player player;
        static int id;
        const int number_of_levels = 10; //Number of levels in the game

        static string[] level_paths;

        //------------------Constructors-----------------
        //Static constructor 
        static Level()
        {
            level_paths = new string[number_of_levels]; //DEBUG Change based on number of level
            level_paths[0] = "level1.1";
            level_paths[1] = "level1.2";
        }

        //Default constructor

        public Level()
        {
            player = new Player();
            id = 0;
        }

        //--------------------Methods---------------------
        /// <summary>
        /// Loads the level into game from the file name level_path. This should then be followed by a creation
        /// of a TiledMapRenderer with the graphics Device as an argument to the constructor. This function also returns the 
        /// newly created map
        /// </summary>
        /// <param name="game"></param>
        /// <param name="level_path"></param>
        /// <returns></returns>
        static public TiledMap loadLevel(Game1 game, string level_path)
        {

            return game.Content.Load<TiledMap>(level_path);

        }

        static public TiledMap loadNextLevel(Game1 game)
        {
            if (id >= number_of_levels - 1) //Bail out so as not to seg-fault when loading on the last level
                return null;
            id++;
            return loadLevel(game, level_paths[id]);
        }

        public void readLevelData(string level_path)
        {


            /*using (StreamReader readFile = new StreamReader(level_path))
            {
                string jsonFile = readFile.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(jsonFile);

                foreach (var item in array)
                {
                    Console.WriteLine(item);
                }
            }*/
        }
    }
}
