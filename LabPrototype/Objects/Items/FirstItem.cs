using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class FirstItem : Item
    {
        public static Texture2D texture;

        public FirstItem(SpriteBatch spriteBatch, Vector2 position)
        {
            constants = ObjectConstans.PlayerConstans;
            Position = position;
            sprite = new Sprite(texture, spriteBatch);
        }

        public override void Draw(Vector2 screenVector)
        {
            sprite.Draw(0, Position - screenVector, constants.color, constants.shiftForDrawing, Depth: (float)Position.Y / (float)(Const.SizeCell * Const.SizeTotal));
        }

        public override void Update(Labirint labirint, GameTime gameTime)
        {
            
        }
    }
}
