﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BBTB.States;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Content;
using BBTB.Items;

namespace BBTB
{
    public class Board
    {
		Tile[,] _tiles;
		Tile[,] _tile2;
		Tile[,] _tile3;
		Tile[,] _tile4;
		List<Monster> _monsters;
       
        public List<Preacher> Preacher { get; set; }

        public Player _player;
		int _stageNumber;
		int _roomInFloor;
		int _roomNumber;
        int _special;
        int _specialType;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D ChestTexture { get; set; }
        public Texture2D TileTexture { get; set; }
        public Texture2D TileTexture2 { get; set; }
        public Texture2D TileTexture3 { get; set; }
        public Texture2D MonsterTexture { get; set; }
        public Texture2D PreacherTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }
        List<Bullet> Bullets { get; }
        public List<Texture2D> ItemTexture { get; set; }
        readonly GameState _gameState;
		BinaryFormatter _f;

		public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D tileTexture2, Texture2D tileTexture3, Texture2D chestTexture, Texture2D monsterTexture, int columns, int rows, Player player, GameState gameState,List<Texture2D> itemTexture)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            TileTexture2 = tileTexture2;
            TileTexture3 = tileTexture3;
            ChestTexture = chestTexture;
			MonsterTexture = monsterTexture;
			SpriteBatch = spritebatch;
            Monsters = new List<Monster>();
            Preacher = new List<Preacher>();
            ItemTexture = itemTexture;

			Tile = new Tile[Columns, Rows];
			Tile2 = new Tile[Columns, Rows];
			Tile3 = new Tile[Columns, Rows];
            Tile4 = new Tile[Columns, Rows];

			Board.CurrentBoard = this;
			Bullets = new List<Bullet>();
			
			_player = player;
			_gameState = gameState;
			Stage1();
		}
		
		#region Prorpiétés
        public int StageNumber { get { return _stageNumber; } set { _stageNumber = value; } }
		public int RoomInFloor { get { return _roomInFloor; } set { _roomInFloor = value; } }
		public int RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }
		public int Special { get { return _special; } set { _special = value; } }
        public int SpecialType { get { return _specialType; } }
		public BinaryFormatter Formatter { get { return _f; } }
		public Tile[,] Tile { get { return _tiles; } set { _tiles = value; } }
		public Tile[,] Tile2 { get { return _tile2; } set { _tile2 = value; } }
		public Tile[,] Tile3 { get { return _tile3; } set { _tile3 = value; } }
		public Tile[,] Tile4 { get { return _tile4; } set { _tile4 = value; } }
		public List<Monster> Monsters { get { return _monsters; } set { _monsters = value; } }
		#endregion

		public void KillMonster()
        {
            for (int x = 0; x < Monsters.Count; x++) {

                if (Monsters[x].IsAlive == false)
                {
                   
                    Monsters[x].DropItem();
                    Monsters.RemoveAt(x--);
                }
            }
        }
		public void CreateNewBoard()
			/*  Types= 1:chest 2:god 3:save  */
        {
			if (_special != _roomNumber)
			{
				AddMonsters();
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
				
					string Saves = Path.GetTempFileName();
					var Hero = _player._playerM;
				    var Map = CurrentBoard;
					_f = new BinaryFormatter();
					using (var stream = File.OpenWrite("Content/Saves/Saves"))
					{
						_f.Serialize(stream, Hero);
					    //_f.Serialize(stream, Map);
					}
				
            }
			SetAllBorderTilesBlocked();
			SetTopLeftTileUnblocked();
			_player.ResetPosition();
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
                if (monster.IsAlive)
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
				//_specialType = _rnd.Next(1, 3);
				_specialType = 3;
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
                        if (monster.Position.X == x && monster.Position.Y == y)
                        {
                            Tile2[x, y].IsBlocked = false;
                        }
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
            foreach (var tile in Tile)
            {
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
                if (monster != null) monster.Draw();
            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }
        }

        internal void Update(GameTime gameTime)
        {
            NewRoom();
            NewStage();
            BulletUpdate(gameTime);
            KillMonster();
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