using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
    public class Item
    {


        public Vector2 _position;
        public Texture2D _texture;
        public SpriteBatch SpriteBatch;
        public string Name { get; }
        public string ItemType { get; set; }
        
        //public enum category
        public string ItemClasse { get; set; }
        public int Attack { get; set; }
        private Player _player;


        public int InventoryEmplacement { get; }



        public Item(Vector2 position, Texture2D texture, SpriteBatch spriteBatch,Player player)
             
        {
            _position = position;
            _texture = texture;
            SpriteBatch = spriteBatch;
            Name = texture.Name.Replace("_"," ");
            Name = Name.Replace("Items/","");
          
            InventoryEmplacement = this.FindInventoryEmplacement(Name);
            _player = player;
            ItemClasse = FindClasse();
            if (ItemType == "weapon")
                Attack = 20 + Convert.ToInt32(_player._playerM.Level * 1.8);
        }

        private string FindClasse()
        {
            if(Name.Contains("bow") || (Name.Contains("crossbow"))) return "Archer";
            if(Name.Contains("gun") || (Name.Contains("rifle"))) return "Gunner";
            if (Name.Contains("staff") || (Name.Contains("stick"))) return "Wizard";
            else return "All";
        }

        public void DefineItem ()
        {
            
        }

        public void AddToInventory(string Name)
        {
         
        }
        internal int FindInventoryEmplacement (string Name)
        {

            if (Name.Contains("helmet")) {  ItemType = "armor"; return 0; } //this.category 
            if (Name.Contains("armor")) {  this.ItemType = "armor"; return 1; }
            if (Name.Contains("boots")) {  this.ItemType = "armor"; return 3; }
            if (Name.Contains("gloves")) {  this.ItemType = "armor"; return 2; }
            if (Name.Contains("bow") || Name.Contains("staff") || Name.Contains("gun")) {  ItemType = "weapon"; return 4; }
            if (Name.Contains("crossbow") || Name.Contains("stick") || Name.Contains("sword")) {  ItemType = "weapon"; return 5; }

            else return 6;

        }

        public void Draw ()
        {
            SpriteBatch.Draw(_texture, _position, Color.White);
        }

        public Texture2D FoundTextureByID(int ItemID, List<Texture2D> ItemTextures)
        {
            Texture2D itemTexture = ItemTextures.ElementAt(ItemID);
            return itemTexture;
        }

    }
}
