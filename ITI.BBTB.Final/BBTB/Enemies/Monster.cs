﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBTB.States;
using Microsoft.Xna.Framework.Input;

namespace BBTB
{
    public class Monster : Sprite
    {
        public bool IsAlive { get; set; }

        int _life;
		int _xp;
		Player _player;
        GameState _ctx; // Paramètre du constructeur

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive)
            : base(texture, position, batch)
        {
            IsAlive = isAlive;
            _life = 100;
			_xp = 10;
        }

        public int Life { get { return _life; } set { _life = value; } }

        public void Update(GameTime gameTime)
        {
            IsDead();
            KillAllMonsters();
        }

        public void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            //if (IsDead()) prévenir le jeu pour gagner l'expérience
        }

        public bool IsDead()
        {
            if (_life <= 0)
            { 
                IsAlive = false;
                return true;
            }
            return false;
        }

        public override void Draw()
        {
            if (IsAlive)
            {
                base.Draw();
            }
        }

        internal void KillAllMonsters()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.B)) { IsAlive = false; }
        }

    }
}
