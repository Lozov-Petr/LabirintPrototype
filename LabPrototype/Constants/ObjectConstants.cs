using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabPrototype
{
    class ObjectConstans
    {
        public int frameStringCount;
        public int frameRowCount;
        public int framesPerSec;
        public Color color;
        public Vector2 shiftForDrawing;
        public int collisionRadius;
        public Dictionary<Animations, int[]> animations;

        public static ObjectConstans PlayerConstans = GetPlayerConstans();

        /**/public/**/ static ObjectConstans GetPlayerConstans()
        {
            ObjectConstans objConst = new ObjectConstans();

            objConst.frameStringCount = 4;
            objConst.frameRowCount = 4;
            objConst.framesPerSec = 5;
            objConst.color = Color.White;
            objConst.shiftForDrawing = new Vector2(31, 63);
            objConst.collisionRadius = 8;

            Dictionary<Animations, int[]> dictionary = new Dictionary<Animations, int[]>();

            dictionary[Animations.STAY] = new int[1] { 8 };
            dictionary[Animations.UP] = new int[4] { 0, 1, 2, 3 };
            dictionary[Animations.RIGHT] = new int[4] { 4, 5, 6, 7 };
            dictionary[Animations.DOWN] = new int[4] { 8, 9, 10, 11 };
            dictionary[Animations.LEFT] = new int[4] { 12, 13, 14, 15 };

            objConst.animations = dictionary;

            return objConst;
        }
    }
}
