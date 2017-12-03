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
        string _name;
        int _classes;
        /*Attributs*/
        int _life, _strength, _agility, _experience, _intelligence, _resistance;
        /*Inventory*/
        string _weapon1, _weapon2;
        string _helmet, _breastplate, _boots, _leggings;
        string _potiontype;

        int _krumbz;
        [NonSerialized]        
        int _level;
        [NonSerialized]
        int _xplast;
        int _xpnext;
        int _skillspoints;
        public string name;

        public PlayerModel(string name,int classes)
        {
            _name = name;
            _classes = classes;
        }

        public int Life { get { return _life; } set { _life = value; } }
        public int Experience { get { return _experience; } set { _experience = value; } }
        public int Strength { get { return _strength; } set { _strength = value; } }
        public int Agility { get { return _agility; } set { _agility = value; } }
        public int Intelligence { get { return _intelligence; } set { _intelligence = value; } }
        public int Resistance { get { return _resistance; } set { _resistance = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Money { get { return _krumbz; } set { _krumbz = value; } }

        public int Level { get { return _level; ; } set { _level = value; } }

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
        }

        public void UpSkills(int numberofskillpoint, string attribute)
        {
            if (attribute == ("intelligence"))
            {
                _intelligence = _intelligence + numberofskillpoint;
            }
            else if (attribute == ("life"))
            {
                _life = _life + numberofskillpoint;
            }
            else if(attribute == ("strength"))
            {
                _strength = _strength + numberofskillpoint;
            }
            else if (attribute==("agility"))
            {
                _agility = _agility + numberofskillpoint; 
            }
            else if (attribute == ("resistance"))
            {
                _resistance = _resistance + numberofskillpoint;
            }
            _skillspoints = _skillspoints - numberofskillpoint;
        }

        public void SetExp()
        {
            _xpnext = _xplast = 0;
            if (_level == 0) { _xpnext = 100; }
        }

        public void StatLevelUP()
        {
            bool LvlUp = (_experience == _xpnext);
            if (LvlUp == true)
            {
                _life = _life +10;
            }
        }

        public int LevelUpdate ()
        {
            if(_experience == _xpnext)
            {
                _level ++;
                _xplast = _xpnext;
                _xpnext = _xpnext * 3;
                _skillspoints ++;
            }
            return _level;
        }
    }
}
