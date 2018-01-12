using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
    public class WorldItem
    {
        private string ItemName;

        public WorldItem()
        {
        }
        

        public string[,] AddItems() {

            string[,] Item = new string[8,2];
            Item[0,0] = "Steel helmet";
            Item[0,1] = "Leather helmet";
            Item[1,0] = "Steel plastron";
            Item[1,1] = "Leather plastron";
            Item[2,0] = "Steel greaves";
            Item[2,1] = "Leather greaves";
            Item[3,0] = "Steel boots";
            Item[3,1] = "Leather boots";
            Item[4,0] = "Crossbow";
            Item[4,1] = "Bow";
            Item[5,0] = "Magic Staff";
            Item[5,1] = "Fire Wand";
            Item[6,0] = "Revolver";
            Item[6,1] = "Machine Gun";

            return Item;

        }

            // Items Key

                    // [0] = Helmet
                    // [1] = Plastron
                    // [2] = Greaves
                    // [3] = Boots

            // Weapons Key

                    // [4] = Archer
                    // [5] = Wizard
                    // [6] = Marksman

            

    }
}
