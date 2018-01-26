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
        public int _potionNb;
        private int _potionMax = 5;

        public PlayerInventory(List<Texture2D> AllTexture, SpriteBatch spriteBatch, Texture2D boxTexture, Texture2D boxTexture2 )
        {
            allTexture = AllTexture;
            sb = spriteBatch;
            this.InvTexture = allTexture[0];
            BoxTexture = boxTexture;
            BoxTexture2 = boxTexture2;
            _potionNb = 2;
        }

        public void ItemByDefault (Player _player)
        {
            List<Item> DefaultInventory = new List<Item>();
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[9], sb,_player)); //Helmet
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[6], sb, _player)); //Armor
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[8], sb, _player)); // gloves
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[7], sb, _player));  //Boots
            if (_player.PlayerClasse == "Wizard")
            {
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[11], sb, _player)); // Weapon2
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[15], sb, _player)); // Weapon1
            }
            else if (_player.PlayerClasse == "Gunner")
            {
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[16], sb, _player)); // Weapon2
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[10], sb, _player)); // Weapon1
            }
            else if (_player.PlayerClasse == "Archer")
            {
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[11], sb, _player)); // Weapon2
                DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[10], sb, _player)); // Weapon1
            }
            DefaultInventory.Add(new Item(new Vector2(80, 80), allTexture[12], sb, _player)); //
            //DefaultInventory.Add(new Item(new Vector2(80, 80), AllTexture[6], SpriteBatch));
            //DefaultInventory.Add(new Item(new Vector2(80, 80), AllTexture[7], SpriteBatch));
            Inventory = DefaultInventory;
        }
        public void AddEmptyPotion(Player _player)
        {
            Item i = new Item(new Vector2(80,80), allTexture[13], sb, _player);
            Inventory.RemoveAt(6);
            Inventory.Insert(6, i);
        }
        public List<Item> AddItemToInventory(Item Item, List<Item> Items,Player _player,int y)
        {
            int i = Item.InventoryEmplacement;
            if (Item.Name == "Heal potion")
            {
                
                if (_potionNb < _potionMax)
                {
                    _potionNb++;
                    Inventory[i]._position.X = Item._position.X;
                    Inventory[i]._position.Y = Item._position.Y;
                    Inventory.RemoveAt(i);
                    Inventory.Insert(i, Item);
                    Items.Remove(Item);
                }

               
            }
            else if (Item.Name == "Used potion")
            {
                _player._playerM.Money += 20;
                Items.Remove(Item);
            }
            else 
            {

                Inventory[i]._position.X = Item._position.X;
                Inventory[i]._position.Y = Item._position.Y;
                UpdatePlayerSats(Inventory[i], Item, _player);
                Items.Add(Inventory[i]);
                Inventory.RemoveAt(i);
                Inventory.Insert(i, Item);
                Items.Remove(Item);

            }
            return Items;
        }

        private void UpdatePlayerSats(Item OldItem, Item NewItem, Player player)
        {
            PlayerModel p = player._playerM;
            p.Strength -= OldItem._strength;
            p.Intelligence -= OldItem._intelligence;
            p.Agility -= OldItem._agility;
            p.Strength += NewItem._strength;
            p.Intelligence += NewItem._intelligence;
            p.Agility += NewItem._agility;
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

        public void ShowInventory (Player player, SpriteFont _debugFont)
        {
            string Text;
            KeyboardState keyboardState = Keyboard.GetState();
            int x = 550;
            int y = 200;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                this.Draw(sb, new Vector2(350,200));
                Text = string.Format("{0:0}\r\n Level = {4:0} \r\n\r\n Skills \r\n Strength ={1:0}\r\n Intelligence ={2:0}\r\n Agility ={3:0} \r\n", player._playerM.Name, player._playerM.Strength, player._playerM.Intelligence, player._playerM.Agility, player._playerM.Level);
                DrawWithShadow(Text, new Vector2(360,200),_debugFont);
                foreach (Item i in Inventory)
                {
                    i._position.X = x;
                    i._position.Y = y;
                    i.Draw();
                    y += 64;
                }
            }
        }
        private void DrawWithShadow(string text, Vector2 position,SpriteFont _debugFont)
        {
            sb.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            sb.DrawString(_debugFont, text, position, Color.LightYellow);
        }
    }
}
