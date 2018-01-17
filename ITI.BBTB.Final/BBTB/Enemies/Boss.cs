using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTB.Enemies
{
    public class Boss : Monster
    {
        
        public bool  _isAlive { get; set; }
        int _life;
        int _xp;
        
        public Boss(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive) : base(texture, position, batch, isAlive)
        {
            _isAlive = isAlive;
            _life = 10000;
            _xp = 100;
       
        }

        public new int Life { get { return _life; } set { _life = value; } }
        public int Xp { get { return _xp; } set { _xp = value; } }

        public new void Update(GameTime gameTime)
        {
            IsDead();
        }

        public new void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            //if (IsDead()) prévenir le jeu pour gagner l'expérience
        }

        public new bool IsDead()
        {
            if (_life <= 0)
            {
                IsAlive = false;
                return true;
            }
            return false;
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
