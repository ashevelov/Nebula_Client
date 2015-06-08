using UnityEngine;
using System.Collections;
using UIC;

public class PlayerInfoProcess : MonoBehaviour {

    public IPlayerInfo playerInfo;

	void Start () {
        StartCoroutine(UpdateInfo());
	}

    IEnumerator UpdateInfo()
    {        
        if (CheckCondition())
        {

            if (playerInfo == null)
            {
                playerInfo = FindObjectOfType<PlayerInfo>();
                playerInfo.Name = G.Game.PlayerInfo.Name;
                playerInfo.Avatar = SpriteCache.RaceSprite(G.Game.PlayerInfo.Race);
            }
            else
            {
                playerInfo.MaxHP = (int)G.Game.CombatStats.MaxHP;
                playerInfo.MaxEnegry = (int)G.Game.CombatStats.MaxEnergy;
                playerInfo.Speed = (int)G.Game.CombatStats.CurrentSpeed;
                playerInfo.Position = G.PlayerComponent.transform.position.ToIntString();
                playerInfo.CurentEnergy = (int)G.Game.CombatStats.CurrentEnergy;
                playerInfo.CurentHP = (int)G.Game.CombatStats.CurrentHP;
                playerInfo.Level = DataResources.Instance.Leveling.LevelForExp(G.Game.PlayerInfo.Exp);
                playerInfo.ProgressExp = DataResources.Instance.Leveling.LevelProgress(G.Game.PlayerInfo.Exp);
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateInfo());
    }

    public bool CheckCondition()
    {
        if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
            return false;
        return true;
    }

	
}
