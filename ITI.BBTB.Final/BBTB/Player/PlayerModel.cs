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
		int _classes;
        /*Attributs*/
        int _life, _strength, _agility, _experience, _intelligence, _resistance;
        /*Inventory*/
        string _weapon1, _weapon2;
        string _helmet, _breastplate, _boots, _leggings;
        string _potiontype;
        int _krumbz;      

        int _level;

		int _xpnext;
		int _skillsPoints;

		int _floor, _room, _type;
		public string _name;

		[NonSerialized]
        int _xplast;
		[NonSerialized]
		Board _board;
#endregion

		public PlayerModel(string name, int classes)
        {
            _name = name;
            _classes = classes;
            _life = 100;
            _level = 1;
            _experience = 0;
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

		public void StageAndRoom()
		{
			_floor = _board.StageNumber;
			_room = _board.RoomNumber;
			_type = 3;
		}
        public void Level1SkillSetUp()
        {
            Level = 1;
            Experience = 0;
            Life = 100;
            Strength = 10;
            Intelligence = 10;
            Agility = 10;
            Resistance = 10;
            Money = 0;
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
            }
        }

        public void StatLevelUP()
        {
            if (_classes == 1)
            {
                _life = _life + 10;
            }
            else if (_classes==2)
            {
                _life = _life + 10;
            }
            else if(_classes ==3)
            {
                _life = _life + 10;
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
