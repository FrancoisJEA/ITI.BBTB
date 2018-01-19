﻿using Microsoft.Xna.Framework;
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
        int _life;
        public Item _item;
		int _xp;
        Vector2 _mouvement;
		Vector2 _position;
        Bullet _bullet;
        Monster _monster;
        PlayerInventory PlayerInventory = new PlayerInventory();

        GameState _ctx; // Paramètre du constructeur
        public List<Texture2D> _itemTexture { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture)
            : base(texture, position, batch)
        {
            _mouvement = Vector2.Zero;
            _position = position;
            _itemTexture = itemTexture;
            _life = 100;
			_xp = 10;
        }

        public int Life { get { return _life; } set { _life = value; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

        public void Update(GameTime gameTime)
        {
            if (IsDead == true)
            {
                Board.CurrentBoard.Monsters.Remove(_bullet._monster);
            }

        }

        public void Idle()
        {
            foreach (Monster monsters in Board.CurrentBoard.Monsters)
            {
                Patroling();
            }
        }

        public void Patroling()
        {
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                if 
                   (
                        !monster.Bounds.Intersects(Board.CurrentBoard.TileTexture.Bounds) 
                        || !monster.Bounds.Intersects(Board.CurrentBoard.TileTexture2.Bounds)
                        || !monster.Bounds.Intersects(Board.CurrentBoard.TileTexture3.Bounds) 
                   )
                {
                    if (monster.Position.X > 0)
                    {
                        _mouvement += Vector2.UnitX * 2;
                    }
                    else if ( monster.Position.X <= 0)
                    {
                        _mouvement -= Vector2.UnitX * 2;
                    }
                }
            }
        }
        
        public void Shooting()
        {

        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += _mouvement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        public void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            _monster = bullet._monster;
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
