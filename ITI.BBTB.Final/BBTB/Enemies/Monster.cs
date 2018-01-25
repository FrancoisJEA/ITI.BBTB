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
        readonly Weapon _weapon;

        int _life;
        public Item _item;
        Vector2 _mouvement;
        Bullet _bullet;
        PlayerInventory PlayerInventory;
        Monster _monster;

        public List<Texture2D> _itemTexture { get; set; }

        public Monster(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture,PlayerInventory Inventory)
            : base(texture, position, batch)
        {
            _mouvement = Vector2.Zero;

            _itemTexture = itemTexture;
            PlayerInventory = Inventory;
            _life = 100;
        }

        public Weapon Weapon => _weapon;
        public int Life { get { return _life; } }
		// public Vector2 Position { get { return _position; } set { _position = value; } }

        public void Update(GameTime gameTime)
        {
            //IsDead();

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
        }

        public Item DropItem()
        {
            Random Random = new Random();
            int ItemNb = _itemTexture.Count - 1;
            int ItemID = Random.Next(1, ItemNb);
            int dropProb = Random.Next(0, 100);
            if (dropProb >= 10)
            {
                Texture2D ItemTexture = PlayerInventory.FoundTextureByID(ItemID, _itemTexture);
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
