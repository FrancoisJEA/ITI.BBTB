﻿using BBTB.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
	public class Preacher: Monster
	{
		God _dieu;
		Board _board;
		Vector2 _position;
		Texture2D _texture;
        SpriteBatch _batch;
        Texture2D itemTexture;
        Item item;
		bool _isAlive;
		bool _activated;
		int _life;

        public Preacher(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive, God dieu, bool activated, List<Texture2D> itemTexture)
			: base(texture,position,batch, itemTexture)
		{
            
			_position = position;
			_batch = batch;
			_texture = texture;
			_isAlive = isAlive;
			_dieu = dieu;
			_activated = activated;
		}

		public int Life { get { return _life; } set { _life = value; } }

		public void Update(GameTime gameTime)
		{
			//IsDead();
		}

		public void Hit(Bullet bullet)
		{
			_life -= bullet.Damages;
			//if (IsDead()) prévenir le jeu pour gagner l'expérience
		}

	

		public override void Draw()
		{
			if (IsAlive)
			{
				base.Draw();
			}
		}
	}
}

