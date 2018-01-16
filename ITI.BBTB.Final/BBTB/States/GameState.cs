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

using BBTB.Items;

namespace BBTB.States
{
    public class GameState : State
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private Board _board;
        private Sprite _background;
        private Sprite _sprite, _item;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;
   
        private Monster monster;

        public Item Item;
        SoundEffect _sound;
        public List<Texture2D> _itemTexture;

        internal Board Board
        {
            get { return _board; }
        }


        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content) : base(game, graphicsDevice, Content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);


            var groundTexture = Content.Load<Texture2D>("ground");
            var tileTexture = Content.Load<Texture2D>("tile");
            var tileTexture2 = Content.Load<Texture2D>("barrel");
            var tileTexture3 = Content.Load<Texture2D>("stairs");
            var monsterTexture = Content.Load<Texture2D>("monster");
            var playerTexture = Content.Load<Texture2D>("BBTBplayer");
            _itemTexture = ItemTextures(Content);
            var weaponTexture = Content.Load<Texture2D>("weapon");
            var bulletTexture = Content.Load<Texture2D>("bullet");
            var weaponTexture2 = Content.Load<Texture2D>("weapon2");
            var bulletTexture2 = Content.Load<Texture2D>("bullet2");
			var _chestTexture = Content.Load<Texture2D>("chest");
            
            _player = new Player(playerTexture, weaponTexture, weaponTexture2, bulletTexture, bulletTexture2, new Vector2(80, 80), _spriteBatch, this, null, false);

            _background = new Sprite(Content.Load<Texture2D>("ground"), new Vector2(60, 60), _spriteBatch);
            _board = new Board(_spriteBatch, tileTexture, tileTexture2, tileTexture3,_chestTexture, monsterTexture, 15, 10, _player, this,_itemTexture);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
          

            _sound = Content.Load<SoundEffect>("Sound/GunSound");
        }

    

       
     public List<Texture2D> ItemTextures (ContentManager Content)
        {
            List<Texture2D> AllTextures = new List<Texture2D>();
            AllTextures.Add(Content.Load<Texture2D>("Steel_boots"));
            AllTextures.Add(Content.Load<Texture2D>("Bow"));
            AllTextures.Add(Content.Load<Texture2D>("Steel_boots"));
            AllTextures.Add(Content.Load<Texture2D>("Bow"));
            AllTextures.Add(Content.Load<Texture2D>("Bow"));
            return AllTextures;
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);


            foreach (Monster monster in _board.Monsters) monster.Update(gameTime);
            foreach (Tile tile in _board.Tiles2) tile.Update(gameTime);
            //foreach (Preacher preacher in _board.Preacher) preacher.Update(gameTime);
            CheckKeyboardAndReact();
            _board.Update(gameTime);

        }

        internal void PlayGunSound()
        {
            _sound.Play();
        }

        private void RestartGame()
        {
            Board.CurrentBoard.Stage1();
            _player.ResetPosition();
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
            _background.Draw();
            _board.Draw();

            WriteDebugInformation();
            _player.Draw();
            _spriteBatch.End();

        }
		
        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _player.Position.X, _player.Position.Y);
          //string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.Mouvement.X, _player.Mouvement.Y);

            string lifeInText = string.Format("Character's life: ({0:0})", _player._playerM.Life);
            string experienceInText = string.Format("Character's experience: ({0:0})", _player._playerM.Experience);

            string RoomNumberInText = string.Format("Room Number: ({0:0})", Board.CurrentBoard.RoomNumber);
            string StageNumberInText = string.Format("Stage Number: ({0:0})", Board.CurrentBoard.StageNumber);

            string SpecialInText = string.Format("Special Number: ({0:0})", Board.CurrentBoard.Special);
            string SpecialTypeInText = string.Format("Special Type: ({0:0})", Board.CurrentBoard.SpecialType);

            //string monsterDeadInText = string.Format("Monsters Dead: ({0:0})", Board.monsters.MonsterDead);

            DrawWithShadow(positionInText, new Vector2(10, 0));
          //DrawWithShadow(movementInText, new Vector2(10, 20));

            DrawWithShadow(lifeInText, new Vector2(200, 200));
            DrawWithShadow(experienceInText, new Vector2(240, 240));

            DrawWithShadow(RoomNumberInText, new Vector2(280, 280));
            DrawWithShadow(StageNumberInText, new Vector2(320, 320));

            DrawWithShadow(SpecialInText, new Vector2(360, 360));
            DrawWithShadow(SpecialTypeInText, new Vector2(400, 400));

            //DrawWithShadow(monsterDeadInText, new Vector2(440, 440));

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
