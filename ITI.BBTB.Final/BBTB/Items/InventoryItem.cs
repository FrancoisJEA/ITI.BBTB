using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
   public class InventoryItem
    {
        string[] Inventory = new string[8];
        public string ItemName { get; set; }
        public Texture2D Texture { get; set; }
        private int MaxQuantity = 1;
        string[,] ItemList;
        WorldItem WorldItem = new WorldItem();

        public InventoryItem()
        {

        }

        public InventoryItem(string ItemName, Texture2D Texture)
        {
            this.ItemName = ItemName;
            this.Texture = Texture;
        }
        public string[] AddItemToInventory(int i, int j)
        {

            ItemList = WorldItem.AddItems();
            Inventory[i] = ItemList[i,j];
            return Inventory;
            
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sb.Draw(this.Texture, position, Color.White);
        }

        public string[] InventoryList()
        {
            return Inventory;
        }
    }
}
