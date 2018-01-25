using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BBTB.Enemies;
using BBTB.Items;
using Microsoft.Xna.Framework.Content;

namespace BBTB
{
    public class Bullet : Sprite
    {
		Board _ctx;
        float _rotation;
        Boss _boss;
        public Monster _monster;
        Vector2 _origin;
        private SpriteBatch _spriteBatch;
        public BulletLib BulletLib { get; set; }

        protected ContentManager _content;
        protected GraphicsDevice GraphicsDevice;
    
        Weapon _weaponCtx;

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib weapon, Board board, Weapon weaponCtx)
            : base(texture, position, spritebatch)
        {
           
    
            _origin = new Vector2(-27, 20);
            _rotation = weapon.Rotation;
            BulletLib = new BulletLib(weapon, new Vector2(base.Position.X, base.Position.Y), texture.Height, texture.Width);    
            _weaponCtx = weaponCtx;
			_ctx = board;
            _boss = _ctx._boss;
        }


        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (TouchEnemy() )
                _monster.Update(gameTime);
            Position += new Vector2(BulletLib.PositionUpdate().X, BulletLib.PositionUpdate().Y);
        }

		public bool TouchEnemy()
		{
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(monster.Bounds))
                    {
                        monster.Hit(this);
                        if (monster.Life <= 0)
                        {
                        Board.CurrentBoard.KillMonster();

                            _monster = monster;
                            monster.IsDead = false;
                        return true;

                        new Rectangle((int)monster.Position.X, (int)monster.Position.Y, Texture.Width, Texture.Height);
                        }
                        
                    }
            }
            
            if(_boss.AddBoss)
            {
                if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(_boss.Bounds))
                {
                    _boss.Hit(this);
                    if (_boss.Life < +0) _boss.IsAlive = false;
                    return true;
                    }
                
            }
				return false;
        }

        public bool HasTouchedTile()
        {
            foreach (Tile tile in Board.CurrentBoard.Tile)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
                    {
                        return true;
                    }
                }
            }

            foreach (Tile tile in Board.CurrentBoard.Tile2)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
                    {
                        /*for (int i = 0; i < Bullets.Count; i++) Bullets.Remove(Bullets[i]);*/
                        return true;
                    }
                }
            }
            return false;
        }

		public override void Draw()
        {
            SpriteBatch.Draw(Texture, Position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
         
        }
    }
}
