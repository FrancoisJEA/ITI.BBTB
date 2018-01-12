using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
    public class WeaponLib
    {
        public Vector2 Distance { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Direction { get; private set; }

        public WeaponLib()
        {
        }

        public void Update(float X, float Y)
        {
            Distance = new Vector2(X, Y);
            Rotation = RotationSet(Distance);
            Direction = DirectionSet(Rotation);
        }

        float RotationSet(Vector2 distance)
        {
            return (float)Math.Atan2(distance.Y, distance.X);
        }

        Vector2 DirectionSet(float rotation)
        {
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }
    }
}
