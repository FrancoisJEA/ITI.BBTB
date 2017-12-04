using BBTB;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class ArmorPieceModel
    {
        string _name;
        int _life, _intelligence, _agility, _strength, _resistance;

        public bool IsCarried ()
        {
            return true;
        }

        public void StatBoost(bool isCarried, ArmorPieceModel Piece, PlayerModel player)
        {
            if (isCarried==true)
            {
                player.Agility = player.Agility + Piece._agility;
                player.Life = player.Life + Piece._life;
                player.Intelligence = player.Intelligence + Piece._intelligence;
                player.Resistance = player.Resistance + Piece._resistance;
                player.Strength = player.Strength + Piece._strength;
            }
        }
    }
}
