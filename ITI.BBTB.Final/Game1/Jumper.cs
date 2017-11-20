using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Jumper : Sprite
    {
        Weapon _weapon;

        public Vector2 Movement { get; set; }
        private Vector2 oldPosition;

        public Jumper(Texture2D texture, Texture2D weaponTexture, Texture2D bulletTexture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            _weapon = new Weapon( weaponTexture,  bulletTexture, /*DungeonPlanetGame ctx,*/  position,  spritebatch,  this/*, List<Enemy> enemys*/);
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

            if (keyboardState.IsKeyDown(Keys.Left)) { Movement -= Vector2.UnitX; }
            if (keyboardState.IsKeyDown(Keys.Right)) { Movement += Vector2.UnitX; }

            if (keyboardState.IsKeyDown(Keys.Down)) { Movement += Vector2.UnitY; }
            if (keyboardState.IsKeyDown(Keys.Up)) { Movement -= Vector2.UnitY; }
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds);
        }

        private void SimulateFriction()
        {
            Movement -= Movement * Vector2.One * 0.25f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;
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