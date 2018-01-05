using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public Sprite(Texture2D texture, Vector2 position, SpriteBatch batch)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = batch;
        }
        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}