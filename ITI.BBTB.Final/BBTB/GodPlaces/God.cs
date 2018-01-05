using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
	public class God
	{
		Player _player;
		string _name;

		int _lifeBoost;

		public God (Player player, string name)
		{
			_name = name;
			_player = player;
		}

		public void Pray ()
		{
			// appication of boosts 
			_player._playerM.Life += 100;
			_player.HavePrayed = true;
		}
	}
}
