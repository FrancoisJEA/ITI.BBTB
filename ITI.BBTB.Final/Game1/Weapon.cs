﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    public class Weapon : Sprite
    {
        Jumper _player;
        internal WeaponLib WeaponLib { get; set; }
        public Vector2 _origin;
        MouseState _currentMouse;
        List<Bullet> Bullets { get; }
        Texture2D _bulletTexture;
        int _time;

        //Game1 _ctx;
        //List<Enemy> _enemys;

        public Weapon(Texture2D weaponTexture, Texture2D bulletTexture, /*Game1 ctx,*/ Vector2 position, SpriteBatch spritebatch, Jumper player/*, List<Enemy> enemys*/)
            : base(weaponTexture, position, spritebatch)
        {
            _player = player;
            _origin = new Vector2(_player.position.X - (_player.Bounds.Width) - 50, _player.position.Y - (_player.Bounds.Height) - 15);
            base.position = new Vector2(_player.position.X + (_player.Bounds.Width / 2), _player.position.Y + (_player.Bounds.Height / 2));
            WeaponLib = new WeaponLib();
            Bullets = new List<Bullet>();
            _time = 15;
            //_ctx = ctx;
            _bulletTexture = bulletTexture;
            //_enemys = enemys;
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
                    bullet = new Bullet(_bulletTexture, position, SpriteBatch, WeaponLib/*, _enemys*/);
                    Bullets.Add(bullet);
                    
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
                if (Bullets[i].BulletLib.IsDead()/* || Bullets[i].HasTouchedEnemy()*/ || Bullets[i].HasTouchedTile())
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
