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
        Services services;
        public List<GameObject> gameObjects;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
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
            
            // TODO: use this.Content to load your game content 
            GameObject obj = new GameObject();
            GameObject obj2 = new GameObject();
            //obj.Components.Add(new SpriteRenderer(Content.Load<Texture2D>("Test_CobbleStoneTile")));
            
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

            services = new Services(spriteBatch, this);
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

            // TODO: Add your update logic here
            //gameObjects[0].transform.Translate(new Vector2(0.5f, 0.1f));
            //gameObjects[0].transform.Rotate(0.05f);

            for(int i = 0; i < gameObjects.Count; i++)
            {
                for(int j = 0; j < gameObjects[i].Components.Count; j++)
                {
                    gameObjects[i].Components[j].Update(gameTime);
                }
            }

            //gameObjects[0].transform.Translate(gameObjects[0].transform.Backward());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            services.Routing(gameObjects);

            base.Draw(gameTime);
        }
    }
}
