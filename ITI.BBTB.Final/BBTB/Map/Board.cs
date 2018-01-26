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
		Tile[,] _tiles;
		Tile[,] _tile2;
		Tile[,] _tile3;
		Tile[,] _tile4;
        public Texture2D[,] mapTextures;
       
        public List<Preacher> Preacher { get; set; }

        public Player _player;
        Weapon _weapon;
		int _stageNumber;
		int _roomInFloor;
		int _roomNumber;
        int _special;
        int _specialType;
        int _monsterDead;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D ChestTexture { get; set; }
        public Texture2D TileTexture { get; set; }
        public Texture2D TileTexture2 { get; set; }
        public Texture2D TileTexture3 { get; set; }
        public Texture2D MonsterTexture { get; set; }
        public Texture2D BossTexture { get; set; }
        public Texture2D PreacherTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private int _time;
        internal Boss _boss;
        Vector2 _bossPosition = new Vector2(300,400);

        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }
        public List<Bullet> Bullets { get; }
        public List<Monster> Monsters;
        public List<Texture2D> ItemTexture { get; set; }
        readonly GameState _gameState;
        private List<Item> items = new List<Item>();
        PlayerInventory Inventory;
		BinaryFormatter _f;
        private SpriteFont _debugFont;


        public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D tileTexture2, Texture2D tileTexture3, Texture2D chestTexture, Texture2D monsterTexture,Texture2D[,] MapTextures, Texture2D preacherTexture, Texture2D bossTexture, int columns, int rows, Player player, GameState gameState, List<Texture2D> itemTexture,SpriteFont debugFont, PlayerInventory inventory)
	    {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            TileTexture2 = tileTexture2;
            TileTexture3 = tileTexture3;
            ChestTexture = chestTexture;
            BossTexture = bossTexture;
			MonsterTexture = monsterTexture;
            PreacherTexture = preacherTexture;
            _time = 15;
           
            ItemTexture = itemTexture;
			SpriteBatch = spritebatch;
            Preacher = new List<Preacher>();
            Inventory = inventory;
            _debugFont = debugFont;
            mapTextures = MapTextures;
            _tiles = new Tile[Columns, Rows];
            _tile2 = new Tile[Columns, Rows];
            _tile3 = new Tile[Columns, Rows];
            _tile4 = new Tile[Columns, Rows];

            _boss = new Boss(BossTexture, _bossPosition, SpriteBatch, false, itemTexture,Inventory);
  
            Board.CurrentBoard = this;
			Bullets = new List<Bullet>();
			
			_player = player;
           

			_gameState = gameState;
			Stage1();

            _monsterDead = 0;
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

        public void TakeItem ()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            for (int y = 0; y < items.Count; y++)
            {
                if (items[y] != null)
                {
                    float DistanceX;
                    float DistanceY;

                    if (_player.Position.X > items[y]._position.X) { DistanceX = _player.Position.X - items[y]._position.X; }
                    else { DistanceX = items[y]._position.X - _player.Position.X; }

                    if (_player.Position.X > items[y]._position.X) { DistanceY = _player.Position.Y - items[y]._position.Y; }
                    else { DistanceY = items[y]._position.Y - _player.Position.Y; }
                    string Text = "";
               


                    if (DistanceX <= 10 && DistanceY <= 10)
                    {
                        if (items[y].ItemClasse == _player.PlayerClasse || items[y].ItemClasse == "All")
                        {
                            Sprite Box = new Sprite(Inventory.BoxTexture, new Vector2(items[y]._position.X, items[y]._position.Y), SpriteBatch);
                            Box.Draw();
                            if (items[y]._specialItem == true)
                            {
                                Text = string.Format(" {0:0} \r\n Special Item !\r\n Strength +{1:0}\r\n Intelligence +{2:0}\r\n Agility +{3:0} \r\n Press F to equip \r\n ", items[y].Name, items[y]._strength, items[y]._intelligence, items[y]._agility);
                            }
                            else if (items[y]._specialItem == false)
                            {
                                Text = string.Format(" {0:0} \r\n \r\n Press F to equip", items[y].Name);
                            }
                            DrawWithShadow(Text, new Vector2(items[y]._position.X + 40, items[y]._position.Y + 15));

                            if ((keyboardState.IsKeyDown(Keys.F)))
                            {
                                if (_time < 15) { _time += 1; }
                                else
                                {
                                    List<Item> _item = Inventory.AddItemToInventory(items[y], items, _player);
                                    items = _item;
                                    _time = 1;
                                }
                            }

                        } else
                        {
                            Sprite Box = new Sprite(Inventory.BoxTexture2, new Vector2(items[y]._position.X-100, items[y]._position.Y - 100), SpriteBatch);
                            Box.Draw();
                            Text = string.Format("{1:1} \r\n \r\nThis Item can only \r\n be equipped by {0:0}", items[y].ItemClasse,items[y].Name);
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
                _player._playerM.Life = 100;
                Stage1();
            }
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
 
            SpriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            SpriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);

        }
        public void CreateNewBoard()
			/*  Types= 1:chest 2:god 3:save  */
        {
            items = new List<Item>();
			if (_special != _roomNumber)
			{
                AddMonsters();
                if (_roomNumber == _roomInFloor)
                    _boss.AddBoss = true;

                BlockSomeTilesRandomly();
				SetStairs();
				SetUpChestInTheMiddle();
            }
            else if (Special == _roomNumber && SpecialType == 1)
			{
				Tile4[5, 4].IsBlocked = true;
			}
            else if (Special == _roomNumber && SpecialType == 2)
			{
				AddPreacher();
                // SetSanctuary();
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
			SetAllBorderTilesBlocked();
			SetTopLeftTileUnblocked();
            if (_boss.AddBoss == true)
                SetBossTileUnblocked();
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
                } else if (_roomNumber == _roomInFloor && monster.IsAlive && _boss.IsAlive)
                {
                    showExist = false;
                    break;    
                }
            }
            Tile3[13, 1].IsBlocked = showExist;

            if (_roomNumber < _roomInFloor)
            {
                if (showExist == true &&_player.Bounds.Intersects(Tile3[13, 1].Bounds))
                {
					_roomNumber++;
					CreateNewBoard();
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
				_specialType = _rnd.Next(1, 3);
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


        private void SetTopLeftTileUnblocked()
        {
            Tile2[1, 1].IsBlocked = false;
            //Monsters[1, 1].IsAlive = false;

            //Monsters[13, 1].IsAlive = false;
            Tile2[13, 1].IsBlocked = false;
            Tile2[12, 1].IsBlocked = false;
            Tile2[13, 2].IsBlocked = false;
            Tile2[11, 1].IsBlocked = false;

            for (int x = 0; x < Columns; x++)
            {
                
                for (int y = 0; y < Rows; y++)
                {
                    foreach (Monster monster in Monsters)
                    {
                        if (monster.Position.X == x * 64 && monster.Position.Y == y * 64)
                        {
                            Tile2[x, y].IsBlocked = false;
                        }
                    }
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
                            Monsters.Add(new Monster(MonsterTexture, monsterPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false, this.ItemTexture,Inventory));
                        }
                    }

                    /*for (int i = 0; i < 2; i++)
                    {
                    Monsters[_rnd.Next(1,10), _rnd.Next(1,6)].IsAlive = true;
                    Monsters[_rnd.Next(1,10), _rnd.Next(1,6)].IsAlive = true;
                    }*/
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
                            //Monsters.Add(new Monster(MonsterTexture, preacherPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false, ItemTexture));
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

        /*private void InitializeAllTilesAndBlockSomeRandomly()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, _rnd.Next(5) == 0);
                }
            }
        }
		*/

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

			foreach (var monster in Monsters)
            {
                if (StageNumber > 0) monster.Texture = mapTextures[StageNumber - 1, 2];
                monster.Draw();

            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }

            foreach(var item in items)
            {
                if (item != null) item.Draw();
            }
        }

        internal void Update(GameTime gameTime)
        {
            NewRoom();
            NewStage();
            BulletUpdate(gameTime);
            PlayerDead();
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