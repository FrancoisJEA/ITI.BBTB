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
    public class Animation : Sprite
    {
        Texture2D _testureAnimation;
        KeyboardState _key;
        float Speed;
        Vector2 Position;
        Point CurrentFrame;
        Point frameSize;
        XmlDocument xmlDoc;
        int width;
        int x;
        int y;

        string Awidth;
        string Ax;
        string Ay;
        enum State
        {
            Walking,
            Dead,
            Idle
        }

        TimeSpan nextFrameInterval = TimeSpan.FromSeconds((float)1 / 10);
        TimeSpan nextFrame;

 
        public Animation(Texture2D animationSprite, Vector2 position, SpriteBatch batch, KeyboardState currentKey, string Path) : base(animationSprite, position, batch)
        {
            Position = position;
            _testureAnimation = animationSprite;
            xmlDoc.Load("Content/WalkCycle");
            XmlNodeList animations = xmlDoc.SelectNodes(Path);
            XmlNodeList speed = xmlDoc.SelectNodes("delay");

            foreach (XmlNode animation in xmlDoc.GetElementsByTagName(Path))
            {
                Awidth = animation.Attributes["w"].Value;
                width = Int32.Parse(Awidth);

                Ax = animation.Attributes["x"].Value;
                x = Int32.Parse(Ax);

                Ay = animation.Attributes["y"].Value;
                y = Int32.Parse(Ay);
                frameSize = new Point(width);
                CurrentFrame = new Point(x, y);
                
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        new public void Draw()
        {
            SpriteBatch.Draw(_testureAnimation, Position, new Rectangle(CurrentFrame.X * frameSize.X, CurrentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                             Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
