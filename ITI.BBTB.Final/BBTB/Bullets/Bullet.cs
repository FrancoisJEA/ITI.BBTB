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
        float _rotation;
        Vector2 _origin;
        public BulletLib BulletLib { get; set; }
        //PlayerModel _ctxM;

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spritebatch, WeaponLib ctx)
            : base(texture, position, spritebatch)
        {
            _origin = new Vector2(-27, 20);
            _rotation = ctx.Rotation;
            BulletLib = new BulletLib(ctx, new Vector2(base.position.X, base.position.Y), texture.Height, texture.Width);
            //_ctxM = new PlayerModel("Tanguy", 1);
        }
        public void Update(GameTime gameTime)
        {
            BulletLib.Timer((float)gameTime.ElapsedGameTime.TotalSeconds);
            position += new Vector2(BulletLib.PositionUpdate().X, BulletLib.PositionUpdate().Y);
        }

        public bool HasTouchedEnemy()
        {
            foreach (Monster monsters in Board.CurrentBoard.Monsters)
            {
                if (monsters.IsAlive)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height).Intersects(monsters.Bounds))
                    {
                        monsters.Life -= 50;

                        if (monsters.Life <= 0)
                        {
                            monsters.IsAlive = false;
                            //_ctxM.Experience += 100;
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
            return false;
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, position, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
        }
    }
}
