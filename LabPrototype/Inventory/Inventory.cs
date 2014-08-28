using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabPrototype
{
    class Inventory
    {
        List<Item> items = new List<Item>();

        public void Add(Item item)
        {
            items.Add(item);
        }
    }
}
