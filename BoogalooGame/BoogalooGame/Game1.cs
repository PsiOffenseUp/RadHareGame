using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BoogalooGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //New declarations
        SpriteFont font;
        Player player;
        Collision testCollision;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Added code
            player = new Player(0.0f, 0.0f);
            testCollision = new Collision(0.0f, 200.0f);

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

            //Added code
            font = Content.Load<SpriteFont>("fonts/Consolas"); //Takes argument of the SpriteFont file name
           
            player.Name = "Rad Hare";
            player.Load();
            player.loadSprite(this);

            testCollision.setPosition(0.0f, 360.0f);
            testCollision.Load();
            testCollision.setSprite("tiles/block-192");
            testCollision.loadSprite(this);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Added code
            player.readControls();
            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            //New code
            
            //Start drawing things
            spriteBatch.Begin();

            //Iterate and draw all active objects in GameObject.object_dict and all background objects DEBUG Add background objects later
            foreach (KeyValuePair<long, GameObject> entry in GameObject.ActiveObjects)
                spriteBatch.Draw(entry.Value.sprite.texture, entry.Value.position, Color.White);

            spriteBatch.DrawString(font, "xspeed: " + player.xspeed.ToString(),  new Vector2(player.position.X, player.position.Y - 40.0f), Color.Black);
            spriteBatch.DrawString(font, "yspeed: " + player.yspeed.ToString(), new Vector2(player.position.X, player.position.Y - 20.0f), Color.Black);
            spriteBatch.DrawString(font, "Jump pressed " + Player.controller.options.JUMP.ToString(),  new Vector2(10.0f, 100.0f), Color.Black);
            spriteBatch.DrawString(font, "Collison below?: " + player.collision_below.ToString(), new Vector2(10.0f, 150.0f), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
