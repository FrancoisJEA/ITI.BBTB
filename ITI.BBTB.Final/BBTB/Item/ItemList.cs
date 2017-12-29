using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
   public class ItemList
   {
        var Items = new Dictionnary<item>
        {
            // Items ID

                    // [0] = Helmet
                    // [1] = Plastron
                    // [2] = Greaves
                    // [3] = Boots

            // Weapons ID

                    // [4] = Archer
                    // [5] = Wizard
                    // [6] = Marksman

            Items.add (0,"Steel helmet"),
            Items.add (0,"Leather helmet"),
            Items.add (1,"Steel plastron"),
            Items.add (1,"Leather plastron"),
            Items.add (2,"Steel greaves"),
            Items.add (2,"Leather greaves"),
            Items.add (3,"Steel boots"),
            Items.add (3,"Leather boots"),
            Items.add (4,"Crossbow1"),
            Items.add (4,"Bow 1"),
            Items.add (5,"Magic Wand"),
            Items.add (5,"Fire Staff"),
            Items.add (6,"Pistol1"),
            Items.add (6,"Machine gun1")


        };
  
   }
}