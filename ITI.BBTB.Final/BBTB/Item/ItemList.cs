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
            Items.add (0,"Steel helmet"),
            Items.add (0,"Leather helmet"),
            Items.add (1,"Steel plastron"),
            Items.add (1,"Leather plastron"),
            Items.add (2,"Steel greaves"),
            Items.add (2,"Leather greaves"),
            Items.add (3,"Steel boots"),
            Items.add (3,"Leather boots"),
            Items.add (4,"Leather helmet"),
            Items.add (4,"Leather helmet"),



        }


    }
}


new Item { Tags = new[0] { "Steel helmet", "Leather helmet" }},
    new Item { Tags = new[1] { "Steel plastron", "Leather armor" }},
    new Item { Tags = new[2] { "Steel greaves", "Leather greaves" }},
    new Item { Tags = new[3] { "Steel Boots", "Leather boots" }},
    new Item { Tags = new[4] { "Crossbow1", "Bow1" }},
    new Item { Tags = new[5] { "Magic Wand1", "Fire Staff1" }},
    new Item { Tags = new[5] { "Pistol1", "Machine gun1" }}