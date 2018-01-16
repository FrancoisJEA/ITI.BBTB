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

namespace BBTB
{
    public class Monster : Sprite
    {
        public bool IsAlive { get { return _life >= 0; } }

        int _life;
        public Item _item;
		int _xp;
		Player _player;
        public int _monsterDead;
		Vector2 _position;
        PlayerInventory PlayerInventory = new PlayerInventory();

        GameState _ctx; // Paramètre du constructeur
        public List<Texture2D> _itemTexture { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture)
            : base(texture, position, batch)
        {
			_position = position;
            _itemTexture = itemTexture;

            _life = 100;
			_xp = 10;
            _monsterDead = 0;
        }

        public int MonsterDead { get { return _monsterDead; } set { _monsterDead = value; } }
        public int Life { get { return _life; } set { _life = value; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

        public void Update(GameTime gameTime)
        {
            IsDead();
        }

        public void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            //if (IsDead()) prévenir le jeu pour gagner l'expérience
        }

        public bool IsDead()
        {
            if (_life <= 0)
            {

               _monsterDead++;
                return true;
            }
            return false;
        }
        
        public void DropItem ()
        {
            Random Random = new Random();
            int ItemNb = _itemTexture.Count - 1;
            int ItemID = Random.Next(0, ItemNb);

            Texture2D ItemTexture = PlayerInventory.FoundTextureByID(ItemID, _itemTexture);
            _item = new Item(new Vector2(this.Position.X, this.Position.Y), ItemTexture, SpriteBatch);
            _item.Draw();
        }

        public override void Draw()
        {
            if (IsAlive)
            {
                base.Draw();
            }
        }

    }
}
