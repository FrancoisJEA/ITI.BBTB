﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBTB.States;
using Microsoft.Xna.Framework.Input;
using BBTB.Items;

namespace BBTB
{
    public class Monster : Sprite
    {
        public bool IsAlive { get; set; }

        int _life;
        public Item _item;
		int _xp;
		Player _player;
        public int _monsterDead;
		Vector2 _position;

        GameState _ctx; // Paramètre du constructeur
        public Texture2D _itemTexture { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,Texture2D itemTexture)
            : base(texture, position, batch)
        {
			_position = position;
            _itemTexture = itemTexture;
            IsAlive = isAlive;
            _life = 100;
			_xp = 10;
            _monsterDead = 0;
        }

        public int MonsterDead { get { return _monsterDead; } set { _monsterDead = value; } }
        public int Life { get { return _life; } set { _life = value; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

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
                _monsterDead++;
               // InventorySystem2 InventorySystem2 = new InventorySystem2();
               // string ItemName = InventorySystem2.Drop_Random_Item();
               // if (ItemName != null) {
                    _item = new Item(new Vector2(this._position.X, this._position.Y), _itemTexture, SpriteBatch);
                    _item.Draw();
                //}
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
