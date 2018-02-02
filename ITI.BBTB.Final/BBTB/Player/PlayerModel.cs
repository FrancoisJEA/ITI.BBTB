using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
    [Serializable]
    public class PlayerModel 
    {
        #region Champs
        public string _classe;
        /*Attributs*/
        int _life, _strength, _agility, _experience, _intelligence, _resistance;
        /*Inventory*/
        string _weapon1, _weapon2;
        string _helmet, _breastplate, _boots, _leggings;
        string _potiontype;
        int _krumbz;
       public int _lifemax;
        int _level;

		int _xpnext;
		int _skillsPoints;

		int _floor, _room, _type;
		public string _name;
        GameTime g;

		[NonSerialized]
        int _xplast;
		[NonSerialized]
		Board _board;
        float _time = 5;
        float Timer = 5;
       public bool lvlup;
#endregion

		public PlayerModel(string name,  string classe)
        {

            _name = name;
            _classe = classe;
            DefineStats();
            Level1SkillSetUp();
            _life = _lifemax;
        }

		#region Propriétés
		public int Life { get { return _life; } set { _life = value; } }
        public int Experience { get { return _experience; } set { _experience = value; } }
        public int Xpnext { get { return _xpnext; } set { _xpnext = value; } }
        public int Strength { get { return _strength; } set { _strength = value; } }
        public int Agility { get { return _agility; } set { _agility = value; } }
        public int Intelligence { get { return _intelligence; } set { _intelligence = value; } }
        public int Resistance { get { return _resistance; } set { _resistance = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Money { get { return _krumbz; } set { _krumbz = value; } }

        public int Level { get { return _level; ; } set { _level = value; } }

        public int SkillPoint { get { return _skillsPoints ; } set { _skillsPoints = value; } }
		public int Room {get {return _room; }}
		#endregion
        public void DefineStats()
        {
            if (_classe == "Wizard")
            {
                _lifemax = 1100;
                _strength = 20;
                _agility = 10;
                _intelligence = 30;

            }
            else if (_classe == "Gunner")
            {
                _lifemax = 130;
                _strength = 30;
                _agility = 20;
                _intelligence = 10;
            }
            else if (_classe == "Archer")
            {
                _lifemax = 100;
                _strength = 15;
                _agility = 25;
                _intelligence = 20;

            }
        }

        public void Level1SkillSetUp()
        {
            Level = 1;
            Experience = 0;
            Resistance = 10;
            Money = 2000;
            _xplast = 0;
            _xpnext = 100;

        }

        public void LevelUp()
        {
            while (_experience >= _xpnext)
            {
                _level++;
                _experience = _xpnext - Experience;
                _xplast = _xpnext;
                _xpnext = _xpnext * 3;
                _skillsPoints++;
                StatLevelUP();
                _life = _lifemax;
               lvlup = true;
            }
        }
        public void  LvlUpdate ()
        {
            if (lvlup == true)
            {
                float elapsed = (float)Board.CurrentBoard.GameTime.ElapsedGameTime.TotalSeconds;
                _time -= elapsed;
                if (_time < 0)
                {
                   
                    //Timer expired, execute action
                    _time = Timer;
                    lvlup = false;

                }               
            }
        }

        public void StatLevelUP()
        {
            if (_classe == "Wizard")
            {
                _lifemax = _lifemax + 10;
                _agility += 1;
                _intelligence += 5;
                _strength += 3;
            }
            else if (_classe =="Gunner")
            {
                _lifemax = _lifemax + 15;
                _agility += 2;
                _intelligence += 2;
                _strength += 5;
            }
            else if(_classe =="Archer")
            {
                _lifemax = _lifemax + 10;
                _agility += 5;
                _intelligence += 3;
                _strength += 1;
            }
        }

        public void UpSkills(int skillPointNumber, string attribute)
        {
            if (attribute == ("intelligence"))
            {
                _intelligence = _intelligence + skillPointNumber;
            }
            else if (attribute == ("life"))
            {
                _life = _life + skillPointNumber;
            }
            else if(attribute == ("strength"))
            {
                _strength = _strength + skillPointNumber;
            }
            else if (attribute==("agility"))
            {
                _agility = _agility + skillPointNumber; 
            }
            else if (attribute == ("resistance"))
            {
                _resistance = _resistance + skillPointNumber;
            }
            _skillsPoints = _skillsPoints - skillPointNumber;
        }
    }
}
