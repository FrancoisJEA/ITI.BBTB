﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
	class MonstersBullet : Sprite
	{
		Player _player;
		Texture2D _texture;
		Vector2 _position;
		Vector2 _destination;
		int _damages;
		
		public MonstersBullet(Texture2D texture,Vector2 position,Vector2 destination,SpriteBatch batch)
			:base (texture,position,batch)
		{
			_texture = texture;
			_position = position;
			_destination = destination;
		}

		public Texture2D Texture { get { return _texture; } }
		public Vector2 Position { get { return _position; } set { _position = value; } }
		public Vector2 Destination { get { return _destination; } set { _destination = value; } }
		public int Damages { get { return _damages; } set { _damages = value; } }

		public void TouchPlayer()
		{
			if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(_player.Bounds))
			{
				_player._playerM.Life -= 10;
			}
		}

		public override void Draw()
		{
			base.Draw();
		}

	}
}
