using UnityEngine;
using System.Collections;
using UIC;

public class PlayerInfoProcess : MonoBehaviour {

    public IPlayerInfo playerInfo;

	void Start () {

        playerInfo = FindObjectOfType<PlayerInfo>();
        if(CheckCondition())
        {
            playerInfo.Name = G.Game.PlayerInfo.Name;
            playerInfo.Avatar = SpriteCache.RaceSprite(G.Game.PlayerInfo.Race);
        }
        StartCoroutine(UpdateInfo());
	}

    IEnumerator UpdateInfo()
    {
        yield return new WaitForSeconds(0.5f);
        if (CheckCondition()) {
            playerInfo.MaxHP = (int)G.Game.CombatStats.MaxHP;
            playerInfo.MaxEnegry = (int)G.Game.CombatStats.MaxEnergy;
            playerInfo.Speed = (int)G.Game.CombatStats.CurrentSpeed;
            playerInfo.Position = G.PlayerComponent.transform.position.ToIntString();
            playerInfo.CurentEnergy = (int)G.Game.CombatStats.CurrentEnergy;
            playerInfo.CurentHP = (int)G.Game.CombatStats.CurrentHP;
            playerInfo.Level = DataResources.Instance.Leveling.LevelForExp(G.Game.PlayerInfo.Exp);
            playerInfo.ProgressExp = DataResources.Instance.Leveling.LevelProgress(G.Game.PlayerInfo.Exp);

            //playerInfo.MaxEXP = data

            //this.RaceImage.overrideSprite = SpriteCache.RaceSprite(G.Game.PlayerInfo.Race);
            //this.WorkshopImage.overrideSprite = SpriteCache.WorkshopSprite(G.Game.PlayerInfo.Workshop);
            //this.NameLevelText.text = string.Format("{0}, {1}", G.Game.PlayerInfo.Name, "" /*PlayerInfo().Level.ToString().Color("orange")*/);
            //this.RaceWorkshopText.text = string.Format("{0}, {1}", StringCache.Race(G.Game.PlayerInfo.Race).Color(Utils.RaceColor(PlayerInfo().Race)), StringCache.Workshop(PlayerInfo().Workshop));

            //string coordStr = string.Empty;
            //if (G.PlayerComponent != null) {
            //    coordStr = G.PlayerComponent.transform.position.ToIntString();
            //}
            //this.WorldPositionText.text = string.Format("{0},Coord:{1}", stringCache.String(CurrentZone().DisplayName(), CurrentZone().DisplayName()).Color("orange"), coordStr);
            //this.HpEnText.text = string.Format("HP:{0}/{1},EN:{2}/{3},EXP:{4}/{5}",
            //    Mathf.RoundToInt(G.Game.CombatStats.CurrentHP),
            //    Mathf.RoundToInt(G.Game.CombatStats.MaxHP),
            //    Mathf.RoundToInt(G.Game.CombatStats.CurrentEnergy),
            //    Mathf.RoundToInt(G.Game.CombatStats.MaxEnergy),
            //    Mathf.RoundToInt(PlayerInfo().Exp),
        }
        StartCoroutine(UpdateInfo());
    }

    public bool CheckCondition()
    {
        if (G.Game == null || G.Game.PlayerInfo == null || playerInfo == null)
            return false;
        return true;
    }

	
}
