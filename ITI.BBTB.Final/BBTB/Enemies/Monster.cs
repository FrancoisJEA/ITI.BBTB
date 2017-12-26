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
		Player player;
		Vector2 _position;
		double l;


        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive)
            : base(texture, position, batch)
        {
            IsAlive = isAlive;
        }

		public void Intelligence()
	    {
			double k;
			k = (position.X - player.position.X) * (position.X - player.position.X) + (position.Y - player.position.Y) * (position.Y - player.position.Y);
			k = Math.Sqrt(k);
			
			if ( 10 < k && k <80 )
			{
				// this.shot(player.position);

			}
		}

		// public static double Sqrt( double l) { return l;}

        public override void Draw()
        {
            if (IsAlive)
            {
                base.Draw();
            }
        }
    }
}
