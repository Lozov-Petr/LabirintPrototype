using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace LabPrototype
{
    class Const
    {
        public static int SizeRoom = 20;
        public static int SizeLabirint = 3;
        public static int SizeTotal = SizeLabirint * SizeRoom;

        public static int SizeCell = 64;
        public static int HalfSizeCell = SizeCell / 2;

        public static int SizeViewX = 11;
        public static int SizeViewY = 7;
        public static int VirtualWidth = 640;
        public static int VirtualHeight = 360;

        public static float Epsilon = 0.2f;
    }

    class ConstCell
    {
        public static int frameStringCount = 2;
        public static int frameRowCount = 1;
        public static int framesPerSec = 0;
        public static Color color = Color.White;
    }
    class ConstCamera
    {
        public static int CountStep = 10;
    }
}

