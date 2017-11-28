﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Bullet : Sprite
    {
        float _rotation;
        Vector2 _origin;
        //List<Enemy> _enemys;
        public BulletLib BulletLib { get; set; }

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib ctx/*, List<Enemy> enemys*/)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(-27, 20);
            _rotation = ctx.Rotation;
            BulletLib = new BulletLib(ctx, new Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
            //_enemys = enemys;
        }
        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X, BulletLib.PositionUpdate().Y);
        }

        /*public bool HasTouchedEnemy()
        {
            foreach (Enemy enemy in _enemys)
            {
                if (new System.Drawing.Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).IntersectsWith(enemy.EnemyLib.Bounds))
                {
                    enemy.EnemyLib.Life -= 10;
                    return true;
                }
            }
            return false;
        }*/

        public bool HasTouchedTile()
        {
            Rectangle bullet = new Rectangle((int)(position.X), (int)(position.Y), Texture.Width, Texture.Height);
            foreach (Tile tile in Board.CurrentBoard.Tiles)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(bullet))
                {
                    return true;
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