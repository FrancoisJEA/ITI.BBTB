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
using BBTB.Enemies;

using BBTB.Items;

namespace BBTB.States
{
    public class GameState : State
    {
        Texture2D[,] mapTextures;
        
        private SpriteBatch _spriteBatch;
        private Player _player;
        private Board _board;
        private Sprite _background;
        private Random _rnd = new Random();
        private SpriteFont _debugFont;

        //public int _endTimer;

        public float totalSecond;
        public float hungerTime = 300;
        string hungerText = "";

        SoundEffect _sound;
        SoundEffect _tpsound;
        SoundEffect _bgsound;

        public List<Texture2D> _itemTexture;
        PlayerInventory Inventory;
        List<Texture2D> _bulletTextures;
        public Texture2D goblinTexture;
        Texture2D LvlUpTexture;
        Texture2D heartTexture;
        private Texture2D TraderTexture;

        internal Board Board
        {
            get { return _board; }
        }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content, string classeSelected) : base(game, graphicsDevice, Content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            LvlUpTexture = Content.Load<Texture2D>("lvlup");

            totalSecond = hungerTime;

            mapTextures = new Texture2D[11, 4]; // Nombre d'étages (11 - 1), type de murs, type de ground, type de monstre ou type d'obstacle (0 ground, 1 murs, 2 monstre, 3 obstacle))
            
            mapTextures[0, 0] = Content.Load<Texture2D>("ground");
            mapTextures[0, 1] = Content.Load<Texture2D>("tile");
            mapTextures[0, 2] = Content.Load<Texture2D>("monster");
            mapTextures[0, 3] = Content.Load<Texture2D>("obstacle1");

            mapTextures[1, 0] = Content.Load<Texture2D>("ground2");
            mapTextures[1, 1] = Content.Load<Texture2D>("tile2");
            mapTextures[1, 2] = Content.Load<Texture2D>("monster2");
            mapTextures[1, 3] = Content.Load<Texture2D>("obstacle2");

            mapTextures[2, 0] = Content.Load<Texture2D>("ground3");
            mapTextures[2, 1] = Content.Load<Texture2D>("tile3");
            mapTextures[2, 2] = Content.Load<Texture2D>("monster3");
            mapTextures[2, 3] = Content.Load<Texture2D>("obstacle3");

            mapTextures[3, 0] = Content.Load<Texture2D>("ground4");
            mapTextures[3, 1] = Content.Load<Texture2D>("tile4");
            mapTextures[3, 2] = Content.Load<Texture2D>("monster4");
            mapTextures[3, 3] = Content.Load<Texture2D>("obstacle4");

            mapTextures[4, 0] = Content.Load<Texture2D>("ground5");
            mapTextures[4, 1] = Content.Load<Texture2D>("tile5");
            mapTextures[4, 2] = Content.Load<Texture2D>("monster5");
            mapTextures[4, 3] = Content.Load<Texture2D>("obstacle5");

            mapTextures[5, 0] = Content.Load<Texture2D>("ground6");
            mapTextures[5, 1] = Content.Load<Texture2D>("tile6");
            mapTextures[5, 2] = Content.Load<Texture2D>("monster6");
            mapTextures[5, 3] = Content.Load<Texture2D>("obstacle6");

            heartTexture = Content.Load<Texture2D>("heart");

            //goblinTexture = Content.Load<Texture2D>("goblin");
            var tileTexture = Content.Load<Texture2D>("tile");
            var tileTexture2 = Content.Load<Texture2D>("barrel");
            var archerTexture = Content.Load<Texture2D>("Character/P_archer");
            var mageTexture = Content.Load<Texture2D>("Character/P_mage");
            var gunnerTexture = Content.Load<Texture2D>("Character/P_gunner");
            var tileTexture3 = Content.Load<Texture2D>("stairs");
            var tileTexture4 = Content.Load<Texture2D>("stairs_closed");
            var monsterTexture = Content.Load<Texture2D>("monster");
            var basicTexture = Content.Load<Texture2D>("Character/P_gunner");
            _bulletTextures = BulletTextures(Content);
            _itemTexture = ItemTextures(Content);
            var tileTexture5 = Content.Load<Texture2D>("shop");
            var tileTexture6 = Content.Load<Texture2D>("torche");
            var tileTexture7 = Content.Load<Texture2D>("spikes");
            var _boxTexture = Content.Load<Texture2D>("HUDBox");
            var _boxTexture2 = Content.Load<Texture2D>("HUDBox2");
            var _chestTexture = Content.Load<Texture2D>("chest");
            var _chestTexture2 = Content.Load<Texture2D>("chest_open");
            var _bossTexture = Content.Load<Texture2D>("boss");
            var TileBis = Content.Load<Texture2D>("tilebis");
            
            TraderTexture = Content.Load<Texture2D>("trader");
            _debugFont = Content.Load<SpriteFont>("DebugFont");

            _sound = Content.Load<SoundEffect>("Sound/GunSound"); 
            _tpsound = Content.Load<SoundEffect>("tpsound"); 
            _bgsound = Content.Load<SoundEffect>("Sound/bgmusic"); 
            
            Inventory = new PlayerInventory(_itemTexture, _spriteBatch, _boxTexture,_boxTexture2);
            if (classeSelected == "Wizard")
            {
                basicTexture = Content.Load<Texture2D>("Character/P_mage");
                _player = new Player(basicTexture, new Vector2(80, 80), _spriteBatch, this, null, false, Inventory, _bulletTextures, classeSelected);
            } else if (classeSelected == "Gunner")
            {
                basicTexture = Content.Load<Texture2D>("Character/P_gunner");
                _player = new Player(basicTexture, new Vector2(80, 80), _spriteBatch, this, null, false, Inventory, _bulletTextures, classeSelected);
            } else if (classeSelected == "Archer")
            {
                basicTexture = Content.Load<Texture2D>("Character/P_archer");
                _player = new Player(basicTexture, new Vector2(80, 80), _spriteBatch, this, null, false, Inventory, _bulletTextures, classeSelected);
            }
                _board = new Board(_spriteBatch, tileTexture, tileTexture2, tileTexture3, tileTexture4, tileTexture5, tileTexture6, tileTexture7, _chestTexture,_chestTexture2, monsterTexture, mapTextures,TraderTexture, mapTextures[1, 2], _bossTexture, 15, 10, _player, this,_itemTexture,_debugFont,LvlUpTexture,TileBis);

            _bgsound.Play(0.3f, 0, 0);
        }

