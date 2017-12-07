using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace BBTB
{
    public class BulletLib
    {
        Vector2 _direction;
        float _timer;
        float _lifeSpan = 1f;
        float _linearVelocity = 8f;
        Vector2 _position;
        int _height;
        int _width;
        //int _damage;

        public BulletLib(WeaponLib ctx, Vector2 position, int height, int width)
        {
            _direction = ctx.Direction;
            _position = position;
            _height = height;
            _width = width;
            //_damage = 10;
        }


        public void Timer(float gameTime)
        {
            _timer += gameTime;
        }
        public bool IsDead()
        {
            return _timer > _lifeSpan;
        }
        public Vector2 PositionUpdate()
        {
            return _position = _direction * _linearVelocity;
        }

    }
}
