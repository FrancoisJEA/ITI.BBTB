using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BBTB
{
	class MonstersBullet : Sprite
	{
		Player _player;
		Texture2D _texture;
		Vector2 _position;
		Vector2 _destination;
		int _damages;
		const float _p = 2 ;
		float _o = _p;
		bool _bool;

		public MonstersBullet(Texture2D texture, Vector2 position, Vector2 destination, SpriteBatch batch, int Dmg)
			: base(texture, position, batch)
		{
			_texture = texture;
			_position = position;
			_destination = destination;
            _damages = Dmg;
		}

		// public Timer LifeTime { get { return _timer; } }
		public Texture2D Texture { get { return _texture; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }
		public Vector2 Destination { get { return _destination; } }
		public int Damages { get { return _damages; } set { _damages = value; } }

		internal void PositionAvencement()
		{
			Vector2 _basePosition = Position;
			if (_destination.X < _position.X)
			{
				_position.X = Position.X - 2;
				_position.Y = ((_destination.Y - _basePosition.Y) / (_destination.X - _basePosition.X)) * _position.X;
				_position.Y = _position.Y + (_basePosition.Y - ((_destination.Y - _basePosition.Y) / (_destination.X - _basePosition.X)) * _basePosition.X);
			}
			else if (_destination.X > _position.X)
			{
				_position.X = Position.X + 2;
				_position.Y = ((_destination.Y - _basePosition.Y) / (_destination.X - _basePosition.X)) * _position.X;
				_position.Y = _position.Y + (_basePosition.Y - ((_destination.Y - _basePosition.Y) / (_destination.X - _basePosition.X)) * _basePosition.X);
			}
			else if (_destination.X == _position.X)
			{
				_position.Y = _position.Y + 2;
			}
			Position = new Vector2(_position.X, _position.Y);
		}

		public bool DeleteMe()
		{
			if ( this.Bounds.Intersects(new Rectangle((int)Board.CurrentBoard._player.Position.X, (int)Board.CurrentBoard._player.Position.Y, Board.CurrentBoard._player.animation.spriteWidth, Board.CurrentBoard._player.animation.spriteHeight)))//|| _player.Bounds.Intersects(this.Bounds))

			//	if (_position == _destination )//|| _player.Bounds.Intersects(this.Bounds))
			{
				Board.CurrentBoard._player._playerM.Life -= _damages;
				return true; 
			}
           
			else if(_bool)
			{
				return true;
			}

            foreach (Tile tile in Board.CurrentBoard.Tile2)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
                    {
                        return true;
                    }
                }
            }
            foreach (Tile tile in Board.CurrentBoard.Tile)
            {
                if (tile.IsBlocked)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(tile.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
		}

		public void Timer()
		{
			float Time = (float)Board.CurrentBoard.GameTime.ElapsedGameTime.TotalSeconds;
			_o -= Time;
			if(_o < 0)
			{
				_bool = true;
			}
		}

		public void Update(GameTime gameTime)
		{
			PositionAvencement();
			Timer();
		}
		public override void Draw()
		{
			base.Draw();
		}

	}
}
