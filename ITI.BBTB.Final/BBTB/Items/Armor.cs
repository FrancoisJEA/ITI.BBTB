using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB.Items
{
    class Armor : Item
    {
        int Protection { get; }

        public Armor(Vector2 position, Texture2D texture, SpriteBatch spriteBatch, Player player) : base(position, texture, spriteBatch,player)
        {
            this.Protection = player._playerM.Level * 14;
        }
    }
}
