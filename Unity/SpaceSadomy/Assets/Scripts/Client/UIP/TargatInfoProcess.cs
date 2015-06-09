using UnityEngine;
using System.Collections;
using UIC;
using Nebula.UI;

public class TargatInfoProcess : MonoBehaviour {

    public ITargetInfo uicPanel;

    private IObjectInfo objectInfo;

    void Start()
    {
        StartCoroutine(UpdateInfo());
    }

    public void SetObject(IObjectInfo objectInfo)
    {
        this.objectInfo = objectInfo;
    }

    private void CombatUpdate(ICombatObjectInfo info)
    {
        uicPanel.Name = info.Name;
        uicPanel.MaxHP = (int)info.MaxHealth;
        uicPanel.CurentHP = (int)info.CurrentHealth;
        uicPanel.Distance = info.DistanceToPlayer;
        uicPanel.Level = info.Level;
    }


    private void AsteroidUpdate(IAsteroidObjectInfo info)
    {
        uicPanel.Name = info.Name;
        uicPanel.Distance = info.DistanceToPlayer;
    }

    IEnumerator UpdateInfo()
    {
        if (CheckCondition())
        {
            if (uicPanel == null)
            {
                uicPanel = FindObjectOfType<TargetInfo>();
                uicPanel.MaxHP = 50000;
                uicPanel.CurentHP = 50000;
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
        StartCoroutine(UpdateInfo());
    }

    public bool CheckCondition()
    {
        if (objectInfo == null)
            return false;
        return true;
    }
}
