using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using BoogalooGame.Imports;

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
        //Collision[] testCollisions;
        Sprite hitboxSprite;
        Level testLevel;
        TiledMap map;
        TiledMapRenderer tmr;
        Camera2D camera; //Need a camera class
        SamplerState samplerstate;

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
            player = new Player("test/Luigi");
            map = Level.loadLevel(this, "levels/test");
            tmr = new TiledMapRenderer(GraphicsDevice);
            camera = new Camera2D();

            /*
            testCollisions = new Collision[20];
            for (int i = 0; i < 20; i++)
                testCollisions[i] = new Collision();
                */

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

            hitboxSprite = new Sprite(this, "test/hitbox");

            player.Name = "Rad Hare";
            player.Load(this);

            /*for (int i = 0; i < 10; i++)
                testCollisions[i].setPosition(32.0f * i, 200.0f);
            for(int i = 10; i < 20; i++)
                testCollisions[i].setPosition(32.0f * i, 260.0f);

            for (int i = 0; i < 20; i++)
                testCollisions[i].Load(this);*/
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

            tmr.Update(map, gameTime);

            camera.setCameraPosition(player.position);

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

            tmr.Draw(map, viewMatrix: camera.getTransformation, SamplerState.PointClamp);

            //Iterate and draw all active objects in GameObject.object_dict and all background objects DEBUG Add background objects later
            foreach (KeyValuePair<long, GameObject> entry in GameObject.ActiveObjects)
            {
                spriteBatch.Draw(entry.Value.sprite.texture, entry.Value.position, Color.White);
                if (Options.debug) //Draw hitboxes and debug info if debug mode is turned on
                {
                    spriteBatch.Draw(hitboxSprite.texture, entry.Value.Hitbox, entry.Value.hitboxColor * 0.35f);
                    spriteBatch.DrawString(font, "xspeed: " + player.xspeed.ToString(), new Vector2(60.0f, 0.0f), Color.Black);
                    spriteBatch.DrawString(font, "yspeed: " + player.yspeed.ToString(), new Vector2(60.0f, 20.0f), Color.Black);
                    spriteBatch.DrawString(font, "x: " + player.position.X.ToString(), new Vector2(0.0f, 0.0f), Color.White);
                    spriteBatch.DrawString(font, "y: " + player.position.Y.ToString(), new Vector2(0.0f, 20.0f), Color.White);

                }
            }

            spriteBatch.DrawString(font, "Collison below?: " + player.collision_below.ToString(), new Vector2(10.0f, 40.0f), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
