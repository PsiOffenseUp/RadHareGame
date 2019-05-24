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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
            uiObjects = new List<GameObject>();
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            serviceManager = new ServiceManager(this);

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

            gameObjects.Add(obj);
            gameObjects.Add(obj2);

            obj.SetChild(obj2);

            //Button Testing
            Texture2D[] buttonSprites = { Content.Load<Texture2D>("Test_Button_Unpressed"), Content.Load<Texture2D>("Test_Button_Pressed") };
            ButtonClicked btn = new ButtonClicked(Action);
            Button button = new Button(buttonSprites, new Vector2(0, 0));
            button.GetComponent<ButtonController>().buttonClicked += Action;
            uiObjects.Add(button);
            ////////////////

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

            base.Draw(gameTime);
        }
    }
}
