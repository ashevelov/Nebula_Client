using UnityEngine;
using System.Collections;
using UIC;
using Nebula;
using Nebula.Resources;

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

                //controlPanel.UpdateButton(6, 0, null, OnFire);
                controlPanel.UpdateButton(6, 0, 0, null, OnAccelerate);
                controlPanel.UpdateButton(7, 0, 0, null, OnMove);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    var skill = G.GamePlayerSkill(i);
                    if (skill.HasSkill)
                    {
                        controlPanel.UpdateButton(i, skill.Cooldown, skill.Progress(), SpriteCache.SpriteSkill("H" + skill.Id.ToString("X8")), (indx) =>
                            {
                                NRPC.RequestUseSkill(indx);
                            });
                    }
                    else
                    {
                        controlPanel.UpdateButton(i, 0, 0, SpriteCache.SpriteSkill("Empty"), null);
                    }
                }
                //NOW NO WEAPON TIMER
                //controlPanel.UpdateButton(8, G.GameShipWeaponLightShotTimer01());
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdateInfo());
    }

    void OnFire(int indx)
    {
        //request shot don't work
        //NRPC.RequestFire(Common.ShotType.Light);

        //Now need use
        //NRPC.RequestUseSkill(skill index)
    }


    bool move = true;
    public void OnMove(int indx)
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
    public void OnStop(int indx)
    {
        G.PlayerItem.RequestStop();
    }

    bool accelerate = false;
    public void OnAccelerate(int indx)
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
