using UnityEngine;
using System.Collections;
using Nebula.Resources;
using Nebula.Mmo.Items.Components;
using Client.UIC.Implementation;
using Client.UIC;
using Nebula;

namespace Client.UIP.Implementation
{
    public class PlayerInfoProcess : MonoBehaviour
    {

        public IPlayerInfo playerInfo;

        void OnEnable()
        {
            StartCoroutine(UpdateInfo());
        }

        IEnumerator UpdateInfo()
        {
            if (CheckCondition())
            {


                if (playerInfo == null)
                {
                    playerInfo = FindObjectOfType<PlayerInfo>();
                    playerInfo.Name = MmoEngine.Get.LoginGame.login;
                    playerInfo.Avatar = SpriteCache.RaceSprite(GameData.instance.playerInfo.Race);
                }
                else
                {
                    playerInfo.MaxHP = (int)GameData.instance.stats.MaxHP;
                    playerInfo.MaxEnegry = (int)GameData.instance.stats.MaxEnergy;
                    playerInfo.Speed = (int)(GameData.instance.stats.CurrentSpeed * 100);
                    playerInfo.Position = G.PlayerComponent.transform.position.ToIntString();
                    playerInfo.CurentEnergy = (int)GameData.instance.stats.CurrentEnergy;
                    playerInfo.CurentHP = (int)GameData.instance.stats.CurrentHP;
                    playerInfo.Level = DataResources.Instance.Leveling.LevelForExp(GameData.instance.playerInfo.Exp);
                    playerInfo.ProgressExp = DataResources.Instance.Leveling.LevelProgress(GameData.instance.playerInfo.Exp);
                }
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(UpdateInfo());
        }

        public bool CheckCondition()
        {
            if (G.Game == null || GameData.instance.playerInfo == null || G.PlayerComponent == null)
                return false;
            return true;
        }


    }
}
