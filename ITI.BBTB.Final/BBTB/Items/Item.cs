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
        //public enum Type
        //public enum category
        int Attack { get; }


        public int InventoryEmplacement { get; }



        public Item(Vector2 position, Texture2D texture, SpriteBatch spriteBatch)
             
        {
            _position = position;
            _texture = texture;
            SpriteBatch = spriteBatch;
            Name = texture.Name;
            InventoryEmplacement = this.FindInventoryEmplacement(Name);
            
            //this.Attack = Convert.ToInt32(player.Level * 1.8);

        }

        public void AddToInventory(string Name)
        {
         
        }
        internal int FindInventoryEmplacement (string Name)
        {

            if (Name.Contains("helmet")) { return 0; } //this.category 
            if (Name.Contains("armor")) { return 1; }
            if (Name.Contains("boots")) { return 2; }
            if (Name.Contains("greaves")) { return 3; }
            if (Name.Contains("bow") || Name.Contains("staff") ||Name.Contains("Gun")) { return 4; }
            else return 5;

        }
         public void Draw ()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_texture, _position, Color.White);
            SpriteBatch.End();
        }

        
     
    }
}
