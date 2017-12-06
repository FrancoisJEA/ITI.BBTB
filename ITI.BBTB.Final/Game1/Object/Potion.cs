using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Object
{
    public class Potion : ObtainableItem
    {
        public Potion()
        {
            MaximumStackableQuantity = 5;
        }
    }
}