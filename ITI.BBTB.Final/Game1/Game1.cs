using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _jumperTexture, _groundTexture, _bulletTexture, _weaponTexture, _monsterTexture;
        private Player _jumper;
        private Board _board;
        private Sprite _sprite;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;

        SoundEffect _sound;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 640;

            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _weaponTexture = Content.Load<Texture2D>("weapon");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _groundTexture = Content.Load<Texture2D>("ground");
            _tileTexture = Content.Load<Texture2D>("tile");
            _monsterTexture = Content.Load<Texture2D>("monster");
            _jumperTexture = Content.Load<Texture2D>("BBTBplayer");
            _sprite = new Sprite(_groundTexture, new Vector2(60, 60), _spriteBatch);
            _jumper = new Player(_jumperTexture, _weaponTexture, _bulletTexture, new Vector2(80, 80), _spriteBatch, this);
            _board = new Board(_spriteBatch, _tileTexture, _monsterTexture, 15, 10);
            _debugFont = Content.Load<SpriteFont>("DebugFont");

            _sound = Content.Load<SoundEffect>("Sound/GunSound");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _jumper.Update(gameTime);
            CheckKeyboardAndReact();
        }

        internal void PlayGunSound()
        {
            _sound.Play();
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
            if (state.IsKeyDown(Keys.Escape)) { Exit(); }
        }

        private void RestartGame()
        {
            Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();
        }

        private void PutJumperInTopLeftCorner()
        {
            _jumper.position = Vector2.One * 80;
            _jumper.Mouvement = Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _sprite.Draw();
            _board.Draw();
            WriteDebugInformation();
            _jumper.Draw();
            _spriteBatch.End();
        }

        private void WriteDebugInformation()
        {
            //string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _jumper.Position.X, _jumper.Position.Y);
            //string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _jumper.Movement.X, _jumper.Movement.Y);

           // DrawWithShadow(positionInText, new Vector2(10, 0));
            //DrawWithShadow(movementInText, new Vector2(10, 20));
           // DrawWithShadow("F5 for random board", new Vector2(70, 600));
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);
        }
    }
}