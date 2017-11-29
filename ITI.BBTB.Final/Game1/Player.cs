using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    [Serializable]
    public class Player : Sprite
    {
        Weapon _weapon;
        Weapon _weapon2;

        Game1 _ctx;

        int _life, _strenght, _agility, _experience, _intelligence, _resistance;
        string _name;
        int _krumbz;

        Texture2D _texture;
        Vector2 _position;
        
        int _level;
        int _time;
        bool _booltime;
        private Vector2 oldPosition;

        public Player(Texture2D texture, Vector2 position, SpriteBatch spritebatch, Game1 ctx, Weapon weapon)
            : base(texture, position, spritebatch)
        {
            _ctx = ctx;
            _texture = texture;
            _position = position;
            _time = 0;
            _booltime = false;
            _weapon = weapon;
        }

        public int Life { get { return _life; } set { _life = 100; } }
        public int Experience { get { return _experience; } set { _experience = value; } }
        public int Strenght { get { return _strenght; } set { _strenght = 20; } }
        public int Agility { get { return _agility; } set { _agility = 20; } }
        public int Intelligence { get { return _intelligence; } set { _intelligence = 20; } }
        public int Resistance { get { return _resistance; }set { _resistance = 10; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Money { get { return _krumbz; } set { _krumbz = 00; } }

        public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
        public Weapon Weapon2 { get { return _weapon2; } set { _weapon2 = value; } }

        public Vector2 Mouvement { get; set; }


        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            if (_weapon != null) _weapon.Update(gameTime);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Console.WriteLine(_time);

            if (keyboardState.IsKeyDown(Keys.Left)) { Mouvement -= Vector2.UnitX; }
            if (keyboardState.IsKeyDown(Keys.Right)) { Mouvement += Vector2.UnitX; }

            if (keyboardState.IsKeyDown(Keys.Down)) { Mouvement += Vector2.UnitY; }
            if (keyboardState.IsKeyDown(Keys.Up)) { Mouvement -= Vector2.UnitY; }

            _booltime = keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space)) Mouvement -= new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space)) Mouvement += new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space)) Mouvement -= new Vector2(20, 0);
                else if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space)) Mouvement += new Vector2(20, 0);

                _time = 300;
                _booltime = false;
            }
            else
            {
                _time -= 1;
            }
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = position;
            UpdatePositionBasedOnMovement(gameTime);
            position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, position, Bounds);
        }

        private void SimulateFriction()
        {
            Mouvement -= Mouvement * Vector2.One * 0.25f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            position += Mouvement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = position - oldPosition;
            if (lastMovement.X == 0) { Mouvement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Mouvement *= Vector2.UnitX; }
        }

        public override void Draw()
        {
            base.Draw();
            if (_weapon != null) _weapon.Draw();
        }
    }
}