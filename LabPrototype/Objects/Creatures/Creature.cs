using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabPrototype
{ 
    abstract class Creature : DrawingObject
    {
        public int Health;
        public int Armor;
        public int Stealth;
        public int SpeedOfAttack;
        public int StrengthOfAttack;
        public int SpeedOfShut;
        public int RangeOfShut;
        public int StrengthOfShut;
    }
}
