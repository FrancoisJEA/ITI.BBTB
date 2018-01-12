using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BBTB.Items;

namespace BBTB
{
    public class Bullet : Sprite
    {
		Board _ctx;
        float _rotation;
        Vector2 _origin;
        private SpriteBatch _spriteBatch;
        List<Monster> _enemys;
        public BulletLib BulletLib { get; set; }
		int _damages;
        public Sprite _item;
        private Texture2D _itemTexture;
        protected ContentManager _content;
        protected GraphicsDevice GraphicsDevice;

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib weapon,Board board)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(-27, 20);
            _rotation = weapon.Rotation;
            BulletLib = new BulletLib(weapon, new Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
			_damages = 50;
			_ctx = board;
        }

		public int Damages { get { return _damages;} set { _damages = value; } }
        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X, BulletLib.PositionUpdate().Y);
        }

		public bool TouchEnemy()
		{
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                if (monster.IsAlive)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).Intersects(monster.Bounds))
                    {
                        monster.Hit(this);
                        if (monster.Life <= 0)
                        { 
                            monster.IsAlive = false;
                           

                            new Rectangle((int)monster.position.X, (int)monster.position.Y, Texture.Width, Texture.Height);
                        }
                        return true;
                    }
                }
            }
				return false;
        }

        public bool HasTouchedTile()
        {
            foreach (Tile tile in Board.CurrentBoard.Tiles)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
                    {
                        return true;
                    }
                }
            }

            foreach (Tile tile in Board.CurrentBoard.Tiles2)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
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
            SpriteBatch.Draw(Texture, position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
         
        }
    }
}
