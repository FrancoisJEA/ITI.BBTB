using BBTB.Manage;
using BBTB.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Sprites
{
    class Sprites
    {
        #region Fields
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animation;

        protected Vector2 _position;

        protected Texture2D _texture;

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;

        public Vector2 Velocity;

        #endregion

        #region Method

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("this aint right...");
        }

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
            else _animationManager.Stop();
        }
        protected virtual void SetAnimation()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Right) || Keyboard.GetState().IsKeyDown(Input.Left) || Keyboard.GetState().IsKeyDown(Input.Up) || Keyboard.GetState().IsKeyDown(Input.Down))
            {
                if (Velocity.X > 0)

                    _animationManager.Play(_animation["RightWalk"]);

                else if (Velocity.X < 0)
                    _animationManager.Play(_animation["LeftWalk"]);
                else if (Velocity.Y < 0)
                    _animationManager.Play(_animation["FrontWalk"]);
                else if (Velocity.Y > 0)
                    _animationManager.Play(_animation["BackWalk"]);
            }
        }
        public Sprites(Dictionary<string, Animation> animation)
        {
            _animation = animation;
            _animationManager = new AnimationManager(_animation.First().Value);
        }
        public Sprites(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprites> sprites)
        {
            Move();

            SetAnimation();
            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion
    }
}
