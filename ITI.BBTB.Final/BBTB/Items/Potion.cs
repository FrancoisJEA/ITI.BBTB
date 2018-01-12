using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Item
{
    public class Potion : InventoryItem
    {
        public Potion()
        {
            MaximumStackableQuantity = 5;
        }
    }
}