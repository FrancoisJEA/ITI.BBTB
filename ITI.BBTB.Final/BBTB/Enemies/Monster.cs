using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBTB.States;
using Microsoft.Xna.Framework.Input;
using BBTB.Items;
using System.Diagnostics;

namespace BBTB
{
    public class Monster : Sprite
    {
        public bool IsAlive { get { return _life >= 0; } }
        public bool IsDead { get; set; }
        readonly Weapon _weapon;

        int _life;
        public Item _item;

        Vector2 newPosition;
        Bullet _bullet;
        PlayerInventory PlayerInventory;
        Monster _monster;
        int Type;
        public int _attack;
        int _level;
        int _xp;
        bool _reflect; // If monster return damage
        bool _goblin;  // If monster Give money when hit
        public List<Texture2D> _itemTexture { get; set; }
        int _money;

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture)//,PlayerInventory Inventory)
            : base(texture, position, batch)
        {


            _itemTexture = itemTexture;
            //PlayerInventory = Inventory;
            if (Board.CurrentBoard == null) Type = 0;
            else Type = Board.CurrentBoard.StageNumber; 
            DefineMonster(Type);
        }

        private void DefineMonster(int t)
        {
            _attack = 15;
            _life = 100;
            _level = 1;
            _xp = 10;
            _money = 2;
            Random r = new Random();
            int rng = r.Next(1, 500);

            for(int x=0; x<t; x++)
            {
                _attack *= 3;
                _life *= 2;
                _level ++;
                _xp *=2;
                _money *= 2;
                if (t == 2) _reflect = true;
                else _reflect = false;
              //  if (rng > 490)
                //{  _goblin = true; this.Texture = }
            }
        }
        public void Reflect (PlayerModel p)
        {
            if (_reflect == true) p.Life -= _attack/ 100;
        }

        public Weapon Weapon => _weapon;
        public int Life { get { return _life; } set { } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

        public void Update(GameTime gameTime)
        {
            //IsDead();
            UpdatePositionBasedOnMovement(gameTime);

        }
        public void WhenMonsterDie(Player p)
        {
            p._playerM.Money += _money;
            p._playerM.Experience +=_xp;
            p._playerM.LevelUp();
        }

        public void Idle()
        {

            Patroling(this);


        }

        public void Patroling(Monster monsters)
        {
            if (TouchTile(monsters) == true)
            {
                float distanceX = monsters.Position.X - Board.CurrentBoard._player.Position.X;
                if (monsters.Position.X < 824 && monsters.Position.X > 64)
                {
                    if (distanceX > 0)
                    {

                        newPosition.X--;

                    }
                    if (distanceX < 0)
                    {

                        newPosition.X++;

                    }
                }
                else newPosition.X = -newPosition.X;


                float distanceY = monsters.Position.Y - Board.CurrentBoard._player.Position.Y;
                if (monsters.Position.Y < 516 && monsters.Position.Y > 64)
                {
                    if (distanceX > 0)
                    {

                        newPosition.Y--;

                    }

                    if (distanceX < 0)
                    {

                        newPosition.Y++;

                    }
                }
                else newPosition.Y = -newPosition.Y;
            }
            else newPosition = -newPosition;
        
        }



        public bool TouchTile(Monster monster)
        {
            foreach (Tile tile in Board.CurrentBoard.Tile2)
            {
                if (new Rectangle((int)monster.Position.X, (int)monster.Position.Y, monster.Bounds.Width, monster.Bounds.Height).Intersects(tile.Bounds))
                    return true;
                
            }
            return false;
        }

        public bool Shooting(Monster monsters)
        {
            if (monsters.Position.Y > Board.CurrentBoard._player.Position.Y * 0.9)
                return true;
            else
                return false;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {

            Position += newPosition * (float)gameTime.ElapsedGameTime.TotalMinutes * 10;
            Idle();
        }


        public void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            
           // _monster = bullet._monster;
        }

        public Item DropItem()
        {
            Random Random = new Random();
            int ItemNb = _itemTexture.Count - 1;
            int ItemID = Random.Next(1, ItemNb);
            int Intelligence = Board.CurrentBoard._player._playerM.Intelligence / 10 * 4;
            int dropProb = Random.Next(Intelligence, 100);
            if (dropProb >= 85)
            {
                Texture2D ItemTexture = Board.CurrentBoard._player.Inventory.FoundTextureByID(ItemID, _itemTexture);
                _item = new Item(new Vector2(this.Position.X, this.Position.Y), ItemTexture, SpriteBatch, Board.CurrentBoard._player);
            }
            return _item;
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
