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

        readonly Weapon _weapon;

        int _life;
        public Item _item;

        public List<Texture2D> _itemTexture { get; set; }

        public Monster(GameState ctx, Texture2D texture, Texture2D weaponTexture, Texture2D bulletTexture, Texture2D weaponTexture2, Texture2D bulletTexture2, Weapon weapon, Vector2 position, SpriteBatch batch, List<Texture2D> itemTexture)
            : base(texture, position, batch)
        {
            _weapon = weapon;
            _weapon = new Weapon(ctx, weaponTexture, bulletTexture, weaponTexture2, bulletTexture2, position, batch, null);

            _itemTexture = itemTexture;
            _life = 100;
        }

        public Weapon Weapon => _weapon;
        public int Life { get { return _life; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

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
        
        /*public void DropItem ()
        {
            Random Random = new Random();
            int ItemNb = _itemTexture.Count - 1;
            int ItemID = Random.Next(0, ItemNb);

            Texture2D ItemTexture = PlayerInventory.FoundTextureByID(ItemID, _itemTexture);
            _item = new Item(new Vector2(this.Position.X, this.Position.Y), ItemTexture, SpriteBatch);
            _item.Draw();
        }*/

        public override void Draw()
        {
            if (IsAlive)
            {
                base.Draw();
                _weapon.Draw();
            }
        }

    }
}
