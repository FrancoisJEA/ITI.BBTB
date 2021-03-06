﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Items
{
    public class Item : Sprite
    {


        public Vector2 _position;
        public Texture2D _texture;
        public SpriteBatch SpriteBatch;
        public string Name { get; }
        public string ItemType { get; set; }
        
        //public enum category
        public string ItemClasse { get; set; }
        public int Attack { get; set; }
        private Player _player;
        public int _agility;
        public int _strength;
        public int _intelligence;
        public bool _specialItem;
        public int Defense;
        public int _price;
        public bool ToSell;



        public int InventoryEmplacement { get; }



        public Item(Vector2 position, Texture2D texture, SpriteBatch spriteBatch,Player player) : base(texture,position,spriteBatch)   
        {
            _position = position;
            _texture = texture;
            SpriteBatch = spriteBatch;
            Name = texture.Name.Replace("_"," ");
            Name = Name.Replace("Items/","");
            InventoryEmplacement = this.FindInventoryEmplacement(Name);
            _player = player;
            ItemClasse = FindClasse();
            if (ItemType == "weapon")
            {
                Attack = 20 * Convert.ToInt32(_player._playerM.Level * 1.8 * _player._playerM.Strength / 5);
            }
            if (ItemType == "armor")
            {
                Defense = 20 + Convert.ToInt32(_player._playerM.Level * 1.8 * _player._playerM.Strength / 5);
            }
            RandomStat();

        }

        private void RandomStat()
        {
            Random random = new Random();
            int prob = _player._playerM.Intelligence/10 *4;
            int RNG = random.Next(prob, 100);
            if (Name != "Heal potion" && Name != "Used potion")
            {
                if (RNG > 60)
                {
                    string _playerclasse = _player._playerM._classe;
                    if (_playerclasse == "Wizard")
                    {
                        _specialItem = true;
                        _strength = _player._playerM.Level * 2;
                        _intelligence = _player._playerM.Level * 3;
                        _agility = _player._playerM.Level * 2;
                    }
                    else if (_playerclasse == "Gunner")
                    {
                        _specialItem = true;
                        _strength = _player._playerM.Level * 3;
                        _intelligence = _player._playerM.Level * 2;
                        _agility = _player._playerM.Level * 2;
                    }
                    else if (_playerclasse == "Archer")
                    {
                        _specialItem = true;
                        _strength = _player._playerM.Level * 2;
                        _intelligence = _player._playerM.Level * 2;
                        _agility = _player._playerM.Level * 3;
                    }
                }
                else
                {
                    _strength = 0;
                    _intelligence = 0;
                    _agility = 0;
                    _specialItem = false;
                }
            }
        }

        private string FindClasse()
        {
            if(Name.Contains("bow") || (Name.Contains("crossbow"))) return "Archer";
            if(Name.Contains("gun") || (Name.Contains("rifle"))) return "Gunner";
            if (Name.Contains("staff") || (Name.Contains("book"))) return "Wizard";
            else return "All";
        }

  
     
        internal int FindInventoryEmplacement (string Name)
        {

            if (Name.Contains("helmet")) { ItemType = "armor"; return 0; }
            if (Name.Contains("armor"))  { ItemType = "armor"; return 1; }
            if (Name.Contains("boots"))  { ItemType = "armor"; return 3; }
            if (Name.Contains("gloves")) { ItemType = "armor"; return 2; }
            if (Name.Contains("bow") || Name.Contains("staff") || Name.Contains("gun")) {  ItemType = "weapon"; return 4; }
            if (Name.Contains("crossbow") || Name.Contains("book") || Name.Contains("sword")) { ItemType = "weapon"; return 5; }
            if (Name.Contains("potion")) { ItemType = "Potion"; return 6; }

            else return 6;

        }

        public void Draw ()
        {
            SpriteBatch?.Draw(_texture, _position, Color.White);
        }

        public Texture2D FoundTextureByID(int ItemID, List<Texture2D> ItemTextures)
        {
            Texture2D itemTexture = ItemTextures.ElementAt(ItemID);
            return itemTexture;
        }
        public List<Texture2D> DefineBulletTexture(List<Texture2D> BulletTextures,string classe)
        {
            List<Texture2D> Btextures = new List<Texture2D>();

            if (classe == "Wizard")
            {
                Texture2D BulletTexture = BulletTextures.ElementAt(0);
                Btextures.Add(BulletTexture);
                Texture2D BulletTexture2 = BulletTextures.ElementAt(1);
                Btextures.Add(BulletTexture2);
                return Btextures;
            }
            if (classe == "Archer")
            {
                Texture2D BulletTexture = BulletTextures.ElementAt(2);
                Btextures.Add(BulletTexture);
                Texture2D BulletTexture2 = BulletTextures.ElementAt(2);
                Btextures.Add(BulletTexture2);
                return Btextures;
            }
            if (classe == "Gunner")
            {
                Texture2D BulletTexture = BulletTextures.ElementAt(3);
                Btextures.Add(BulletTexture);
                Texture2D BulletTexture2 = BulletTextures.ElementAt(4);
                Btextures.Add(BulletTexture2);
                return Btextures;
            }
            else return Btextures;

        }

    }
}
