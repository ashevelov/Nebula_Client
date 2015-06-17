using UnityEngine;
using System.Collections;
using UIC;
using Nebula;

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

                controlPanel.UpdateButton(6, 0, null, OnFire);
                controlPanel.UpdateButton(7, 0, null, OnMove);
                controlPanel.UpdateButton(8, G.GameShipWeaponLightShotTimer01(), null, OnAccelerate);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    var skill = G.GamePlayerSkill(i);
                    if (!skill.HasSkill)
                    {
                        controlPanel.UpdateButton(i, skill.Progress(), SpriteCache.SpriteSkill(skill.Id), () => NRPC.RequestUseSkill(i));
                    }
                    else
                    {
                        controlPanel.UpdateButton(i, 0, SpriteCache.SpriteSkill("Empty"), () => { });
                    }
                }
                controlPanel.UpdateButton(8, G.GameShipWeaponLightShotTimer01());
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateInfo());
    }

    void OnFire()
    {
        NRPC.RequestFire(Common.ShotType.Light);
    }


    bool move = true;
    public void OnMove()
    {

        accelerate = !accelerate;
        if (accelerate)
        {
            G.PlayerItem.RequestMoveDirection();
            G.PlayerItem.RequestLinearSpeed(G.Game.Ship.MaxLinearSpeed);
        }
        else
        {
            G.PlayerItem.RequestStop();
        }
    }
    public void OnStop()
    {
        G.PlayerItem.RequestStop();
    }

    bool accelerate = false;
    public void OnAccelerate()
    {
        accelerate = !accelerate;
        if (accelerate)
            NRPC.RequestShiftDown();
        else
            NRPC.RequestShiftUp();
    }
       

    public bool CheckCondition()
    {
        if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
            return false;
        return true;
    }
}
