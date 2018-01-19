using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public List<Texture2D> allTexture;
        public Texture2D InvTexture;
        SpriteBatch sb;
        public Texture2D BoxTexture { get; set; }
        public Texture2D BoxTexture2 { get; set; }



        public PlayerInventory(List<Texture2D> AllTexture, SpriteBatch spriteBatch, Texture2D boxTexture, Texture2D boxTexture2 )
        {
            allTexture = AllTexture;
            sb = spriteBatch;
            this.InvTexture = allTexture[0];
            BoxTexture = boxTexture;
            BoxTexture2 = boxTexture2;
        }

        public void ItemByDefault (Player _player)
        {
            List<Item> DefaultInventory = new List<Item>();
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[9], sb,_player)); //Helmet
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[6], sb, _player)); //Armor
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[8], sb, _player)); // gloves
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[7], sb, _player));  //Boots
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[11], sb, _player)); // Weapon2
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[10], sb, _player)); // Weapon1
            //DefaultInventory.Add(new Item(new Vector2(80, 80), AllTexture[5], SpriteBatch));
            //DefaultInventory.Add(new Item(new Vector2(80, 80), AllTexture[6], SpriteBatch));
            //DefaultInventory.Add(new Item(new Vector2(80, 80), AllTexture[7], SpriteBatch));
            Inventory = DefaultInventory;
        }
        public List<Item> AddItemToInventory(Item Item, List<Item> Items,Player _player)
        {   
                int i = Item.InventoryEmplacement;
                Inventory.RemoveAt(i);
                Inventory.Insert(i, Item);
                Items.Remove(Item);
                return Items;

        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
          
            sb.Draw(this.InvTexture, position, Color.White);
            
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

        public void ShowInventory ()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            int x = 550;
            int y = 200;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                this.Draw(sb, new Vector2(350,200));

                foreach (Item i in Inventory)
                {
                    i._position.X = x;
                    i._position.Y = y;
                    i.Draw();
                    y += 64;
                }
            }
        }
    }
}
