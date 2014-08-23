using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabPrototype
{
    abstract class ObjectWithPosition
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 CurrentDirection = Vector2.Zero;
        public Vector2 NewDirection = Vector2.Zero;
        public int Speed = 1;
        public int slide = 1;

        public int GetCellX()
        {
            return (int)Position.X / Const.SizeCell;
        }

        public int GetCellY()
        {
            return (int)Position.Y / Const.SizeCell;
        }

        public int GetShiftX()
        {
            return (int)Position.X % Const.SizeCell;
        }

        public int GetShiftY()
        {
            return (int)Position.Y % Const.SizeCell;
        }
    }
}
