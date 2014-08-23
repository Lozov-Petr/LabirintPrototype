using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class FirstEnemy : Enemy
    {
        public static Texture2D texture;

        public FirstEnemy(SpriteBatch spriteBatch, Vector2 position)
        {
            constants = ObjectConstans.GetPlayerConstans();
            sprite = new Sprite(texture, spriteBatch, constants.frameCount, constants.frameStringCount, constants.frameRowCount, constants.framesPerSec);
            Position = position;
        }
        
        public override void Draw(Vector2 screenVector)
        {
            int[] animation;

            if (CurrentDirection.Length() < Const.Epsilon) animation = constants.animations[Animations.STAY];
            else 
            {
                float absX = Math.Abs(CurrentDirection.X);
                float absY = Math.Abs(CurrentDirection.Y);
                if (absX > absY)
                {
                    if (CurrentDirection.X > 0) animation = constants.animations[Animations.RIGHT];
                    else animation = constants.animations[Animations.LEFT];
                }
                else
                {
                    if (CurrentDirection.Y > 0) animation = constants.animations[Animations.DOWN];
                    else animation = constants.animations[Animations.UP];
                }
 
            }
            /////
            //Primitives.AddRectangle(new Rectangle((int)(Position.X - GetShiftX() - screenVector.X), (int)(Position.Y - GetShiftY() - screenVector.Y), 64, 64), Color.Red);
            /////

            float scale = (float)constants.collisionRadius / 8f;
            sprite.DrawAnimation(animation, Position - screenVector, constants.color, constants.shiftForDrawing, ScaleX: scale, ScaleY: scale, Depth: (float)Position.Y / (float)(Const.SizeCell * Const.SizeTotal));
        }

        public override void Update(Labirint labirint, GameTime gameTime)
        {
            NewDirection = labirint.player.Position - Position;
            if (NewDirection != Vector2.Zero) NewDirection.Normalize();

            base.Update(labirint, gameTime);
        }

    }
}
