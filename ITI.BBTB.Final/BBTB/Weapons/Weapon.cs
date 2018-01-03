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
using BBTB.States;

namespace BBTB
{
    public class Weapon : Sprite
    {
        Player _player;
        internal WeaponLib WeaponLib { get; set; }
        public Vector2 _origin;
        MouseState _currentMouse;
        Texture2D _bulletTexture;
        int _time;

        Vector2 _position;

        GameState _ctx;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, GameState ctx, Vector2 position, SpriteBatch spritebatch, Player player)
            : base(weaponTexture, position, spritebatch)
        {
            _position = position;
            _player = player;
            _origin = new Vector2(_player.position.X - (_player.Bounds.Width) - 50, _player.position.Y - (_player.Bounds.Height) - 15);
            base.position = new Vector2(_player.position.X + (_player.Bounds.Width / 2), _player.position.Y + (_player.Bounds.Height / 2));
            WeaponLib = new WeaponLib();
            _time = 15;
            _ctx = ctx;
            _bulletTexture = bulletTexture;
        }

        public void Update(GameTime gameTime)
        {
            CheckMouseAndUpdateMovement();
            position = new Vector2(_player.position.X + (_player.Bounds.Width / 2), _player.position.Y + (_player.Bounds.Height / 2));
            WeaponLib.Update(_currentMouse.X - position.X, _currentMouse.Y - position.Y);
        }

        private void CheckMouseAndUpdateMovement()
        {
            _currentMouse = Mouse.GetState();

            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                if (_time >= 15)
                {
                    _ctx.Board.CreateBullet(_bulletTexture, position, SpriteBatch, WeaponLib);
                    _ctx.PlayGunSound();
                    _time = 0;
                }
                else
                {
                    _time += 1;
                }
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, position, null, Color.White, WeaponLib.Rotation, _origin, 1, SpriteEffects.None, 0);
        }
    }
}
