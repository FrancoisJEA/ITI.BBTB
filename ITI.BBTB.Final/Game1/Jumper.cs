using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    public class Jumper : Sprite
    {
        Weapon _weapon;

        int _time;
        bool _booltime;

        public Vector2 Movement { get; set; }
        private Vector2 oldPosition;

        public Jumper(Texture2D texture, Texture2D weaponTexture, Texture2D bulletTexture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            _weapon = new Weapon(weaponTexture,  bulletTexture, /*DungeonPlanetGame ctx,*/  position,  spritebatch,  this/*, List<Enemy> enemys*/);

            _time = 0;
            _booltime = false;
        }

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

            if (keyboardState.IsKeyDown(Keys.Left)) { Movement -= Vector2.UnitX; }
            if (keyboardState.IsKeyDown(Keys.Right)) { Movement += Vector2.UnitX; }

            if (keyboardState.IsKeyDown(Keys.Down)) { Movement += Vector2.UnitY; }
            if (keyboardState.IsKeyDown(Keys.Up)) { Movement -= Vector2.UnitY; }

            _booltime = keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Space)) Movement -= new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Space)) Movement += new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Space)) Movement -= new Vector2(20, 0);
                else if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space)) Movement += new Vector2(20, 0);

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
            Movement -= Movement * Vector2.One * 0.25f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }

        public override void Draw()
        {
            base.Draw();
            _weapon.Draw();
        }
    }
}