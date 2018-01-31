using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BBTB.States;
using BBTB.Enemies;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Content;
using BBTB.Items;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace BBTB
{
    public class Board
    {
		Tile[,] _tiles; //
		Tile[,] _tile2; //
		Tile[,] _tile3; //
		Tile[,] _tile4; //
        Tile[,] _tile5; //
        Tile[,] _tile6; //
        Tile[,] _tile7; //

        public Texture2D[,] mapTextures;
       
        public List<Preacher> Preacher { get; set; }

        public Player _player;
		int _stageNumber;
		int _roomInFloor;
		int _roomNumber;
        int _special;
        int _specialType;
        int _monsterDead;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D ChestTexture { get; set; }

        private Texture2D ChestTexture2;

        public Texture2D TileTexture { get; set; }
        public Texture2D TileTexture2 { get; set; }
        public Texture2D TileTexture3 { get; set; }
        public Texture2D TileTexture4 { get; set; }
        public Texture2D TileTexture5 { get; set; }
        public Texture2D TileTexture6 { get; set; }
        public Texture2D TileTexture7 { get; set; }

        public Texture2D MonsterTexture { get; set; }
        public Texture2D BossTexture { get; set; }
        public Texture2D PreacherTexture { get; set; }
        public Texture2D LvlUpTexture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        private int _time;
        internal Boss _boss;
        public bool _lvlup;
        Vector2 _bossPosition = new Vector2(300,400);
        public GameTime GameTime;
        float _time2 = 5;
        float Timer = 5;

        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }
        public List<Bullet> Bullets { get; }
        public List<Monster> Monsters;
        public List<Texture2D> ItemTexture { get; set; }
        private Texture2D _traderTexture;
        readonly GameState _gameState;
        private List<Item> items = new List<Item>(); // All items in the ground 
        PlayerInventory Inventory;
		BinaryFormatter _f;
        private SpriteFont _debugFont;
        bool _chestState;
        Monster m;
        private Texture2D Goblin;
        Trader Trader;
        bool Shop;
        


        public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D tileTexture2, Texture2D tileTexture3, Texture2D tileTexture4, Texture2D tileTexture5, Texture2D tileTexture6, Texture2D tileTexture7, Texture2D chestTexture,Texture2D chestTexture2, Texture2D monsterTexture,Texture2D[,] MapTextures,Texture2D TraderTexture, Texture2D preacherTexture, Texture2D bossTexture, int columns, int rows, Player player, GameState gameState, List<Texture2D> itemTexture,SpriteFont debugFont,Texture2D lvluptexture)
	    {
            LvlUpTexture = lvluptexture;
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            TileTexture2 = tileTexture2;
            TileTexture3 = tileTexture3;
            TileTexture4 = tileTexture4;
            TileTexture5 = tileTexture5;
            TileTexture6 = tileTexture6;
            TileTexture7 = tileTexture7;

            ChestTexture = chestTexture;
            ChestTexture2 = chestTexture2;
            BossTexture = bossTexture;
			MonsterTexture = monsterTexture;
            PreacherTexture = preacherTexture;
            _traderTexture = TraderTexture;
            _time = 15;
            _chestState = false;
            ItemTexture = itemTexture;
			SpriteBatch = spritebatch;
            Preacher = new List<Preacher>();
       
            _debugFont = debugFont;
            mapTextures = MapTextures;

            _tiles = new Tile[Columns, Rows];
            _tile2 = new Tile[Columns, Rows];
            _tile3 = new Tile[Columns, Rows];
            _tile4 = new Tile[Columns, Rows];
            _tile5 = new Tile[Columns, Rows];
            _tile6 = new Tile[Columns, Rows];
            _tile7 = new Tile[Columns, Rows];

            _boss = new Boss(BossTexture, _bossPosition, SpriteBatch, false, itemTexture);
            Board.CurrentBoard = this;
			Bullets = new List<Bullet>();
			_player = player;
            Inventory = _player.Inventory;
            _gameState = gameState;
			Stage1();
            _monsterDead = 0;
            Shop = Special == _roomNumber && SpecialType == 4;
        }
		
		#region Prorpiétés

        public int StageNumber { get { return _stageNumber; } set { _stageNumber = value; } }
		public int RoomInFloor { get { return _roomInFloor; } set { _roomInFloor = value; } }
		public int RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }
		public int Special { get { return _special; } set { _special = value; } }
        public int SpecialType { get { return _specialType; } }
		public BinaryFormatter Formatter { get { return _f; } }
        public int MonsterDead { get { return _monsterDead; } set { _monsterDead = value; } }

		public Tile[,] Tile { get { return _tiles; } set { _tiles = value; } }
		public Tile[,] Tile2 { get { return _tile2; } set { _tile2 = value; } }
		public Tile[,] Tile3 { get { return _tile3; } set { _tile3 = value; } }
		public Tile[,] Tile4 { get { return _tile4; } set { _tile4 = value; } }
        public Tile[,] Tile5 { get { return _tile5; } set { _tile5 = value; } }
        public Tile[,] Tile6 { get { return _tile6; } set { _tile6 = value; } }
        public Tile[,] Tile7 { get { return _tile7; } set { _tile7 = value; } }

        #endregion

        public void KillMonster()
        {
            List<Monster> monsterToRemove = new List<Monster>();

            for (int x = 0; x < Monsters.Count; x++)
            {

                if (Monsters[x].IsAlive == false)
                {

                    Item Item = Monsters[x].DropItem();
                    Monsters[x].WhenMonsterDie(_player);
                    monsterToRemove.Add(Monsters[x]);
                    items.Add(Item);
                }
            }

            foreach (Monster m in monsterToRemove) Monsters.Remove(m);
        }

        private void SpikesHit()
        {
            foreach (var tile in Tile7)
            {
                int timeHit = 40;
                if (tile.IsBlocked && tile.Bounds.Intersects(_player.Bounds) && timeHit == 40)
                {
                    _player._playerM.Life -= 1;
                    timeHit = 0;
                }
                timeHit++;
            }
        }

        public void TakeItem ()
        {
            string Text = "";
            int _charac = 0;
            string _type = "";
            for (int y = 0; y < items.Count; y++)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if (items[y] != null)
                {
                    //float DistanceX;
                    //float DistanceY;

                    //if (_player.Position.X > items[y]._position.X) { DistanceX = _player.Position.X - items[y]._position.X; }
                    //else { DistanceX = items[y]._position.X - _player.Position.X; }

                    //if (_player.Position.X > items[y]._position.X) { DistanceY = _player.Position.Y - items[y]._position.Y; }
                   // else { DistanceY = items[y]._position.Y - _player.Position.Y; }
                   



                    //if (DistanceX <= 10 && DistanceY <= 10)
                    if (_player.Bounds.Intersects(items[y].Bounds))
                    {
                        if (items[y].ItemClasse == _player.PlayerClasse || items[y].ItemClasse == "All")
                        {
                            float X = items[y]._position.X + 25;
                            float Y = items[y]._position.Y;
                            if (X< 70) X += 40;
                            else if (X > 800) X -= Inventory.BoxTexture.Bounds.Width + 25;
                            if (Y < 70) Y += items[y].Bounds.Height;
                            else if (Y > 800) Y -= Inventory.BoxTexture.Bounds.Height;
                            Sprite Box = new Sprite(Inventory.BoxTexture, new Vector2(X, Y), SpriteBatch);
                            Box.Draw();

                            if (items[y].ItemType == "weapon") { _type = "Damages:";  _charac = items[y].Attack; }
                            else if (items[y].ItemType == "armor") { _type = "Armor:"; _charac = items[y].Defense; }
                            else if (items[y].Name == "Heal potion") { _type = "Recover HP: +"; _charac = 30; }
                            else if (items[y].Name == "Used potion") { _type = "This potion has already been \r\n used by his former owner. \r\n            Money +"; _charac = 20; }

                            if (items[y]._specialItem == true)
                            {
                                Text = string.Format("\r\n          {0:0} \r\n      Special Item !\r\n {4:0}: {5:0}\r\nStrength +{1:0} Intelligence +{2:0} Agility +{3:0}\r\n\r\n          Press F to equip \r\n ", items[y].Name, items[y]._strength, items[y]._intelligence, items[y]._agility, _type, _charac);
                            }
                            else if (items[y]._specialItem == false)
                            { 
                                Text = string.Format("\r\n          {0:0} \r\n           {1:0} {2:0}\r\n \r\n             Press F to equip", items[y].Name, _type, _charac);
                            }
                            
                            DrawWithShadow(Text, new Vector2(X + 10, Y + 15));

                            if ((keyboardState.IsKeyDown(Keys.F)))
                            {
                                if (_time < 10) { _time += 1; }
                                else
                                {
                                    List<Item> _item = Inventory.AddItemToInventory(items[y], items, _player,y);
                                    items = _item;
                                    _time = 1;
                                }
                            }
                        }
                        else
                        {
                            Sprite Box = new Sprite(Inventory.BoxTexture2, new Vector2(items[y]._position.X-100, items[y]._position.Y - 100), SpriteBatch);
                            Box.Draw();
                            Text = string.Format("\r\n    {1:1} \r\n \r\nThis Item can only \r\nbe equipped by {0:0}", items[y].ItemClasse,items[y].Name);
                            DrawWithShadow(Text, new Vector2(Box.Position.X+45, Box.Position.Y+15));
                        }
                    }
                }
            }            
        }

        private void PlayerDead()
        {
            if (_player._playerM.Life <= 0)
            {
                _player._playerM.Life = _player._playerM._lifemax;
                _player.Inventory.ItemByDefault(_player);
                Stage1();
                _player.IsDead = true;
            }
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
 
            SpriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            SpriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);

        }

        public void CreateNewBoard()
			/*  Types= 1:chest 2:god 3:save 4:Shop */
        {
            
            items = new List<Item>();
            if (_boss.AddBoss())
                SetBossTileUnblocked();
            if (_special != _roomNumber)
			{
                AddMonsters();
                if (_roomNumber == _roomInFloor)
                    _boss.AddBoss();
                
                BlockSomeTilesRandomly();
				SetStairs();
                Tile3[13, 1].IsBlocked = true;
                SetUpChestInTheMiddle();
                SetUpShop();
                SetTorches();
                SetSpikes();
            }
            else if (Special == _roomNumber && SpecialType == 1)
			{
				Tile4[5, 4].IsBlocked = true;
                Tile4[7, 6].IsBlocked = true;
			}
            else if (Special == _roomNumber && SpecialType == 2)
			{
				AddPreacher();
                 //SetSanctuary();
            }
            else if (Special == _roomNumber && SpecialType == 3)
            {
                
					/*string Saves = Path.GetTempFileName();
					var Hero = _player._playerM;
				    var Map = CurrentBoard;
					_f = new BinaryFormatter();
					using (var stream = File.OpenWrite("Content/Saves/Saves"))
					{
						_f.Serialize(stream, Hero);
					    //_f.Serialize(stream, Map);
					}*/
				
            }
            else if (Shop)
            {
                //Tile5[6, 4].IsBlocked = true;
                Tile5[3, 3].IsBlocked = true;
                Trader = new Trader(_traderTexture,new Vector2(400,400),SpriteBatch, ItemTexture);
                items = Trader.ItemsToSell(items);

            }
			SetAllBorderTilesBlocked();
			SetTopLeftTileUnblocked();
         
			_player.ResetPosition();
		}
        
        public void IsMonsterDead()
        {
            foreach (Monster monster in Monsters)
                if (monster.Life <= 0)  _monsterDead++;
        }

		public void Stage1()
        {
            _roomInFloor = _rnd.Next(4, 7);
            _stageNumber = 1;
            _roomNumber = 1;
            _special = _rnd.Next(2, _roomInFloor);
            _specialType = _rnd.Next(1, 4);
            _boss._life = 5000;
            CreateNewBoard();
        }
        
        public void NewRoom()
        {
            bool showExist = true;
            foreach (Monster monster in Monsters)
            {
                if (monster.IsAlive && _roomNumber < _roomInFloor)
                {
                    showExist = false;
                    break;
                }

                if (_roomNumber == _roomInFloor && _boss.AddBoss() == true)
                {
                    showExist = false;
                    break;    
                }
            }
            if (showExist == true)
            {
                Tile3[13, 1].Texture = TileTexture3;
            } else if (showExist == false)
            {
                Tile3[13, 1].Texture = TileTexture4;
            }

            if (_roomNumber < _roomInFloor)
            {
                if (showExist == true &&_player.Bounds.Intersects(Tile3[13, 1].Bounds))
                {
					_roomNumber++;
                    Shop = Special == _roomNumber && SpecialType == 4;
                    CreateNewBoard();
                }
            }
        }
        public void OpenChest()
        {
            Rectangle r = Tile4[5, 4].Bounds;
            r.Inflate(30, 30);
            if (_player.Bounds.Intersects(r) && _chestState == false && SpecialType == 1)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if ((keyboardState.IsKeyDown(Keys.F)))
                {
                    int X = r.X;
                    int Y = r.Y;
                    Random rng = new Random();
                    int nbItem = rng.Next(1, 3);
                    for (int w = 0; w < nbItem; w++)
                    {
                         X = rng.Next(Convert.ToInt32(Tile4[5,4].Position.X - 128), Convert.ToInt32(Tile4[5, 4].Position.X + 128));
                         Y = rng.Next(Convert.ToInt32(Tile4[5, 4].Position.Y - 128), Convert.ToInt32(Tile4[5, 4].Position.Y + 128));
                        int i = rng.Next(1, ItemTexture.Count);
                        items.Add(new Item(new Vector2(X, Y), ItemTexture[i], SpriteBatch, _player));
                    }
                    _player._playerM.Money += rng.Next(10 * StageNumber * (1 + _player._playerM.Intelligence / 10), 20 * StageNumber * (1 + _player._playerM.Intelligence / 10));
                         _chestState = true;
                    Tile4[5, 4].Texture = ChestTexture2;
                }
            }
        }
        
        public void NewStage()
        {
            if(_roomNumber == _roomInFloor && _player.Bounds.Intersects(Tile3[13, 1].Bounds))
            {
                CreateNewBoard();
                _roomInFloor = _rnd.Next(4, 7);
                _stageNumber = _stageNumber + 1;
                _roomNumber = 1;
				_special = _rnd.Next(2, _roomInFloor);
                _specialType = 4;//_rnd.Next(1, 4);
               // Shop = Special == _roomNumber && SpecialType == 4;
               if (StageNumber != 1)
                {
                    _boss._life = 5000 * StageNumber * _player._playerM.Strength;
                }
                
            }
        }

		private void SetUpChestInTheMiddle()
		{
			for (int x = 0; x < Columns; x++)
			{
				for (int y = 0; y < Rows; y++)
				{
					Vector2 tilePosition = new Vector2(x * ChestTexture.Width, y * ChestTexture.Height);
					Tile4[x, y] = new Tile(ChestTexture, tilePosition, SpriteBatch, false);
				}
			}
		}

        private void SetUpShop()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x+64, y * TileTexture4.Height); //a modifier
                    Tile5[x, y] = new Tile(TileTexture5, tilePosition, SpriteBatch, false);
                }
            }
        }

        private void SetTorches()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture6.Width, y * TileTexture6.Height);
                    Tile6[x, y] = new Tile(TileTexture6, tilePosition, SpriteBatch, false);

                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { if (_rnd.Next(4, 20) == 4) Tile6[x, y].IsBlocked = true; }
                }
            }
        }

        private void SetTopLeftTileUnblocked()
        {
            if (Shop)
            {
                Tile2[3, 3].IsBlocked = false;
                Tile2[4, 3].IsBlocked = false;
                Tile2[5, 3].IsBlocked = false;
                Tile2[6, 3].IsBlocked = false;
                Tile2[6, 3].IsBlocked = false;
                Tile2[7, 3].IsBlocked = false;
                Tile2[8, 3].IsBlocked = false;
            }

            Tile7[1, 1].IsBlocked = false;
            Tile2[1, 1].IsBlocked = false;
            Tile2[2, 1].IsBlocked = false;
            Tile2[1, 2].IsBlocked = false;
            Tile2[2, 2].IsBlocked = false;
            Tile2[10, 1].IsBlocked = false;
            Tile2[10, 2].IsBlocked = false;

            Tile2[9, 1].IsBlocked = false;
            Tile2[10, 1].IsBlocked = false;
            Tile2[9, 2].IsBlocked = false;
            Tile2[10, 2].IsBlocked = false;

            Tile7[9, 1].IsBlocked = false;
            Tile7[10, 1].IsBlocked = false;
            Tile7[9, 2].IsBlocked = false;
            Tile7[10, 2].IsBlocked = false;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    foreach (Monster monster in Monsters)
                    {
                        if (monster.Position.X == x * 64 && monster.Position.Y == y * 64)
                        {
                            Tile2[x, y].IsBlocked = false;
                            Tile7[x, y].IsBlocked = false;
                        }
                    }

                if (Tile2[x, y].IsBlocked.Equals(Tile7[x, y].IsBlocked)) Tile7[x, y].IsBlocked = false;
                }
            }
        }

        private void SetBossTileUnblocked()
        {
            
            Tile2[6, 4].IsBlocked = false;
            Tile2[7, 4].IsBlocked = false;
            Tile2[8, 4].IsBlocked = false;
            Tile2[6, 5].IsBlocked = false;
            Tile2[7, 5].IsBlocked = false;
            Tile2[8, 5].IsBlocked = false;
            Tile2[6, 6].IsBlocked = false;
            Tile2[7, 6].IsBlocked = false;
            Tile2[8, 6].IsBlocked = false;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (Tile2[x,y].IsBlocked.Equals(_boss.Position))
                    {
                        Tile2[x, y].IsBlocked = false;
                        _boss.IsAlive = false;
                    }
                }
            }
        }

        internal void CreateBullet(Texture2D bulletTexture, Vector2 position, SpriteBatch spriteBatch, WeaponLib weaponLib)
        {
            Bullets.Add(new Bullet(bulletTexture, position, spriteBatch, weaponLib, this, _player._weapon));
        }

		// Add Monster in the board (if its not a special room)
        private void AddMonsters()
        {
            Monsters = new List<Monster>();
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    //Vector2 monsterPosition = new Vector2(x * MonsterTexture.Width, y * MonsterTexture.Height);
                    //Monsters[x, y] = new Monster(MonsterTexture, monsterPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false,this.ItemTexture);

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Vector2 monsterPosition = new Vector2(x * MonsterTexture.Width, y * MonsterTexture.Height);
                            Monsters.Add(new Monster(MonsterTexture, monsterPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false, this.ItemTexture));
                        }
                    }
                }
            }
        }
    
		// Add preachers in the room (if its a special room and a god one)
		private void AddPreacher()
		{
			for (int x = 0; x < Columns; x++)
			{
				for (int y = 0; y < Rows; y++)
				{
					if (x > 0 && x < 14 && y > 0 && y < 9)
					{
						if (_rnd.Next(4, 20) == 4)
						{
                            Vector2 preacherPosition = new Vector2(x * MonsterTexture.Width, y * MonsterTexture.Height);
                            Monsters.Add(new Monster(PreacherTexture, preacherPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false, ItemTexture));
						}
					}
				}
			}
		}

        internal void OnBulletDestroy(Bullet bullet)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullet b = Bullets[i];
                Bullets.RemoveAt(i--);
            }
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tile[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, false);

                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { Tile[x, y].IsBlocked = true; }
                }
            }
        }

        private void BlockSomeTilesRandomly()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture2.Width, y * TileTexture2.Height);
                    Tile2[x, y] = new Tile(TileTexture2, tilePosition, SpriteBatch, false);

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Tile2[x, y].IsBlocked = true;
                        }
                    }
                }
            }

            SelectTexture();
        }

        private void SetSpikes()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture7.Width, y * TileTexture7.Height);
                    Tile7[x, y] = new Tile(TileTexture7, tilePosition, SpriteBatch, false);

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Tile7[x, y].IsBlocked = true;
                        }
                    }
                }
            }
        }

        private void SetStairs() // donne la position aux escaliers
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture3.Width, y * TileTexture3.Height);
                    Tile3[x, y] = new Tile(TileTexture3, tilePosition, SpriteBatch, false);
                }
            }
            Tile3[13, 1].IsBlocked = true;
        }

        private void SelectTexture()
        {
            Random rng = new Random();
            int i;
            foreach (Tile tile2 in Tile2)
            {
                i = rng.Next(0, mapTextures.GetLength(1)+2);
                tile2.Texture = mapTextures[i, 3];
            }
        }

        public void Draw()
        {
            _boss.Draw();

            foreach (var tile in Tile)
            {
                if (StageNumber > 0) tile.Texture = mapTextures[StageNumber - 1, 1];
                tile.Draw();
            }

            foreach (var tile2 in Tile2)
            {
                tile2.Draw();
            }

            foreach (var tile3 in Tile3)
            {
                tile3.Draw();
            }

			foreach (var tile4 in Tile4)
			{
				tile4.Draw();
			}

            foreach (var tile5 in Tile5)
            {
                tile5.Draw();
            }

            foreach (var tile6 in Tile6)
            {
                tile6.Draw();
            }

            foreach (var tile7 in Tile7)
            {
                tile7.Draw();
            }

            foreach (var monster in Monsters)
            {
                if (StageNumber > 0) monster.Texture = mapTextures[StageNumber - 1, 2];
                monster.Draw();
            }

            if (Trader != null && Special == _roomNumber && SpecialType == 4) Trader.Draw();

            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }

            foreach(var item in items)
            {
                item?.Draw();
            }

            if (_player.IsDead == true) {
                float elapsed = (float)Board.CurrentBoard.GameTime.ElapsedGameTime.TotalSeconds;
                _time2 -= elapsed;
                SpriteBatch.Draw(LvlUpTexture, new Vector2(250, 250), Color.AliceBlue);
                DrawWithShadow("                        Game Over\r\n  Don't worry, you stil have your stats", new Vector2(270, 270));
                if (_time2 < 0)
                {
                    _time2 = Timer;
                    _player.IsDead = false;
                }
                    
            }
        }

        internal void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            NewRoom();
            NewStage();
            BulletUpdate(gameTime);
            PlayerDead();
            OpenChest();
            SpikesHit();
        }

        private void BulletUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullet b = Bullets[i];
                if (b.BulletLib.IsDead() || b.TouchEnemy() || b.HasTouchedTile())
                {
                    Bullets.RemoveAt(i--);
                }
                else
                {
                    b.Update(gameTime);
                }

                _boss.Update(gameTime);
            }
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var tile in Tile)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }

            foreach (var tile in Tile2)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }

            foreach (var tile in Tile4)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }
            return wrapper.FurthestAvailableLocationSoFar;
        }
    }
}