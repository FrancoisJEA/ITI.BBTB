using BBTB.Manage;
using BBTB.Models;
using BBTB.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BBTB
{
    [Serializable]
    public class Player : Sprite
    {
        readonly GameState _ctx;
        public Weapon _weapon;
        public PlayerModel _playerM;
        protected Vector2 _position;
        public AnimationManager _animationManager;
        public Input Input = new Input();
        public Texture2D _texture;
        public SpriteBatch _spriteBatch;
        protected Dictionary<string, Animation> _animation;
        Vector2 _mouvement;

        int _time;
        bool _booltime;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        private Vector2 oldPosition;
		//God
		bool _havePrayed;
		God _god;

        public Player(Texture2D texture, Texture2D weaponTexture, Texture2D weaponTexture2, Texture2D bulletTexture, Texture2D bulletTexture2, Vector2 position, SpriteBatch spritebatch, GameState ctx, Weapon weapon, bool havePrayed)
            : base(texture, position, spritebatch)
        {
            _playerM = new PlayerModel("Tanguy", 1);
            _ctx = ctx;
            _time = 0;
            _booltime = false;
            _weapon = weapon;
			_havePrayed = havePrayed;
            _weapon = new Weapon(weaponTexture, bulletTexture, weaponTexture2, bulletTexture2, Position, spritebatch, this);

        }
		
		public bool HavePrayed { get { return _havePrayed; } set { _havePrayed = value; }  }

        public int WeaponType => _weapon.WeaponType;

        public Weapon Weapon => _weapon;

        public GameState Ctx => _ctx;

        public void ResetPosition()
        {
            Position = Vector2.One * 70;
            _mouvement = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            SetAnimation();
            _animationManager.Update(gameTime);
            StopMovingIfBlocked();
            if (_weapon != null) { _weapon.Update(gameTime); }
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Console.WriteLine(_time);

            if (keyboardState.IsKeyDown(Keys.Left)) { _mouvement -= Vector2.UnitX*2; }
            else if (keyboardState.IsKeyDown(Keys.Right)) { _mouvement += Vector2.UnitX*2; }

            else if (keyboardState.IsKeyDown(Keys.Down)) { _mouvement += Vector2.UnitY*2; }
            else if (keyboardState.IsKeyDown(Keys.Up)) { _mouvement -= Vector2.UnitY*2; }


            _booltime = keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(20, 0);
                else if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(20, 0);

                _time = 300;
                _booltime = false;
            }
            else
            {
                _time -= 1;
            }
        }
        protected virtual void SetAnimation()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Right) || Keyboard.GetState().IsKeyDown(Input.Left) || Keyboard.GetState().IsKeyDown(Input.Up) || Keyboard.GetState().IsKeyDown(Input.Down))
            {
                if (_mouvement.X > 0)

                    _animationManager.Play(_animation["RightWalk"]);

                else if (_mouvement.X < 0)
                    _animationManager.Play(_animation["LeftWalk"]);
                else if (_mouvement.Y < 0)
                    _animationManager.Play(_animation["FrontWalk"]);
                else if (_mouvement.Y > 0)
                    _animationManager.Play(_animation["BackWalk"]);
            }
        }

        public bool HasTouchedMonster()
        {
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                if (monster.IsAlive)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(monster.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds);
        }

        private void SimulateFriction()
        {
            _mouvement -= _mouvement * Vector2.One * 0.25f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += _mouvement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { _mouvement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { _mouvement *= Vector2.UnitX; }
        }

        public override void Draw()
        {
            base.Draw();
            if (_texture != null)
                _spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(_spriteBatch);
            else throw new Exception("this aint right...");
            if (_weapon != null) _weapon.Draw();
        }
    }
}