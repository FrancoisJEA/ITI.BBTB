﻿using BBTB.Items;
using BBTB.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BBTB
{
    public class Player : Sprite
    {
        int heartNumber;

        #region Champs
        readonly GameState _ctx;
        public Weapon _weapon;
        public PlayerModel _playerM;
        Vector2 _mouvement;

        int _time;
        bool _booltime;
        private Vector2 oldPosition;
        bool _havePrayed;

        internal string _classes { get; set; }
        God _god;
        #endregion
        public PlayerInventory Inventory;
        public string PlayerClasse { get; set; }
        public List<Texture2D> _bulletTextures;
        public Texture2D _weaponTexture;
        int _time2;

        public Player(Texture2D texture, Vector2 position, SpriteBatch spritebatch, GameState ctx, Weapon weapon, bool havePrayed, PlayerInventory inventory, List<Texture2D> BulletTextures, string _classe)
            : base(texture, position, spritebatch)
        {
            PlayerClasse = _classe;
            _playerM = new PlayerModel("Tanguy", PlayerClasse);
            _ctx = ctx;
            _time = 0;
            _time2 = 0;
            _booltime = false;
            _weapon = weapon;
            Inventory = inventory;
            Inventory.ItemByDefault(this);
            _havePrayed = havePrayed;
            _bulletTextures = BulletTextures;
            _weapon = new Weapon(_ctx, _bulletTextures[1], Position, spritebatch, this, _bulletTextures);
        }

        #region propriété 
        public bool HavePrayed { get { return _havePrayed; } set { _havePrayed = value; } }
        public string Classes { get { return _classes; } set { _classes = value; } }
        public int WeaponType => _weapon.WeaponType;

        public Weapon Weapon => _weapon;

        public GameState Ctx => _ctx;
        #endregion

        public void HeartsDrawing(Texture2D _heartTexture)
        {
            int heartPositionx = 900;
            int heartPositiony = 10;
            heartNumber = (_playerM.Life / 25);
            for (int i = 0; i < heartNumber; i++)
            {
                 heartPositionx -= 40;

                Sprite heart = new Sprite(_heartTexture, new Vector2(heartPositionx, heartPositiony), SpriteBatch);
                heart.Draw();

            }
            if (_playerM.Life < 25 && _playerM.Life > 0)
            {
                Sprite heart = new Sprite(_heartTexture, new Vector2(heartPositionx, heartPositiony), SpriteBatch);
                heart.Draw();
            }
        }

        public void ResetPosition()
        {
            Position = Vector2.One * 70;
            _mouvement = Vector2.Zero;
        }

        public Texture2D FindPlayerTexture(List<Texture2D> t)
        {
            if (this.PlayerClasse == "Gunner") return t[0];
            if (this.PlayerClasse == "Archer") return t[1];
            if (this.PlayerClasse == "Wizard") return t[2];
            else return t[2];
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            if (_weapon != null) { _weapon.Update(gameTime); }
            UsePotion();
            HasTouchedMonster();
        }
        private void UsePotion()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _time = 60;
            if (keyboardState.IsKeyDown(Keys.P)) {
                
                if (Inventory._potionNb > 0)
                {
                    if (_time == 60)
                    {
                        int x = 0;
                        if (_playerM.Life < _playerM._lifemax) {
                            while (_playerM.Life < _playerM._lifemax && x < 30) 
                            {
                                _playerM.Life++;
                                x++;
                                _time = 0;
                            }
                            Inventory._potionNb--;
                            if (Inventory._potionNb == 0)
                            {
                                Inventory.AddEmptyPotion(this);
                            }
                        }
                    }
                    else _time++;
                }
            }
        }
        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Console.WriteLine(_time);

            if (keyboardState.IsKeyDown(Keys.Q)) { _mouvement -= Vector2.UnitX*2; }
            if (keyboardState.IsKeyDown(Keys.D)) { _mouvement += Vector2.UnitX*2; }

            if (keyboardState.IsKeyDown(Keys.S)) { _mouvement += Vector2.UnitY*2; }
            if (keyboardState.IsKeyDown(Keys.Z)) { _mouvement -= Vector2.UnitY*2; }

            _booltime = keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(0, 20);
                else if (keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space)) _mouvement -= new Vector2(20, 0);
                else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space)) _mouvement += new Vector2(20, 0);

                _time = 300;
                _booltime = false;
            }
            else
            {
                _time -= 1;
            }
        }

        public bool HasTouchedMonster()
        {
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                if (monster.IsAlive)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height).Intersects(monster.Bounds) && _time2 == 120)
                    {
                        _playerM.Life -= monster._attack/10;
                        _time2 = 1;
                        return true;
                    } else if (_time2 < 120) _time2++; 
                }
            }
            return false;
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds);
        }

        private void SimulateFriction()
        {
            _mouvement -= _mouvement * Vector2.One * 0.25f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += _mouvement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { _mouvement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { _mouvement *= Vector2.UnitX; }
        }

        public override void Draw()
        {
            base.Draw();
            if (_weapon != null) _weapon.Draw();
        }
    }
}