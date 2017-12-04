using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace BBTB
{
    public class Weapon : Sprite
    {
        Player _player;
        internal WeaponLib WeaponLib { get; set; }
        public Vector2 _origin;
        MouseState _currentMouse;
        List<Bullet> Bullets { get; }
        Texture2D _bulletTexture;
        int _time;

        Vector2 _position;

        BBTB _ctx;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, BBTB ctx, Vector2 position, SpriteBatch spritebatch, Player player)
            : base(weaponTexture, position, spritebatch)
        {
            _position = position;
            _player = player;
            _origin = new Vector2(_player.position.X - (_player.Bounds.Width) - 50, _player.position.Y - (_player.Bounds.Height) - 15);
            base.position = new Vector2(_player.position.X + (_player.Bounds.Width / 2), _player.position.Y + (_player.Bounds.Height / 2));
            WeaponLib = new WeaponLib();
            Bullets = new List<Bullet>();
            _time = 15;
            _ctx = ctx;
            _bulletTexture = bulletTexture;
        }

        public void Update(GameTime gameTime)
        {
            CheckMouseAndUpdateMovement();
            BulletUpdate(gameTime);
            position = new Vector2(_player.position.X + (_player.Bounds.Width / 2), _player.position.Y + (_player.Bounds.Height / 2));
            WeaponLib.Update(_currentMouse.X - position.X, _currentMouse.Y - position.Y);
        }

        private void CheckMouseAndUpdateMovement()
        {
            _currentMouse = Mouse.GetState();

            Bullet bullet;

            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                if (_time >= 15)
                {
                    bullet = new Bullet(_bulletTexture, position, SpriteBatch, WeaponLib);
                    Bullets.Add(bullet);

                    _ctx.PlayGunSound();
                    _time = 0;
                }
                else
                {
                    _time += 1;
                }
            }
        }
        private void BulletUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].BulletLib.IsDead() || Bullets[i].HasTouchedEnemy() || Bullets[i].HasTouchedTile())
                {
                    Bullets.Remove(Bullets[i]);
                }
                else
                {
                    Bullets[i].Update(gameTime);
                }
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, position, null, Color.White, WeaponLib.Rotation, _origin, 1, SpriteEffects.None, 0);
            foreach (Bullet bullet in Bullets)
            {
                bullet.Draw();
            }
        }
    }
}
