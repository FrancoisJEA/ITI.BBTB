using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BBTB.Items;

namespace BBTB.Enemies
{
    class Trader : Sprite
    {
        int _money;         
        int _itemNb = 3;        // Nb of items sold by the Trader
        public List<Item> Inventory;
        List<Texture2D> _allTexture;

        public Trader(Texture2D texture, Vector2 position, SpriteBatch batch, List<Texture2D> AllTextureItems) : base(texture, position, batch)
        {
            _allTexture = AllTextureItems;
            Inventory = new List<Item>();
            GenerateItems();
        }

        public void GenerateItems()
        {
            Random rng = new Random();
            for (int i = 0; i < _itemNb; i++)
            {
                int ItemNb = _allTexture.Count - 1;
                int ItemID = rng.Next(1, ItemNb);

                Texture2D ItemTexture = Board.CurrentBoard._player.Inventory.FoundTextureByID(ItemID,_allTexture);
                float X = rng.Next(Convert.ToInt32(Position.X - 128),Convert.ToInt32(Position.X + 128));
                float Y = rng.Next(Convert.ToInt32(Position.Y - 128), Convert.ToInt32(Position.Y + 128));
                Inventory.Add(new Item(new Vector2(X,Y), ItemTexture, SpriteBatch, Board.CurrentBoard._player));
            }

        }
        public List<Item> ItemsToSell(List<Item> i)
        {
            foreach (Item item in Inventory)
            {
                i.Add(item);
            }
            return i;
        }

        public void DefinePrices ()
        {
            foreach (Item i in Inventory)
            {
                
            }

        }
         ~Trader () { }
        

    }
}
