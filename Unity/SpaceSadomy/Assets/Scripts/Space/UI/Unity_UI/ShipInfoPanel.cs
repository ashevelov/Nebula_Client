using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Client;
using Nebula;
using Nebula.Client.Inventory.Objects;
using Nebula.UI;

public class ShipInfoPanel : Nebula.UI.BaseView
{
    public RectTransform CBView;
    public RectTransform DFView;
    public RectTransform DMView;
    public RectTransform CMView;
    public RectTransform ESView;
    public RectTransform WeaponView;
    public RectTransform AllView;
    public ShipInfoParam shipInfoParam;

    public override void Setup(object objData) {
        base.Setup(objData);
    }

    public void UpdateCBInfo()
    {
        ClearInfo(CBView);
        RectTransform view = CBView;
        ClientShipModule info = G.Game.Ship.ShipModel.cb.Module;
        AddNewParam(view, "Type".Color(info.color.ToString()), info.type.ToString().Color(info.color.ToString()), 0);
        AddNewParam(view, "Name", info.name, 1);
        AddNewParam(view, "Min level", info.level.ToString(), 2);
        AddNewParam(view, "Damage bonus", info.damageBonus.ToString(), 3);
        AddNewParam(view, "Distance bonus", info.distanceBonus.ToString(), 4);
        AddNewParam(view, "Energy", info.energy.ToString(), 5);
        AddNewParam(view, "Energy restoration", info.energyRestoration.ToString(), 6);
        AddNewParam(view, "hp", info.hp.ToString(), 7);
        AddNewParam(view, "resist", info.resist.ToString(), 8);
        AddNewParam(view, "Cooldown bonus", info.cooldownBonus.ToString(), 9);
        AddNewParam(view, "Crit chance", info.critChance.ToString(), 10);
        AddNewParam(view, "Crit damage", info.critDamage.ToString(), 11);
        AddNewParam(view, "hold", info.hold.ToString(), 12);
        AddNewParam(view, "Weapon slots count", info.weaponSlotsCount.ToString(), 13);
    }

    public void UpdateDFInfo()
    {
        ClearInfo(DFView);
        ClientShipModule info = G.Game.Ship.ShipModel.df.Module;
        RectTransform view = DFView;
        AddNewParam(view, "Type".Color(info.color.ToString()), info.type.ToString().Color(info.color.ToString()), 0);
        AddNewParam(view, "Name", info.name, 1);
        AddNewParam(view, "Min level", info.level.ToString(), 2);
        AddNewParam(view, "Damage bonus", info.damageBonus.ToString(), 3);
        AddNewParam(view, "Distance bonus", info.distanceBonus.ToString(), 4);
        AddNewParam(view, "Energy", info.energy.ToString(), 5);
        AddNewParam(view, "Energy restoration", info.energyRestoration.ToString(), 6);
        AddNewParam(view, "hp", info.hp.ToString(), 7);
        AddNewParam(view, "resist", info.resist.ToString(), 8);
        AddNewParam(view, "Cooldown bonus", info.cooldownBonus.ToString(), 9);
        AddNewParam(view, "Crit chance", info.critChance.ToString(), 10);
        AddNewParam(view, "Crit damage", info.critDamage.ToString(), 11);
        AddNewParam(view, "hold", info.hold.ToString(), 12);
        AddNewParam(view, "Weapon slots count", info.weaponSlotsCount.ToString(), 13);
    }


