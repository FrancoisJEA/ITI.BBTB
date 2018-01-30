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
using BBTB.Items;

namespace BBTB
{
    public class Weapon : Sprite
    {
        readonly Vector2 _rotationOrigin;
        readonly Player _player;
        internal WeaponLib WeaponLib { get; set; }
        public Texture2D _weaponTexture, _bulletTexture, _weaponTexture2, _bulletTexture2;
        int _time;
        List<Item> Inventory;
        public List<Texture2D> _bulletTextures;
        GameState _ctx;
        int _damages;

        public Weapon(GameState ctx, Texture2D Texture,Vector2 position, SpriteBatch spritebatch, Player player, List<Texture2D> BulletTextures)
            : base (Texture,position,spritebatch)
        {
            WeaponLib = new WeaponLib();
            _player = player;
            _bulletTextures = BulletTextures;
            Inventory = _player.Inventory.InventoryList();
            _weaponTexture = Inventory[4]._texture;

            _ctx = ctx;
           
            _weaponTexture2 = Inventory[5]._texture;

            SetWeaponType(1);
            _bulletTextures = Inventory[4].DefineBulletTexture(BulletTextures, _player.PlayerClasse);

            _rotationOrigin = new Vector2(_player.Position.X - (_player.Bounds.Width) - 50, _player.Position.Y - (_player.Bounds.Height) - 15);
            Position = new Vector2(_player.Position.X + (_player.Bounds.Width / 2), _player.Position.Y + (_player.Bounds.Height / 2));
            _time = 15;
        }

		public int Damages { get { return _damages;} set { _damages = value; } }

        public int WeaponType => Texture == _weaponTexture ? 1 : 2;

        void SetWeaponType( int type )
        {
            if (type == 1)
            {
                Texture = Inventory[4]._texture;
                _damages = Inventory[4].Attack;
            }
            else
            {
                Texture = Inventory[5]._texture;
                _damages = Inventory[5].Attack;
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.E)) SetWeaponType(1);
            if (keyboardState.IsKeyDown(Keys.R)) SetWeaponType(2);
            

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
                    var bTexture = WeaponType == 1 ? _bulletTextures[0] : _bulletTextures[1];
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
