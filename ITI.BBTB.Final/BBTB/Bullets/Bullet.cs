using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB
{
    public class Bullet : Sprite
    {
        Board _ctx;
        float _rotation;
        Vector2 _origin;
        public BulletLib BulletLib { get; set; }

        //List<Bullet> Bullets { get; }

        int _damages;

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib weapon, Board ctx)
            : base(texture, position, spritebatch)
        {
            _ctx = ctx;
            _origin = new Vector2(-27, 20);
            _rotation = weapon.Rotation;
            BulletLib = new BulletLib(weapon, new Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);

            //Bullets = new List<Bullet>();

            _damages = 50;
        }

        public int Damages { get { return _damages; } set { _damages = value; } }

        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X, BulletLib.PositionUpdate().Y);
            TouchEnemy();
            //BulletUpdate(gameTime);
        }

        public bool TouchEnemy()
        {
            foreach(Monster monster in Board.CurrentBoard.Monsters)

                if (monster.IsAlive)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).Intersects(monster.Bounds))
                    {
                        monster.Hit(this);
                        if (monster.Life <= 0) monster.IsAlive = false;
                        _ctx.OnBulletDestroy(this);
                        return true;
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
