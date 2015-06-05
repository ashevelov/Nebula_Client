using UnityEngine;
using System.Collections;
using UIC;

public class ControlPanelProcess : MonoBehaviour {

    public IControlPanel controlPanel;

    void Start()
    {
        StartCoroutine(UpdateInfo());
    }

    IEnumerator UpdateInfo()
    {
        if (CheckCondition())
        {

            if (controlPanel == null)
            {
                controlPanel = FindObjectOfType<ControlPanel>();
                for(int i = 0; i < 9; i++)
                {
                    var skill = G.GamePlayerSkill(i);
                }
            }
            else
            {
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
