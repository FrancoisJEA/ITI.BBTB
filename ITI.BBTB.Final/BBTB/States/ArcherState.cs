using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace BBTB.States
{
    class ArcherState : State
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _jumperTexture, _groundTexture, _bulletTexture, _weaponTexture, _monsterTexture;
        private Player _jumper;
        private Board _board;
        private Sprite _sprite;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;

        Game1 ctx;

        SoundEffect _sound;

        public ArcherState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content) : base(game, graphicsDevice, Content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _weaponTexture = Content.Load<Texture2D>("weapon");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _groundTexture = Content.Load<Texture2D>("ground");
            _tileTexture = Content.Load<Texture2D>("tile");
            _monsterTexture = Content.Load<Texture2D>("monster");
            _jumperTexture = Content.Load<Texture2D>("BBTBplayer");
            _sprite = new Sprite(_groundTexture, new Vector2(60, 60), _spriteBatch);
            _jumper = new Player(_jumperTexture, new Vector2(80, 80), _spriteBatch, this, null);
            Weapon _weapon = new Weapon(_weaponTexture, _bulletTexture, this, _jumper.position, _spriteBatch, _jumper);
            _jumper.Weapon = _weapon;
            _board = new Board(_spriteBatch, _tileTexture, _monsterTexture, 15, 10);
            _debugFont = Content.Load<SpriteFont>("DebugFont");

            _sound = Content.Load<SoundEffect>("Sound/GunSound");
        }


        public override void Update(GameTime gameTime)
        {

            _jumper.Update(gameTime);
            CheckKeyboardAndReact();

        }

        internal void PlayGunSound()
        {
            _sound.Play();
        }

        private void PutJumperInTopLeftCorner()
        {
            _jumper.position = Vector2.One * 80;
            _jumper.Mouvement = Vector2.Zero;
        }

        private void RestartGame()
        {
            Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.WhiteSmoke);
            _spriteBatch.Begin();
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


        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
