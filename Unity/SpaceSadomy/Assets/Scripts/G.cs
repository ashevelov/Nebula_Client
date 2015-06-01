using Common;
using Game.Network;
using Game.Space;
using Nebula.Client.Inventory;
using Nebula.UI;
using System.Collections.Generic;
using UnityEngine;
using Nebula.Client;
using Nebula;
using Nebula.Mmo.Games;

public class G  
{

    public static ClientInventory Inventory() {
        if(Game == null ) {
            return null;
        }
        return Game.Inventory;
    }

    public static NetworkGame Game
    {
        get
        {
            if (MmoEngine.Get)
                return MmoEngine.Get.NebulaGame;
            else
                return null;
        }
    }

    public static MyItem PlayerItem
    {
        get
        {
            return G.Game.Avatar;
        }
    }

    public static MyPlayer PlayerComponent
    {
        get
        {
            if (G.Game.Avatar != null)
            {
                return G.Game.Avatar.Component as MyPlayer;
            }
            return null;
        }
    }

    public static MouseOrbitRotateZoom PlayerControlCamera
    {
        get
        {
            return MouseOrbitRotateZoom.Get;
        }
    }

    public static float GetHitProbTo(IDamagable damagable)
    {
        if(IsPlayerValid())
        {
            return GameBalance.ComputeHitProb(Game.Ship.Weapon.OptimalDistance,
                Game.Ship.Weapon.Range, DistanceTo(damagable), Game.Ship.MaxLinearSpeed, damagable.GetSpeed());
        }
        return 0f;
    }

    public static float DistanceTo(IDamagable damagable)
    {
        if(IsPlayerValid())
        {
            return Vector3.Distance(PlayerComponent.transform.position, damagable.GetPosition());
        }
        return 0f;
    }

    public static bool IsPlayerValid()
    {
        return Game != null && G.PlayerItem != null && G.PlayerComponent && (!G.PlayerItem.ShipDestroyed);
    }

    public static bool IsPlayerTargetValid()
    {
        if(IsPlayerValid())
        {
            return PlayerItem.Target.HasTargetAndTargetGameObjectValid;
        }
        return false;
    }

    public static bool IsPlayerHasTarget()
    {
        if(IsPlayerValid())
        {
            return PlayerItem.Target.HasTarget;
        }
        return false;
    }

    public static void ResetPlayerTarget()
    {
        if(IsPlayerHasTarget())
        {
            PlayerItem.RequestTarget(string.Empty, ItemType.Avatar.toByte(), false);
        }
    }

    public static List<ClientWorldEventInfo> ActiveWorldEvents()
    {
        if (IsPlayerValid())
        {
            return Game.ActiveWorldEvents();
        }
        return new List<ClientWorldEventInfo>();
    }

    public static bool ChatVisible()
    {
        return MainCanvas.Get.Exists(CanvasPanelType.ChatView);
    }
    public static void SetChatVisible(bool visible)
    {
        MainCanvas.Get.Show(CanvasPanelType.ChatView);
    }

    public static string CurrentWorldId()
    {
        return Game.CurrentWorldId();
    }

    public static float GameShipWeaponLightShotTimer01()
    {
        return Game.ShipWeaponLightShotTimer01();
    }

    public static float GameShipWeaponHeavyShotTimer01()
    {
        return Game.ShipWeaponHeavyShotTimer01();
    }

    public static ClientPlayerSkill GamePlayerSkill(int index)
    {
        return Game.PlayerSkill(index);
    }

    //public static void RequestFireShot(ShotType shotType )
    //{
    //    if(Game == null )
    //    {
    //        return;
    //    }
    //    if(PlayerItem == null )
    //    {
    //        return;
    //    }
    //    if(Game.State != GameState.WorldEntered)
    //    {
    //        return;
    //    }
    //    if(!PlayerItem.Target.HasTargetAndTargetGameObjectValid)
    //    {
    //        return;
    //    }
    //    NRPC.RequestFire(shotType);
    //}

    //public static void GamePlayerRequestUseSkill(int index)
    //{
    //    if (Game == null)
    //    {
    //        return;
    //    }
    //    if (PlayerItem == null)
    //    {
    //        return;
    //    }
    //    Game.PlayerRequestUseSkill(index);
    //}

    public static float GamePlayerShipWeaponShotEnergy(ShotType shotType )
    {
        return Game.ShotEnergy(shotType);
    }

    public static float GamePlayerShipEnergy()
    {
        return Game.ShipEnergy();
    }
}
