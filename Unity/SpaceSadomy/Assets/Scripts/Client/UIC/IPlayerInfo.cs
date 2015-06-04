namespace UIC
{
    using UnityEngine;

	public interface IPlayerInfo 
	{
		int MaxHP { set; }

		int MaxEnegry { set; }

        string Position { set; }

		string Name { set; }

		int Speed { set; }

		int CurentEnergy { set; }

        int CurentHP { set; }

        float ProgressExp { set; }

        int Level { set; }

        Sprite Avatar { set; }

	}
}