    public void UpdateDMInfo()
    {
        ClearInfo(DMView);
        ClientShipModule info = G.Game.Ship.ShipModel.dm.Module;
        RectTransform view = DMView;
        AddNewParam(view, "Type".Color(info.color.ToString()), info.type.ToString().Color(info.color.ToString()), 0);
        AddNewParam(view, "Name", info.name, 1);
        AddNewParam(view, "Min level", info.level.ToString(), 2);
        AddNewParam(view, "Damage bonus", info.damageBonus.ToString(), 3);
        AddNewParam(view, "Distance bonus", info.distanceBonus.ToString(), 4);
        AddNewParam(view, "Energy", info.energy.ToString(), 5);
        AddNewParam(view, "Energy restoration", info.energyRestoration.ToString(), 6);
        AddNewParam(view, "hp", info.hp.ToString(), 7);
        AddNewParam(view, "resist", info.resist.ToString(), 8);
        AddNewParam(view, "Cooldown bonus", info.cooldownBonus.ToString(), 9);
        AddNewParam(view, "Crit chance", info.critChance.ToString(), 10);
        AddNewParam(view, "Crit damage", info.critDamage.ToString(), 11);
        AddNewParam(view, "hold", info.hold.ToString(), 12);
        AddNewParam(view, "Weapon slots count", info.weaponSlotsCount.ToString(), 13);
    }


    public void UpdateCMInfo()
    {
        ClearInfo(CMView);
        ClientShipModule info = G.Game.Ship.ShipModel.cm.Module;
        RectTransform view = CMView;
        AddNewParam(view, "Type".Color(info.color.ToString()), info.type.ToString().Color(info.color.ToString()), 0);
        AddNewParam(view, "Name", info.name, 1);
        AddNewParam(view, "Min level", info.level.ToString(), 2);
        AddNewParam(view, "Damage bonus", info.damageBonus.ToString(), 3);
        AddNewParam(view, "Distance bonus", info.distanceBonus.ToString(), 4);
        AddNewParam(view, "Energy", info.energy.ToString(), 5);
        AddNewParam(view, "Energy restoration", info.energyRestoration.ToString(), 6);
        AddNewParam(view, "hp", info.hp.ToString(), 7);
        AddNewParam(view, "resist", info.resist.ToString(), 8);
        AddNewParam(view, "Cooldown bonus", info.cooldownBonus.ToString(), 9);
        AddNewParam(view, "Crit chance", info.critChance.ToString(), 10);
        AddNewParam(view, "Crit damage", info.critDamage.ToString(), 11);
        AddNewParam(view, "hold", info.hold.ToString(), 12);
        AddNewParam(view, "Weapon slots count", info.weaponSlotsCount.ToString(), 13);
    }

    public void UpdateESInfo()
    {
        ClearInfo(ESView);
        ClientShipModule info = G.Game.Ship.ShipModel.es.Module;
        RectTransform view = ESView;
        AddNewParam(view, "Type".Color(info.color.ToString()), info.type.ToString().Color(info.color.ToString()), 0);
        AddNewParam(view, "Name", info.name, 1);
        AddNewParam(view, "Min level", info.level.ToString(), 2);
        AddNewParam(view, "Damage bonus", info.damageBonus.ToString(), 3);
        AddNewParam(view, "Distance bonus", info.distanceBonus.ToString(), 4);
        AddNewParam(view, "Energy", info.energy.ToString(), 5);
        AddNewParam(view, "Energy restoration", info.energyRestoration.ToString(), 6);
        AddNewParam(view, "hp", info.hp.ToString(), 7);
        AddNewParam(view, "resist", info.resist.ToString(), 8);
        AddNewParam(view, "Cooldown bonus", info.cooldownBonus.ToString(), 9);
        AddNewParam(view, "Crit chance", info.critChance.ToString(), 10);
        AddNewParam(view, "Crit damage", info.critDamage.ToString(), 11);
        AddNewParam(view, "hold", info.hold.ToString(), 12);
        AddNewParam(view, "Weapon slots count", info.weaponSlotsCount.ToString(), 13);
    }

