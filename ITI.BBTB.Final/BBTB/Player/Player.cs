using BBTB.AnimationManager;
using BBTB.Items;
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
        new Texture2D Texture { get; set; }
        internal string _classes { get; set; }
        God _god;
        #endregion
        public PlayerInventory Inventory;
        public string PlayerClasse { get; set; }
        public List<Texture2D> _bulletTextures;
        public Texture2D _weaponTexture;
        public bool IsDead;
        SpriteBatch _spriteBatch;
        public Animation animation;

        public Player(Texture2D texture, Vector2 position, SpriteBatch spritebatch, GameState ctx, Weapon weapon, bool havePrayed, PlayerInventory inventory, List<Texture2D> BulletTextures, string _classe)
            : base(texture, position, spritebatch)
        {
            PlayerClasse = _classe;
            _playerM = new PlayerModel("Tanguy", PlayerClasse);
            Inventory = new PlayerInventory(GameState._itemTexture, _spriteBatch, this);
            _ctx = ctx;
            _booltime = false;
            _weapon = weapon;
            _havePrayed = havePrayed;
            _bulletTextures = BulletTextures;
            Position = position;
            animation = new Animation(texture, position, spritebatch, 9, 4);
            Position = new Vector2(animation.SourceRect.X, animation.SourceRect.Y);
            _weapon = new Weapon(_ctx, _bulletTextures[1], Position, spritebatch, this, _bulletTextures);
            _spriteBatch = spritebatch;
        }

        #region propriété 
        public bool HavePrayed { get { return _havePrayed; } set { _havePrayed = value; } }
        public string Classes { get { return _classes; } set { _classes = value; } }
        public int WeaponType => _weapon.WeaponType;

        public Weapon Weapon => _weapon;

        public GameState Ctx => _ctx;
        #endregion

        private void PlayerDead()
        {
            if (_playerM.Life <= 0)
            {
                _playerM.Life = _playerM._lifemax;
                Inventory.ItemByDefault();
                _ctx.totalSecond = _ctx.hungerTime;
                Board.CurrentBoard.Stage1();
                IsDead = true;
            }
        }

        public void HeartsDrawing(Texture2D _heartTexture)
        {
            int heartPositionx = 900;
            int heartPositiony = 10;
            heartNumber = (_playerM.Life / 50);
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
            Position = Vector2.One * 128;
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
            PlayerDead();
            CheckKeyboardAndUpdateMovement(gameTime);
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            if (_weapon != null) { _weapon.Update(gameTime); }
            UsePotion();
            HasTouchedMonster();
            _playerM.LvlUpdate();
        }

        private void UsePotion()
        {
            int _time = 15;
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.P))
            {
                if (Inventory._potionNb > 0)
                {
                    if (_time == 15)
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
                                Inventory.AddEmptyPotion();
                            }
                        }
                    }
                    else _time++;
                }
            }
        }

        private void CheckKeyboardAndUpdateMovement(GameTime gameTime)
        {
            int _time = 0;
            KeyboardState keyboardState = Keyboard.GetState();
            
            Console.WriteLine(_time);

            if (keyboardState.IsKeyDown(Keys.Q)) {_mouvement -= Vector2.UnitX*2; animation.AnimateLeft(gameTime);}
            if (keyboardState.IsKeyDown(Keys.D)) { _mouvement += Vector2.UnitX*2; animation.AnimateRight(gameTime);}

            if (keyboardState.IsKeyDown(Keys.S)) { _mouvement += Vector2.UnitY*2; animation.AnimateDown(gameTime);}
            if (keyboardState.IsKeyDown(Keys.Z)) { _mouvement -= Vector2.UnitY*2; animation.AnimateUp(gameTime);}

            _booltime = keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space) ||
                        keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space);


            if (_time <= 0 && _booltime == true)
            {
                if (keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.Space))
                {
                    _ctx.PlaySound(2);
                    _mouvement -= new Vector2(0, 30);
                }
                else if (keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.Space))
                {
                    _ctx.PlaySound(2);
                    _mouvement += new Vector2(0, 30);
                }
                else if (keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.Space))
                {
                    _ctx.PlaySound(2);
                    _mouvement -= new Vector2(30, 0);
                }
                else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.Space))
                {
                    _ctx.PlaySound(2);
                    _mouvement += new Vector2(30, 0);
                }

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
            int _time = 0;
            foreach (Monster monster in Board.CurrentBoard.Monsters)
            {
                if (monster.IsAlive)
                {
                    if (new Rectangle((int)Position.X, (int)Position.Y, animation.spriteWidth, animation.spriteHeight).Intersects(monster.Bounds) && _time == 60)
                    {
                        _playerM.Life -= monster._attack/10;
                        _time = 1;
                        return true;
                    } else if (_time < 60) _time++; 
                }
            }
            return false;
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, animation.SourceRect);
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
            animation.Draw(Position);
            if (_weapon != null) _weapon.Draw();
        }
    }
}