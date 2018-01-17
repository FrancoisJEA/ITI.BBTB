using BBTB.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BBTB
{
    public class Player : Sprite
    {
		#region Champs
		readonly GameState _ctx;
        public Weapon _weapon;
        public PlayerModel _playerM;
        Vector2 _mouvement;
       
        int _time;
        bool _booltime;
        private Vector2 oldPosition;
		bool _havePrayed;
      
        internal string _classes;
		God _god;
		#endregion

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

		#region propriété 
		public bool HavePrayed { get { return _havePrayed; } set { _havePrayed = value; }  }
        public string Classes { get { return _classes ; } set { _classes = value; } }
        public int WeaponType => _weapon.WeaponType;

        public Weapon Weapon => _weapon;

        public GameState Ctx => _ctx;
		#endregion 

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
            StopMovingIfBlocked();
            if (_weapon != null) { _weapon.Update(gameTime); }
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Console.WriteLine(_time);

            if (keyboardState.IsKeyDown(Keys.Q)) { _mouvement -= Vector2.UnitX*2; }
            if (keyboardState.IsKeyDown(Keys.D)) { _mouvement += Vector2.UnitX*2; }

            if (keyboardState.IsKeyDown(Keys.S)) { _mouvement += Vector2.UnitY*2; }
            if (keyboardState.IsKeyDown(Keys.Z)) { _mouvement -= Vector2.UnitY*2; }

            _booltime = keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(20, 0);
                else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(20, 0);

                _time = 300;
                _booltime = false;
            }
            else
            {
                _time -= 1;
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
            if (_weapon != null) _weapon.Draw();
        }
    }
}