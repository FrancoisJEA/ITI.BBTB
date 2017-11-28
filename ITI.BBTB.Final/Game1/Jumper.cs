using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    [Serializable]
    public class Jumper : Sprite
    {
        Weapon _weapon;
        Weapon _weapon2;

        int _life, _strenght, _agility, _experience, _intelligence, _resistance;
        string _name;
        int _krumbz;
        [NonSerialized]
        int _level;
        [NonSerialized]
        int _time;
        [NonSerialized]
        bool _booltime;

        public Vector2 Mouvement { get; set; }
        private Vector2 oldPosition;

        public Jumper(Texture2D texture, Texture2D weaponTexture, Texture2D bulletTexture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            _weapon = new Weapon(weaponTexture,  bulletTexture, /*DungeonPlanetGame ctx,*/  position,  spritebatch,  this/*, List<Enemy> enemys*/);

            _time = 0;
            _booltime = false;
        }

        public int Life { get { return _life; } set { _life = 100; } }
        public int Experience { get { return _experience; } set { _experience = value; } }
        public int Strenght { get { return _strenght; } set { _strenght = 20; } }
        public int Agility { get { return _agility; } set { _agility = 20; } }
        public int Intelligence { get { return _intelligence; } set { _intelligence = 20; } }
        public int Resistance { get { return _resistance; }set { _resistance = 10; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Money { get { return _krumbz; } set { _krumbz = 00; } }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            _weapon.Update(gameTime);
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
            _weapon.Draw();
        }
    }
}