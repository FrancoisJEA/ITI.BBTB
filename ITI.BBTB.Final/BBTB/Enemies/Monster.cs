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

        int _life;
        public Item _item;
		int _xp;
        PlayerInventory PlayerInventory = new PlayerInventory();

        GameState _ctx; // Paramètre du constructeur
        public List<Texture2D> _itemTexture { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture)
            : base(texture, position, batch)
        {
            _itemTexture = itemTexture;
            _life = 100;
			_xp = 10;
        }

        public int Life { get { return _life; } set { _life = value; } }

        public void Update(GameTime gameTime)
        {
        }

        public void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            if (_life <= 0)
            {

            } 
            //if (IsDead()) prévenir le jeu pour gagner l'expérience
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
