using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BoogalooGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Collision testCollision;

        //New declarations
        SpriteFont font;
        Player player;

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
            player.Update(gameTime);
            player.readControls();

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
            //Iterate and draw all things in GameObject.object_dict and all background objects
            spriteBatch.Draw(player.sprite.texture, player.position, Color.White);
            spriteBatch.Draw(testCollision.sprite.texture, testCollision.position, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
