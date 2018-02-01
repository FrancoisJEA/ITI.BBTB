using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BBTB.Items;

namespace BBTB.Enemies
{
    public class Boss : Monster
    {

        public new bool IsAlive { get { return _life >= 0; }  set { } }
     
        internal int _life;
        int _xp;
		Texture2D _monsterBullettexture;
        
        public Boss(Texture2D texture, Vector2 position, SpriteBatch batch, bool isAlive,List<Texture2D> itemTexture,SpriteFont debugFont) : base(texture, position, batch, isAlive, itemTexture,debugFont)
			: base(texture,bulletTexture, position, batch, isAlive, itemTexture)
        {
           
            _life = 5000;
            _xp = 100;
          
        }

        public new int Life { get { return _life; } set { _life = value; } }
        public int Xp { get { return _xp; } set { _xp = value; } }


        public new void Hit(Bullet bullet)
        {
            _life -= bullet.Damages;
            //if (IsDead()) prévenir le jeu pour gagner l'expérience
        }

        public bool AddBoss()
        {
            if (IsAlive && Board.CurrentBoard.RoomInFloor == Board.CurrentBoard.RoomNumber)
            {
                return true;
            }
            else return false;
        }

  
        public override void Draw()
        {
           
            if (AddBoss() )
            {
                base.Draw();
            }
        }


    }
}
