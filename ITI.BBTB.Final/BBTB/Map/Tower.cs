using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB
{
	class Tower
	{
		int _numberOfStages;
		int _stageNumber;
		Board _board;
		Random _rnd = new Random();

		public Tower(int numberofstages)
		{
			_numberOfStages = numberofstages;
		}

		public int StageNumber { get { return _stageNumber; } set { _stageNumber = value; } }

		public void Stage1()
		{
			_board.RoomInFloor = _rnd.Next(4, 7);
			_stageNumber = 1;
			_board.PlayerActualRoom = 1;
			_board.CreateNewBoard();
		}


	}
}
