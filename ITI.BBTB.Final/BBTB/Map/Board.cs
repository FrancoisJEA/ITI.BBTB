using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB
{
    public class Board
    {
        public Tile[,] Tiles { get; set; }
        public Monster[,] Monsters { get; set; }
        public Player player;
        int _stageNumber;
        int _roomInFloor;
        int _roomNumber;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D TileTexture { get; set; }
        public Texture2D MonsterTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, Texture2D monsterTexture, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            MonsterTexture = monsterTexture;
            SpriteBatch = spritebatch;
            Monsters = new Monster[Columns, Rows];
            Tiles = new Tile[Columns, Rows];
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public int StageNumber { get { return _stageNumber; } set{ _stageNumber = value; } }

        public void CreateNewBoard()
        {
            SetAllBorderTilesBlockedAndSomeRandomly();
            AddMonsters();
            //InitializeAllTilesAndBlockSomeRandomly();
            SetTopLeftTileUnblocked();
        }

        public void Stage1()
        {
            _roomInFloor = _rnd.Next(4, 7);
            _stageNumber = 1;
            _roomNumber = 1;
        }
        /*
        public Vector2 TPtile()
        {
            if(TPtileUp())
            {

            }
        }
        */

        public bool TPtileUp()
        {
            foreach (Monster monster in Monsters)
            {
                if (!monster.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }

        public void NewStage()
        {
            if(_roomNumber == _roomInFloor /*&& player.position == TPtile()*/ )
            {
                CreateNewBoard();
                _roomInFloor = _rnd.Next(4, 7);
                _stageNumber = _stageNumber + 1;
            }
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
            Monsters[1, 1].IsAlive = false;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (Tiles[x, y].IsBlocked.Equals(Monsters[x, y].IsAlive))
                    {
                        Tiles[x, y].IsBlocked = false;
                        Monsters[x, y].IsAlive = false;
                    }
                }
            }
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

                    if (x > 0 && x < 14 && y > 0 && y < 9)
                    {
                        if (_rnd.Next(4, 20) == 4)
                        {
                            Tiles[x, y].IsBlocked = true;
                        }
                    }

                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { Tiles[x, y].IsBlocked = true; }
                    //else Tiles[x, y].IsBlocked = false;
                }
            }
        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }

            foreach (var monster in Monsters)
            {
                monster.Draw();
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