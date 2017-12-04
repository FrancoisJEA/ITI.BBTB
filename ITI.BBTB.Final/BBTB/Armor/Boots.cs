using BBTB;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Armor
{
    class Boots : Sprite
    {
        readonly ArmorPieceModel Stats;
        Texture2D _texture;
        Vector2 _position;

        public Boots(Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            _texture = texture;
            _position = position;
        }
    }
}
