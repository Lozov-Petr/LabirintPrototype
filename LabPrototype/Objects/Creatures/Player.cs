﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class Player : Creature
    {
        public static Texture2D texture;

        public Inventory inventory;

        public Player(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            constants = ObjectConstans.PlayerConstans;
            sprite = new Sprite(texture, spriteBatch, constants.frameStringCount, constants.frameRowCount, constants.framesPerSec);
            Position = playerPosition;
            inventory = new Inventory();
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
            sprite.Draw(animation, Position - screenVector, constants.color, constants.shiftForDrawing, Depth: (float)Position.Y / (float)(Const.SizeCell * Const.SizeTotal));
        }

        public override void Update(Labirint labirint)
        {

            base.Update(labirint);
        }
    }
}
