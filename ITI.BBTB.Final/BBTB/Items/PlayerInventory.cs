using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
   public class PlayerInventory
    {
        List<Item> Inventory = new List<Item>();
        public string ItemName { get; set; }
        public Texture2D Texture { get; set; }
       
       
       public PlayerInventory()
        {

        }

        public PlayerInventory(string ItemName, Texture2D Texture)
        {
            this.ItemName = ItemName;
            this.Texture = Texture;
        }
        public void AddItemToInventory(Item Item)
        { 
            int i = Item.InventoryEmplacement;
            Inventory[i] = Item;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sb.Draw(this.Texture, position, Color.White);
        }

        public List<Item> InventoryList()
        {
            return Inventory;
        }
        public Texture2D FoundTextureByID(int ItemID, List<Texture2D> ItemTextures)
        {
            Texture2D itemTexture = ItemTextures.ElementAt(ItemID);
            return itemTexture;
        }
    }
}
