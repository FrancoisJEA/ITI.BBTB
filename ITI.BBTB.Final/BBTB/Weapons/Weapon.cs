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
        readonly Vector2 _rotationOrigin;
        readonly Player _player;
        internal WeaponLib WeaponLib { get; set; }
        readonly Texture2D _weaponTexture, _bulletTexture, _weaponTexture2, _bulletTexture2;
        int _time;

        GameState _ctx;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, Texture2D weaponTexture2, Texture2D bulletTexture2, Vector2 position, SpriteBatch spritebatch, Player player)
            : base(weaponTexture, position, spritebatch)
        {
            _bulletTexture = bulletTexture;
            _weaponTexture = weaponTexture;
            _bulletTexture2 = bulletTexture2;
            _weaponTexture2 = weaponTexture2;
            _player = player;
            _rotationOrigin = new Vector2(_player.Position.X - (_player.Bounds.Width) - 50, _player.Position.Y - (_player.Bounds.Height) - 15);
            Position = new Vector2(_player.Position.X + (_player.Bounds.Width / 2), _player.Position.Y + (_player.Bounds.Height / 2));
            WeaponLib = new WeaponLib();
            _time = 15;
            _ctx = player.Ctx;
        }

        public int WeaponType => Texture == _weaponTexture ? 1 : 2;

        void SetWeaponType( int type )
        {
            if (type == 1) Texture = _weaponTexture;
            else Texture = _weaponTexture2;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.O)) SetWeaponType( 1 );
            if (keyboardState.IsKeyDown(Keys.P)) SetWeaponType( 2 );

            var mousePos = CheckMouseAndUpdateMovement();
            Position = new Vector2(_player.Position.X + (_player.Bounds.Width / 2), _player.Position.Y + (_player.Bounds.Height / 2));
            WeaponLib.Update(mousePos.X - Position.X, mousePos.Y - Position.Y);
        }

        private Vector2 CheckMouseAndUpdateMovement()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (_time >= 15)
                {
                    var bTexture = WeaponType == 1 ? _bulletTexture : _bulletTexture2;
                    _ctx.Board.CreateBullet(bTexture, Position, SpriteBatch, WeaponLib);
                    _ctx.PlayGunSound();
                    _time = 0;
                }
                else
                {
                    _time += 1;
                }
            }
            return new Vector2(mouse.X, mouse.Y);
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, Position, null, Color.White, WeaponLib.Rotation, _rotationOrigin, 1, SpriteEffects.None, 0);
        }
    }
}
