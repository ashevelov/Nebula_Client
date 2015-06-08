using UnityEngine;
using System.Collections;
using UIC;
using Nebula.UI;

public class TargatInfoProcess : MonoBehaviour {

    public ITargetInfo uicPanel;

    void Start()
    {
    }

    public void SetObject(IObjectInfo objectInfo)
    {
        StartCoroutine(UpdateInfo(objectInfo));
    }

    private void CombatUpdate(ICombatObjectInfo info)
    {
        uicPanel.Name = info.Name;
        uicPanel.MaxHP = (int)info.MaxHealth;
        uicPanel.CurentHP = (int)info.CurrentHealth;
        uicPanel.Distance = info.DistanceToPlayer;
    }


    private void AsteroidUpdate(IAsteroidObjectInfo info)
    {
        uicPanel.Name = info.Name;
        uicPanel.Distance = info.DistanceToPlayer;
    }

    IEnumerator UpdateInfo(IObjectInfo objectInfo)
    {
        if (CheckCondition())
        {

            if (uicPanel == null)
            {
                uicPanel = FindObjectOfType<TargetInfo>();

                uicPanel.MaxHP = 50000;
                uicPanel.CurentHP = 50000;
                //uicPanel.Avatar = SpriteCache.RaceSprite(G.Game.PlayerInfo.Race);
            }

            if (objectInfo is ICombatObjectInfo)
            {
                CombatUpdate(objectInfo as ICombatObjectInfo);
            }
            else if (objectInfo is IAsteroidObjectInfo)
            {
                AsteroidUpdate(objectInfo as IAsteroidObjectInfo);
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateInfo(objectInfo));
    }

    public bool CheckCondition()
    {
        return true;
    }
}
