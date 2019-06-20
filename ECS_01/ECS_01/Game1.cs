using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ECS_01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /*****BETA*****/
        ServiceManager serviceManager;
        /**************/
        public List<GameObject> gameObjects;
        public List<GameObject> uiObjects;

        //DEBUG Testing collision, delete later
        GameObject object1;
        GameObject floor;

        //Testing maps
        MapManager mapManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
            uiObjects = new List<GameObject>();
            GameObject.game = this; //Set the static reference to the game to be this game
            ServiceManager.game = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            //DEBUG Map stuff
            mapManager = new MapManager(this, "maps/test.tmx"); //Load the first map
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            serviceManager = new ServiceManager();

            // TODO: use this.Content to load your game content 
            
            GameObject obj = new GameObject();
            GameObject obj2 = new GameObject();
            
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite.Image = Content.Load<Texture2D>("Test_CobbleStoneTile");
            obj.GetComponent<SpriteRenderer>().sprite.Scale = new Vector2(3.0f, 3.0f); //scales the sprite
            obj.transform.Scale(new Vector2(3.0f, 3.0f)); //scales the Object
            obj2.transform.Scale(new Vector2(5.0f, 5.0f));
            obj.transform.Rotate(45.0f, AngleType.DEGREES);
            obj.AddComponent<InputController>();
            obj.AddComponent<Camera>();

            obj2.AddComponent<SpriteRenderer>();
            obj2.GetComponent<SpriteRenderer>().sprite.Image = Content.Load<Texture2D>("Test_CobbleStoneTile");

            obj.Load();
            obj2.Load();

            //obj.SetChild(obj2);

            //Button Testing
            //Texture2D[] buttonSprites = { Content.Load<Texture2D>("Test_Button_Unpressed"), Content.Load<Texture2D>("Test_Button_Pressed") };
            //ButtonClicked btn = new ButtonClicked(Action);
            //Button button = new Button(buttonSprites, new Vector2(0, 0));
            //button.GetComponent<ButtonController>().buttonClicked += Action;
            //uiObjects.Add(button);
            ////////////////
            ///
            

            //*****DEBUG: Collision Testing*****
            
            floor = new GameObject();
            object1 = new GameObject();

            Vector2[] floorVertices = new Vector2[3];
            Vector2[] objectVertices = new Vector2[4];

            floorVertices[0] = new Vector2(0, 0);
            floorVertices[1] = new Vector2(15, 0);
            floorVertices[2] = new Vector2(15, 15);
            //floorVertices[3] = new Vector2(0, 15);

            objectVertices[0] = new Vector2(0, 0);
            objectVertices[1] = new Vector2(4, 0);
            objectVertices[2] = new Vector2(4, 4);
            objectVertices[3] = new Vector2(0, 4);

            object1.Components.Add(new Hitbox(objectVertices, object1));
            object1.Components.Add(new CollisionComponent(object1));
            //object1.AddComponent<SpriteRenderer>();
            object1.transform.SetPosition(new Vector2(0.0f, 0.0f));

            floor.Components.Add(new Hitbox(floorVertices, floor));
            floor.Components.Add(new CollisionComponent(floor, true));
            floor.AddComponent<SpriteRenderer>();
            floor.transform.SetPosition(new Vector2(2.0f, 2.0f));

            object1.Load();
            floor.Load();

            //******************End collision testing*****************

            //**************Map testing****************
            Extensions.AddService<CameraManager>(serviceManager, this); //Add a camera manager, so that we can get its view matrix later

            //********************End******************

            serviceManager.GetService<CameraManager>().SetMainCamera(obj.GetComponent<Camera>());
        }

        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }

        public void Action()
        {
            Console.WriteLine("Button Clicked!");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            for(int i = 0; i < gameObjects.Count; i++)
            {
                for(int j = 0; j < gameObjects[i].Components.Count; j++)
                {
                    gameObjects[i].Components[j].Update(gameTime);
                }
            }

            foreach(GameObject obj in uiObjects)
            {
                foreach(Component c in obj.Components)
                {
                    c.Update(gameTime);
                }
            }

            //DEBUG Map stuff
            mapManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            serviceManager.Update();

            //DEBUG Map Stuff
            mapManager.drawMap(this);

            base.Draw(gameTime);
        }
    }
}
