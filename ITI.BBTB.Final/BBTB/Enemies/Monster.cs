﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
    public class Monster : Sprite
    {
        public bool IsAlive { get; set; }
        public int Life { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive)
            : base(texture, position, batch)
        {
            IsAlive = isAlive;
        }

        public override void Draw()
        {
            if (IsAlive)
            {
                base.Draw();
            }
        }
    }
}
