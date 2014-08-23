using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    enum CellType   { FLOOR, WALL }
    
    class Cell
    {
        public static Texture2D texture;
        Sprite spriteWithBlocks;


        public List<DrawingObject> objects = new List<DrawingObject>();

        public Vector2 angleLU;
        public Vector2 angleRU;
        public Vector2 angleRD;
        public Vector2 angleLD;
        
        public CellType Type;

        public Cell(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteWithBlocks = new Sprite(texture, spriteBatch, ConstCell.frameCount, ConstCell.frameStringCount, ConstCell.frameRowCount, ConstCell.framesPerSec);
            
            angleLU = position;
            angleRU = position;
            angleRD = position;
            angleLD = position;

            angleRU.X += Const.SizeCell;
            angleLD.Y += Const.SizeCell;
            angleRD.Y += Const.SizeCell;
            angleRD.X += Const.SizeCell;
        }

        public bool IsWall()
        {
            return Type == CellType.WALL;
        }

        public void Draw(float posX, float posY)
        {
            int indexFrame = (int)Type;
            Vector2 screenPosition = new Vector2(posX, posY);
            spriteWithBlocks.Draw(indexFrame, screenPosition, ConstCell.color, Vector2.Zero, Depth: 0);
        }

        public void Update(GameTime gameTime)
        {
            //TODO
        }
    }
}
