using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BBTB.States;

namespace BBTB
{
    public class Board
    {
        public Tile[,] Tiles { get; set; }
        public Tile[,] Tiles2 { get; set; }
        public Monster[,] Monsters { get; set; }
        public Player _player;
        int _stageNumber;
        int _roomInFloor;
        int _roomNumber;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D TileTexture { get; set; }
        public Texture2D TileTexture2 { get; set; }
        public Texture2D MonsterTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }
        List<Bullet> Bullets { get; }

        readonly GameState _gameState;


        public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D tileTexture2, Texture2D monsterTexture, int columns, int rows, Player player, GameState gameState)
        {
            if (gameState == null) throw new ArgumentNullException(nameof(gameState));
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            TileTexture2 = tileTexture2;
            MonsterTexture = monsterTexture;
            SpriteBatch = spritebatch;
            Monsters = new Monster[Columns, Rows];
            Tiles = new Tile[Columns, Rows];
            Tiles2 = new Tile[Columns, Rows];
            //CreateNewBoard();
            Board.CurrentBoard = this;
            Bullets = new List<Bullet>();

            _player = player;
            _gameState = gameState;
            Stage1();
        }

        public int StageNumber { get { return _stageNumber; } set{ _stageNumber = value; } }
        public int RoomInFloor { get { return _roomInFloor; } set { _roomInFloor = value; } }
        public int RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }

        public void CreateNewBoard()
        {
            SetAllBorderTilesBlockedAndSomeRandomly();
            AddMonsters();
            BlockSomeTilesRandomly();
            SetTopLeftTileUnblocked();
            _gameState.PutJumperInTopLeftCorner();
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
            Tiles2[13, 1].IsBlocked = showExist;

            if (_roomNumber < _roomInFloor)
            {
                if (_player.Bounds.Intersects(Tiles2[13, 1].Bounds))
                {
                    CreateNewBoard();
                    _roomNumber++;
                }
            }
        }
        
        public void NewStage()
        {
            if(_roomNumber == _roomInFloor && _player.Bounds.Intersects(Tiles2[13, 1].Bounds))
            {
                CreateNewBoard();
                _roomInFloor = _rnd.Next(4, 7);
                _stageNumber = _stageNumber + 1;
                _roomNumber = 1;
            }
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles2[1, 1].IsBlocked = false;

            Monsters[13, 1].IsAlive = false;
            Tiles2[13, 1].IsBlocked = false;
            Tiles2[12, 1].IsBlocked = false;
            Tiles2[13, 2].IsBlocked = false;
            Tiles2[11, 1].IsBlocked = false;

            Monsters[1, 1].IsAlive = false;

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
            Bullets.Add(new Bullet(bulletTexture, position, spriteBatch, weaponLib, this));
        }

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

        internal void OnBulletDestroy(Bullet bullet)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets.Remove(Bullets[i]);
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

        private void SetAllBorderTilesBlockedAndSomeRandomly()
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