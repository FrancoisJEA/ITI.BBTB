using Microsoft.Xna.Framework;
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
		Player _player;
		Vector2 _position;

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive)
            : base(texture, position, batch)
        {
            IsAlive = isAlive;
        }

		public void Intelligence()
	    {
			double k;
			k = (position.X - _player.position.X) * (position.X - _player.position.X) + (position.Y - _player.position.Y) * (position.Y - _player.position.Y);
			k = Math.Sqrt(k);
			
			if ( 10 < k && k <80 )
			{
				this.Shot(_player.position);
				this.Move(_player.position);
			}
		}

		public void Shot(Vector2 positionPlayer)
		{
			
		}

		public void Move(Vector2 p)
		{

		}

		public void Damages( int life)
		{
			if ()
			{

			}
			

			
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
