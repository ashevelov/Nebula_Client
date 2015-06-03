namespace UIC
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IPlayerInfo 
	{
		int MaxHP { set; }

		int MaxEnegry { set; }

		int MaxEXP { set; }

        string Position { set; }

		string Name { set; }

		int Speed { set; }

		int CurentEnergy { set; }

		int CurentExp { set; }

		int CurentHP { set; }

	}
}

