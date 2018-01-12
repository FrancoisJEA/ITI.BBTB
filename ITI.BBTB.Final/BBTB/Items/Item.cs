using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
    public class Item : Sprite
    {


        public Vector2 _position;
        public Texture2D _texture;
        public SpriteBatch _spriteBatch;
        public string Name { get; set; }
        public Guid ID { get; set; }


        public Item(Vector2 position, Texture2D texture, SpriteBatch spriteBatch)
             : base(texture, position, spriteBatch)
        {
            _position = position;
            _texture = texture;
            SpriteBatch = spriteBatch;
            //Name = _texture;
        }

        public void AddToInventory(string Name)
        {
         
        }

        
     
    }
}
