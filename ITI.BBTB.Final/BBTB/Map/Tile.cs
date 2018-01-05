using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BBTB
{
    public class Tile : Sprite
    {
        public bool IsBlocked { get; set; }

        public Tile(Texture2D texture, Vector2 position, SpriteBatch batch, bool isBlocked)
            : base(texture, position, batch)
        {
            IsBlocked = isBlocked;
        }

        public void Update(GameTime gameTime)
        {
            DestroyObjects();
        }

        public override void Draw()
        {
            if (IsBlocked)
            {
                base.Draw();
            }
        }

        internal void DestroyObjects()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.B)) { IsBlocked = false; }
        }

    }
}
