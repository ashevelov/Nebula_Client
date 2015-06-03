using UnityEngine;
using System.Collections;
using UIC;

public class PlayerInfoProcess : MonoBehaviour {

    public IPlayerInfo playerInfo;

	void Start () {

        playerInfo = FindObjectOfType<PlayerInfo>();

        playerInfo.MaxHP = 1000;
        playerInfo.MaxEnegry = 300;
        playerInfo.MaxEXP = 300;

        playerInfo.Name = "VASA";

        StartCoroutine(UpdateInfo());
	}

    int count = 0;
    IEnumerator UpdateInfo()
    {
        yield return new WaitForSeconds(1);
        count ++;
        playerInfo.CurentEnergy = count * 2;
        playerInfo.CurentHP = count * 5;

        playerInfo.Speed = count;
        playerInfo.Position = (count / 2) + " , " + count + " , " + (count * 2);

        StartCoroutine(UpdateInfo());
    }
	
}
