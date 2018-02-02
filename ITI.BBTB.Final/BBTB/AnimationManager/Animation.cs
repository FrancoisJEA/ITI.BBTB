using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BBTB.AnimationManager
{
    public class Animation :Sprite 
    {
        SpriteBatch spriteBatch;
        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 0;
        public int spriteWidth;
        public int spriteHeight;
        int spriteSpeed = 2;
        int currentAnimation = 0;
       
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;
        Point frameSize;
        public int totalFrame;

        public Vector2 Position{get { return position; } set { position = value; }}
        public Vector2 Origin{ get { return origin; }set { origin = value; }}
        public Texture2D Texture{get { return spriteTexture; } set { spriteTexture = value; }}
        public Rectangle SourceRect{ get { return sourceRect; } set { sourceRect = value; } }
        public int Rows { get; set; }
        public int Columns { get; set; }


        KeyboardState currentKBState;
        KeyboardState previousKBState;


        public Animation(Texture2D texture, Vector2 position, SpriteBatch spritebatch, int columns, int rows) : base(texture, position, spritebatch)
        {
  
            this.spriteTexture = texture;
            Rows = rows;
            Columns = columns;
            totalFrame = rows * columns;
            this.spriteBatch = spritebatch;
      
        }


        public void HandleSpriteMovement(GameTime gameTime)

        {

            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
 
    
            if (currentKBState.GetPressedKeys().Length == 0)
            {
             

                if (currentKBState.IsKeyDown(Keys.D) == true)
                {
                    AnimateRight(gameTime);

                }



                if (currentKBState.IsKeyDown(Keys.Q) == true)

                {

                    AnimateLeft(gameTime);

                }



                if (currentKBState.IsKeyDown(Keys.S) == true)

                {

                    AnimateDown(gameTime);

                }

                if (currentKBState.IsKeyDown(Keys.Z) == true)

                {

                    AnimateUp(gameTime);



                }
               if (currentKBState.IsKeyDown(Keys.None) == true )
                {
                    currentFrame = 0;
                }

                
            }
        }




        public void AnimateRight(GameTime gameTime)
        {

            if (currentKBState != previousKBState)
                currentFrame  = 0;
                currentAnimation = 3;


            timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;


                if (timer > interval)
                {
                    currentFrame++;

                    if (currentFrame > 8)
                    {
                        currentFrame = 0;
                    }
                timer = 0f;
                }
        }

        public void AnimateLeft(GameTime gameTime)
        {

            if (currentKBState != previousKBState)
                currentFrame = 0;
            currentAnimation = 1;


            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 8)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateUp(GameTime gameTime)
        {

            if (currentKBState != previousKBState)
                currentFrame = 0;
            currentAnimation = 0;


            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 8)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {

            if (currentKBState != previousKBState)
                currentFrame = 0;
            currentAnimation = 2;


            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 8)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void Draw(Vector2 location)
        {
            spriteWidth = Texture.Width / Columns;
            spriteHeight = Texture.Height / Rows;
            int row = (int)((float)currentAnimation % Rows);
            int colums = currentFrame % Columns;

            sourceRect = new Rectangle(spriteWidth * colums, spriteHeight * row, spriteWidth, spriteHeight);
            Rectangle destinationRect = new Rectangle((int)location.X, (int)location.Y, spriteWidth, spriteHeight);
            spriteBatch.Draw(Texture, destinationRect, sourceRect, Color.White);

        }
    }
}
