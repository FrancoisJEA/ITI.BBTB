using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB.Bullets
{
    class MonsterBullets : Bullet
    {
        Player _player;
        float _rotation;

        public MonsterBullets(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib weapon, Board board, Weapon weaponCtx) : base(texture, position, spritebatch, weapon, board, weaponCtx)
        {
            _player = Board.CurrentBoard._player;
        }

        public override void Draw()
        {
            //SpriteBatch.Draw(Texture, Position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
        }

        public void TouchPlayer()
        {
            if (new Rectangle((int)Position.X,(int)Position.Y,Texture.Width,Texture.Height).Intersects(_player.Bounds))
            {
                _player._playerM.Life -= 10; 
            }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
