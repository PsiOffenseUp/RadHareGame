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
    public class Map
    {
        public List<BackGround> backGrounds;
        public Vector2 MapSize;
        public Tile[,] mapData;

        public Map(int width, int height)
        {
            backGrounds = new List<BackGround>();

            this.MapSize.X = width; this.MapSize.Y = height;
            mapData = new Tile[width, height];

            for(int i = 0; i < MapSize.X; i++)
            {
                for(int j = 0; j < MapSize.Y; j++)
                {
                    if (j % 2 == 0 && i % 2 == 0)
                        mapData[i, j] = new Tile(1);
                    else mapData[i, j] = new Tile();
                }
            }
        }

        public void Update(Vector2 v)
        {
            foreach(BackGround b in backGrounds)
            {
                b.Update(v);
            };
        }

        public static Map LoadMap()
        {
            //Future - reads in map from map file
            return new Map(10, 10);
        }
    }

    public class BackGround
    {
        private Game1 game;
        public Texture2D Image;
        public Vector2 position, offset;
        public float Distance; //Determines how fast the image scrolls with player movement
        public bool HorizontalRepeat = true;
        public bool VerticalRepeat = true;
        //Constraints to prevent movement along a particular axis.
        public bool X_Constrained = false;
        public bool Y_Constrained = false;
        
        public BackGround(Game1 game)
        {
            this.game = game;
        }

        public void Update(Vector2 v)
        {
            Distance = 0.1f;
            Vector2 move = (-v * (1.0f / Distance));
            if (X_Constrained) move.Y = 0.0f; if (Y_Constrained) move.X = 0.0f; //constraining the movement
            position += move;
        }

        public void Draw(SpriteBatch sb)
        {
            int tx = (int)position.X / Image.Width;
            int ty = (int)position.Y / Image.Height;

            Vector2 offset = GameManager.camera.Position - position;

            sb.Begin();
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    int posX = (int)(position.X + (i * Image.Width));
                    int posY = (int)(position.Y + (j * Image.Height));
                    Console.WriteLine(new Vector2(posX, posY));
                }
            }
            sb.End();
        }
    }

    public class Tile
    {
        public int ImgID;
        public bool Collidable;

        public Tile()
        {
            ImgID = 0; Collidable = false;
        }
        public Tile(int ID)
        {
            ImgID = ID; Collidable = false;
        }
    }
}
