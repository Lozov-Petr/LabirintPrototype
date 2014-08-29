using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabPrototype
{
    abstract class Ingredient : Item
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