    public void UpdateWeaponInfo()
    {
        ClearInfo(WeaponView);
        WeaponInventoryObjectInfo info = G.Game.Ship.Weapon.WeaponObject;
        RectTransform view = WeaponView;
        AddNewParam(view, "Type".Color(info.Color.ToString()), info.Type.ToString().Color(info.Color.ToString()), 0);
        AddNewParam(view, "Min level", info.Level.ToString(), 1);
        AddNewParam(view, "CritChance", info.CritChance.ToString(), 2);
        AddNewParam(view, "Heavy damage", info.HeavyDamage.ToString(), 3);
        AddNewParam(view, "Heavy cooldown", info.HeavyCooldown.ToString(), 4);
        AddNewParam(view, "Heavy crit damage", info.HeavyCritDamage.ToString(), 5);
        AddNewParam(view, "Heavy energy", info.HeavyEnergy.ToString(), 6);
        AddNewParam(view, "Light damage", info.LightDamage.ToString(), 7);
        AddNewParam(view, "Light cooldown", info.LightCooldown.ToString(), 8);
        AddNewParam(view, "Light crit damage", info.LightCritDamage.ToString(), 9);
        AddNewParam(view, "Optimal distance", info.OptimalDistance.ToString(), 10);
        AddNewParam(view, "Template", info.Template.ToString(), 11);
    }

    public void UpdateAllInfo()
    {
        ClearInfo(AllView);
        ClientShipCombatStats info = G.Game.CombatStats;
        RectTransform view = AllView;
        AddNewParam(view, "Ship", "", 0);
        AddNewParam(view, "Energy", info.MaxEnergy.ToString(), 1);
        AddNewParam(view, "HP", info.MaxHP.ToString(), 2);
        AddNewParam(view, "Speed", info.MaxSpeed.ToString(), 3);
        AddNewParam(view, "Resist", info.DamageResist.ToString(), 4);
        AddNewParam(view, "Energy restoration", info.EnergyRestoration.ToString(), 5);
        AddNewParam(view, "Heavy weapon damage", info.HeavyWeaponDamage.ToString(), 6);
        AddNewParam(view, "Light weapon damage", info.LightWeaponDamage.ToString(), 7);
        AddNewParam(view, "Restoration HP per sec", info.RestorationHPPerSec.ToString(), 8);
        AddNewParam(view, "Restoration HP per shot", info.RestorationHPPerShot.ToString(), 9);
        AddNewParam(view, "Heavy critical damage", info.WeaponHeavyCriticalDamage.ToString(), 10);
        AddNewParam(view, "Critical chance", info.WeaponCriticalChance.ToString(), 11);
        AddNewParam(view, "Optimal distance", info.WeaponOptimalDistance.ToString(), 12);
        AddNewParam(view, "Light Critical Damage", info.WeaponLightCriticalDamage.ToString(), 13);
        AddNewParam(view, "Precision", info.WeaponPrecision.ToString(), 14);
        AddNewParam(view, "Inventory Slots Count", info.InventorySlotsCount.ToString(), 15);
        AddNewParam(view, "Weapon Slots", G.Game.Ship.ShipModel.TotalWeaponSlots.ToString(), 16);
    }

    private void ClearInfo(RectTransform view)
    {
        if (shipInfoParam == null)
        {
            shipInfoParam = Resources.Load("Prefabs/UI/ShipInfoParam") as ShipInfoParam;
        }

        if (shipInfoParam == null)
        {
            Debug.LogError("shipInfoParam == null");
            return;
        }
        else
        {
            view.sizeDelta = new Vector2(view.sizeDelta.x, view.parent.ToRectTransform().sizeDelta.y);
            foreach (Transform child in view)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void AddNewParam(RectTransform p, string param, string value, int index)
    {
        ShipInfoParam sIParam = Instantiate(shipInfoParam) as ShipInfoParam;
        sIParam.SetInfo(param, value);
        RectTransform rTransform = sIParam.transform as RectTransform;

        p.sizeDelta = new Vector2(p.sizeDelta.x, Mathf.Max((index+1)*75, p.parent.ToRectTransform().sizeDelta.y));
        rTransform.SetParent(p);
        rTransform.sizeDelta = new Vector2(0,75);
        rTransform.anchoredPosition = new Vector2(0, index * -rTransform.sizeDelta.y);
        rTransform.localScale = Vector3.one;
    }

    public void DestroyThis()
    {
        MainCanvas.Get.Destroy(CanvasPanelType.ShipInfoView);
    }
}