        public List<Texture2D> ItemTextures (ContentManager Content)
        {
            List<Texture2D> AllTextures = new List<Texture2D>();
            AllTextures.Add(Content.Load<Texture2D>("InventorySprite"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Steel_gloves"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Steel_boots"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Steel_helmet"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Super_bow"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Steel_armor"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Leather_armor"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Leather_boots"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Leather_gloves"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Leather_helmet"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Fireball_gun"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Super_staff"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Heal_potion"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Used_potion"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Deadric_crossbow"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Fire_book"));
            AllTextures.Add(Content.Load<Texture2D>("Items/Ice_gun"));
            return AllTextures;
        }

        public List<Texture2D> BulletTextures(ContentManager Content)
        {
            List<Texture2D> AllTextures = new List<Texture2D>();
            AllTextures.Add(Content.Load<Texture2D>("Effect/Mage_effect1"));
            AllTextures.Add(Content.Load<Texture2D>("Effect/Mage_effect2"));
            AllTextures.Add(Content.Load<Texture2D>("Effect/arrow"));
            AllTextures.Add(Content.Load<Texture2D>("Effect/Bullet1"));
            AllTextures.Add(Content.Load<Texture2D>("Effect/Bullet2"));
            return AllTextures;
        }

        private void deadTimer(GameTime gameTime)
        {
            
            float totalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalSecond -= totalTime;

            var ss = Convert.ToInt32(totalSecond % 60).ToString("00");
            var mm = (Math.Floor(totalSecond / 60) % 60).ToString("00");
            var hh = Math.Floor(totalSecond / 60 / 60).ToString("00");

            hungerText = mm + ":" + ss; 

            if (totalSecond < 0)
            {
                _player._playerM.Life = 0;
                totalSecond = hungerTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            deadTimer(gameTime);

            _player.Update(gameTime);
            //Board.KillMonster();
            foreach (Monster monster in _board.Monsters) monster.Update(gameTime);
           
            foreach (Tile tile in _board.Tile2) tile.Update(gameTime);
            //foreach (Preacher preacher in _board.Preacher) preacher.Update(gameTime);
            _board.Update(gameTime);
            CheckKeyboardAndReact();
          
            _background = new Sprite(mapTextures[_board.StageNumber - 1, 0], new Vector2(60, 60), _spriteBatch);
        }

        internal void PlaySound(int type)
        {
            if (type == 1) _sound.Play(0.2f, 0, 0);
            if (type == 2) _tpsound.Play(0.4f, 0, 0);
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
            Inventory.ShowInventory(_player,_debugFont);
            _board.TakeItem();
            if (_player._playerM.lvlup)_spriteBatch.Draw(Board.CurrentBoard.LvlUpTexture, new Vector2(250, 250), Color.White);
            _player.Draw();
            _player.HeartsDrawing(heartTexture);

            WriteDebugInformation();
            
            _spriteBatch.End();

        }
		
        private void WriteDebugInformation()
        {
          //string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", _player.Position.X, _player.Position.Y);
          //string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.Mouvement.X, _player.Mouvement.Y);

            string lifeInText = string.Format("Life: ({0:0})", _player._playerM.Life);
            string experienceInText = string.Format("Experience: ({0:0} / {1:0})", _player._playerM.Experience,_player._playerM.Xpnext);
            string moneyInText = string.Format("Money: ({0:0})", _player._playerM.Money);

            string RoomNumberInText = string.Format("Room Number: ({0:0})", Board.CurrentBoard.RoomNumber);
            string StageNumberInText = string.Format("Stage Number: ({0:0})", Board.CurrentBoard.StageNumber);

            string SpecialInText = string.Format("Special Number: ({0:0})", Board.CurrentBoard.Special);
            string SpecialTypeInText = string.Format("Special Type: ({0:0})", Board.CurrentBoard.SpecialType);

            string monsterDeadInText = string.Format("Monsters Dead: ({0:0})", _board.MonsterDead);
            string LvlUpInText = string.Format("             Level Up ! \r\n {0:0} just reached Lvl. {1:0}", _player._playerM.Name, _player._playerM.Level);

            string hungerTimeInText = string.Format("Death by Hunger in: ({0})", hungerText);

            //DrawWithShadow(positionInText, new Vector2(10, 0));
            //DrawWithShadow(movementInText, new Vector2(10, 20));

            DrawWithShadow(lifeInText, new Vector2(250, 20));
            DrawWithShadow(experienceInText, new Vector2(10, 20));
            DrawWithShadow(moneyInText, new Vector2(400, 20));
            if (_player._playerM.lvlup) DrawWithShadow(LvlUpInText, new Vector2(320, 270));

            // DrawWithShadow(RoomNumberInText, new Vector2(280, 280));
            // DrawWithShadow(StageNumberInText, new Vector2(320, 320));

            // DrawWithShadow(SpecialInText, new Vector2(360, 360));
            //  DrawWithShadow(SpecialTypeInText, new Vector2(400, 400));

            // DrawWithShadow(monsterDeadInText, new Vector2(440, 440));

            DrawWithShadow(hungerTimeInText, new Vector2(10, 600));

            //DrawWithShadow("F5 for random board", new Vector2(70, 600));
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
