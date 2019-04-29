using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RadHareEngine_v1.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    public class GameManager
    {
        private Game1 game;
        public static Dictionary<int, Texture2D> TileSprites;
        public List<Map> Maps;
        public static Map currentMap;
        public static Camera camera;
        public static InputManager inputManager;

        public GameManager(Game1 game)
        {
            TileSprites = new Dictionary<int, Texture2D>();
            Maps = new List<Map>();
            this.game = game;

            InitTileSprites();
            InitMaps();

            currentMap = Maps[0];
            
            camera = new Camera(game.GraphicsDevice.Viewport);

            inputManager = new InputManager();
        }

        private void InitTileSprites()
        {
            //Need to add spritesheet support that then feeds the data into the TileSprites Dictionary
            TileSprites.Add(0, game.Content.Load<Texture2D>("Block"));
            TileSprites.Add(1, game.Content.Load<Texture2D>("Blue"));
            TileSprites.Add(100, game.Content.Load<Texture2D>("Test_Background_1"));
        }

        private void InitMaps()
        {
            Map map = new Map(15, 15);
            BackGround bg = new BackGround(game);

            bg.Image = TileSprites[100];

            map.backGrounds.Add(bg);

            Maps.Add(map);
        }

        public void Update()
        {
            inputManager.Update();
            Vector2 move = Input();
            camera.MoveCamera(move);
            currentMap.Update(move);
            camera.UpdateCamera(game.GraphicsDevice.Viewport);
        }

        public Vector2 Input()
        {
            Vector2 move = new Vector2();

            if (InputManager.Keys[0]) move.Y += 0.1f;
            if (InputManager.Keys[1]) move.X += 0.1f;
            if (InputManager.Keys[2]) move.Y -= 0.1f;
            if (InputManager.Keys[3]) move.X -= 0.1f;

            return move;
        }

        public static void DrawMap(SpriteBatch sb, Game1 game)
        {

            int tx = (int)Math.Floor(camera.Position.X); int ty = (int)Math.Floor(camera.Position.Y);
            Vector2 offset = new Vector2(tx, ty) - camera.Position;

            //BackGrounds
            currentMap.backGrounds[0].Draw(sb);

            //ForeGround
            sb.Begin();
            for (int i = 0; i <= camera.Bounds.Bottom / 32; i++)
            {
                for(int j = 0; j <= camera.Bounds.Right / 32; j++)
                {
                    if (tx + j < currentMap.MapSize.X && ty + i < currentMap.MapSize.Y && tx + j >= 0 && ty + i >= 0)
                    {
                        sb.Draw(TileSprites[currentMap.mapData[tx + j, ty + i].ImgID], ((new Vector2((tx + j), (ty + i)) - camera.Position) * 32) + offset);
                    }
                }
            }
            sb.End();
        }

    }
}
