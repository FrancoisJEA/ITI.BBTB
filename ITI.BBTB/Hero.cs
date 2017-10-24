﻿using System;

namespace ITI.BBTB
{
    public class Hero
    {
        int _life;
        int _xp;
        int _level;
        int _agility;
        int _strength;
        int _intelligence;
        int _resistance;

        public Hero(int life, int xp, int level, int agility, int strength, int intelligence, int resistance)
        {
            _life = life;
            _xp = xp;
            _level = level;
            _agility = agility;
            _strength = strength;
            _intelligence = intelligence;
            _resistance = resistance;
        }

        public int Life
        {
            get { return _life; }
        }

        public int Xp
        {
            get { return _xp; }
        }

        public int Agility
        {
            get { return _agility; }
        }

        public int Strength
        {
            get { return _strength; }
        }

        public int Intelligence
        {
            get { return _intelligence; }
        }

        public int Resistance
        {
            get { return _resistance; }
        }





    }
}
