﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BBTB.States;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BBTB
{
	[Serializable]
	public class Board
	{
		#region Champs
		[NonSerialized]
		Tile[,] _tiles;
		[NonSerialized]
		Tile[,] _tile2;
		[NonSerialized]
		Tile[,] _tile3;
		[NonSerialized]
		Tile[,]_tiles4;
		[NonSerialized]
		Monster[,] _monster;
		[NonSerialized]
		Preacher[,] _preacher;
		[NonSerialized]
		public Player _player;
		int _stageNumber;
		int _roomInFloor;
		int _roomNumber;
		int _specialType;
		[NonSerialized]
		int _special;
		[NonSerialized]
		int _columns;
		[NonSerialized]
		int _rows;
		[NonSerialized]
		Texture2D _chestTexture, _tileTexture, _tileTexture2, _tileTexture3, _monsterTexture, _preacherTexture;
		[NonSerialized]
		SpriteBatch _spriteBatch;
		[NonSerialized]
		private Random _rnd = new Random();
		[NonSerialized]
		static Board _currentBoard;
		[NonSerialized]
		List<Bullet> _bullet;
		[NonSerialized]
		readonly GameState _gameState;
		// [NonSerialized]
		// BinaryFormatter f;
		#endregion

		public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D tileTexture2, Texture2D tileTexture3, Texture2D chestTexture, Texture2D monsterTexture, int columns, int rows, Player player, GameState gameState)
		{
			Rows = rows;
			TileTexture = tileTexture;
			TileTexture2 = tileTexture2;
			TileTexture3 = tileTexture3;
			ChestTexture = chestTexture;
			MonsterTexture = monsterTexture;
			SpriteBatch = spritebatch;
			Monsters = new Monster[Columns, Rows];

			Tiles = new Tile[Columns, Rows];
			Tiles2 = new Tile[Columns, Rows];
			Tiles3 = new Tile[Columns, Rows];
			Tile4 = new Tile[Columns, Rows];

			Board.CurrentBoard = this;
			Bullets = new List<Bullet>();
			
			_player = player;
			_gameState = gameState;
			Stage1();
		}
		
		#region Prorpiétés
		// public BinaryFormatter F { get { return f; } }
		public int StageNumber { get { return _stageNumber; } set { _stageNumber = value; } }
		public int RoomInFloor { get { return _roomInFloor; } set { _roomInFloor = value; } }
		public int RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }
		public int Special { get { return _special; } set { _special = value; } }
		public int SpecialType { get { return _specialType; } }
		public Tile[,] Tiles { get { return _tiles; } set { _tiles = value; } }
		public Tile[,] Tiles2 { get { return _tile2; }  set {_tile2 = value ;} }
		public Tile[,] Tiles3 { get { return _tile3; } set { _tile3 = value; } }
		public Tile[,] Tile4 { get {return _tiles4; } set {_tiles4 = value; } }
		public Monster[,] Monsters { get { return _monster; } set { _monster = value; } }
		public Preacher[,] Preacher { get {return _preacher; } set {_preacher = value; } }
		public int Columns { get {return _columns; } set {_columns = value; } }
		public int Rows { get {return _rows; } set {_rows = value; } }
		public Texture2D ChestTexture { get {return _chestTexture; } set {_chestTexture = value; } }
		public Texture2D TileTexture { get {return _tileTexture; } set {_tileTexture = value; } }
		public Texture2D TileTexture2 { get {return _tileTexture2; } set {_tileTexture2= value; } }
		public Texture2D TileTexture3 { get {return _tileTexture3; } set {_tileTexture3=value; } }
		public Texture2D MonsterTexture { get { return _monsterTexture; } set { _monsterTexture = value; } }
		public Texture2D PreacherTexture { get { return _preacherTexture; } set { _preacherTexture = value; } }
		private SpriteBatch SpriteBatch { get { return _spriteBatch; } set { _spriteBatch = value; } }
		List<Bullet> Bullets { get { return _bullet; } set { _bullet = value; } }
		public static Board CurrentBoard { get { return _currentBoard; } private set { _currentBoard = value; } }
		#endregion

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
				/*
					string Saves = Path.GetTempFileName();
					var Hero = _player._playerM;
				    var Map = _currentBoard;
					f = new BinaryFormatter();
					using (var stream = File.OpenWrite("Content/Saves/Saves"))
					{
						f.Serialize(stream, Hero);
					    f.Serialize(stream, Map);
					}
				*/
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
            Tiles3[13, 1].IsBlocked = showExist;

            if (_roomNumber < _roomInFloor)
            {
                if (showExist == true &&_player.Bounds.Intersects(Tiles3[13, 1].Bounds))
                {
					_roomNumber++;
					CreateNewBoard();
                }
            }
        }
        
        public void NewStage()
        {
            if(_roomNumber == _roomInFloor && _player.Bounds.Intersects(Tiles3[13, 1].Bounds))
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
            Tiles2[1, 1].IsBlocked = false;
            Monsters[1, 1].IsAlive = false;

            Monsters[13, 1].IsAlive = false;
            Tiles2[13, 1].IsBlocked = false;
            Tiles2[12, 1].IsBlocked = false;
            Tiles2[13, 2].IsBlocked = false;
            Tiles2[11, 1].IsBlocked = false;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (Tiles2[x, y].IsBlocked.Equals(Monsters[x, y].IsAlive))
                    {
                        Tiles2[x, y].IsBlocked = false;
                        Monsters[x, y].IsAlive = false;
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
                    Vector2 monsterPosition = new Vector2(x * MonsterTexture.Width, y * MonsterTexture.Height);
                    Monsters[x, y] = new Monster(MonsterTexture, monsterPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false);

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Monsters[x, y].IsAlive = true;
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
					Vector2 preacherPosition = new Vector2(x * MonsterTexture.Width, y * MonsterTexture.Height);
					Monsters[x, y] = new Monster(MonsterTexture, preacherPosition, SpriteBatch, /*_rnd.Next(5) == 0*/ false);

					if (x > 0 && x < 14 && y > 0 && y < 9)
					{
						if (_rnd.Next(4, 20) == 4)
						{
							Monsters[x, y].IsAlive = true;
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
        }*/

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, false);

                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { Tiles[x, y].IsBlocked = true; }
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
                    Tiles2[x, y] = new Tile(TileTexture2, tilePosition, SpriteBatch, false);

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Tiles2[x, y].IsBlocked = true;
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
                    Tiles3[x, y] = new Tile(TileTexture3, tilePosition, SpriteBatch, false);
                }
            }
        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }

            foreach (var tile2 in Tiles2)
            {
                tile2.Draw();
            }

            foreach (var tile3 in Tiles3)
            {
                tile3.Draw();
            }

			foreach (var tile4 in Tile4)
			{
				tile4.Draw();
			}

			foreach (var monster in Monsters)
            {
                monster.Draw();
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
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }

            foreach (var tile in Tiles2)
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