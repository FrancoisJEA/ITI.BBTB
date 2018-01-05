using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace BBTB.States
{
    public class GameState : State
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _tileTexture2, _tileTexture3, _jumperTexture, _groundTexture, _bulletTexture, _weaponTexture, _monsterTexture;
        private Player _player;
        private Board _board;
        private Sprite _sprite;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;

        SoundEffect _sound;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content) : base(game, graphicsDevice, Content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _weaponTexture = Content.Load<Texture2D>("weapon");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _groundTexture = Content.Load<Texture2D>("ground");
            _tileTexture = Content.Load<Texture2D>("tile");
            _tileTexture2 = Content.Load<Texture2D>("barrel");
            _tileTexture3 = Content.Load<Texture2D>("stairs");
            _monsterTexture = Content.Load<Texture2D>("monster");
            _jumperTexture = Content.Load<Texture2D>("BBTBplayer");
            _sprite = new Sprite(_groundTexture, new Vector2(60, 60), _spriteBatch);
            _player = new Player(_jumperTexture, new Vector2(80, 80), _spriteBatch, this, null, false);
            Weapon _weapon = new Weapon(_weaponTexture, _bulletTexture, this, _player.position, _spriteBatch, _player);
            _player.Weapon = _weapon;
            _board = new Board(_spriteBatch, _tileTexture, _tileTexture2, _tileTexture3, _monsterTexture, 15, 10, _player, this);
            _debugFont = Content.Load<SpriteFont>("DebugFont");

            _sound = Content.Load<SoundEffect>("Sound/GunSound");

        }

        internal Board Board
        {
            get { return _board; }
        }

        public override void Update(GameTime gameTime)
        {

            _player.Update(gameTime);
            foreach (Monster monster in _board.Monsters) monster.Update(gameTime);
			//foreach (Preacher preacher in _board.Preacher) preacher.Update(gameTime);
			CheckKeyboardAndReact();
            _board.Update(gameTime);
        }

        internal void PlayGunSound()
        {
            _sound.Play();
        }

        internal void PutJumperInTopLeftCorner()
        {
            _player.position = Vector2.One * 80;
            _player.Mouvement = Vector2.Zero;
        }

        private void RestartGame()
        {
            Board.CurrentBoard.Stage1();
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
            _player.Draw();
            _spriteBatch.End();

        }

        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _player.position.X, _player.position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.Mouvement.X, _player.Mouvement.Y);
            string lifeInText = string.Format("Character's life: ({0:0})", _player._playerM.Life);
            string experienceInText = string.Format("Character's experience: ({0:0})", _player._playerM.Experience);

            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(lifeInText, new Vector2(200, 200));
            DrawWithShadow(experienceInText, new Vector2(240, 240));
            DrawWithShadow("F5 for random board", new Vector2(70, 600));
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
