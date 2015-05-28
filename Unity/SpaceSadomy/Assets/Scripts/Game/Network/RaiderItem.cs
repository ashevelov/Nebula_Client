
namespace Game.Network
{

}
/*
using UnityEngine;
using System.Collections;
using Common;
using Game.Space;

public class RaiderItem : NpcItem, IDamagable  {

    private BaseSpaceObject _component;
    private ForeignShip _ship;

    public override BaseSpaceObject Component
    {
        get { return _component; }
    }

    public ForeignShip Ship {
        get {
            return _ship;
        }
    }

    public RaiderItem(string id, byte type, NetworkGame game, string name)
        : base(id, type, game, BotItemSubType.Raider, name)
    {
        _ship = new ForeignShip(this);
    }



    public override void Create(GameObject obj)
    {
        base.Create(obj);
        _component = _view.AddComponent<Raider>();
        _component.Initialize(Game, this);
    }


    public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
    {
        //Debug.Log(string.Format("received raider property: group: {0} prop {1} value: {2}", group, propName, newValue));
        base.OnSettedProperty(group, propName, newValue, oldValue);
        switch (group)
        {
            case GroupProps.SHIP_BASE_STATE:
                if (propName == Props.SHIP_BASE_STATE_DESTROYED) {
                    Debug.Log("Received destroyed for raider: " + newValue);
                }
                _ship.ParseProp(propName, newValue);
                break;
            case GroupProps.DEFAULT_STATE:
                break;
            case GroupProps.SHIP_WEAPON_STATE:
                _ship.Weapon.ParseProp(propName, newValue);
                break;
        }
    }

    public override void OnSettedGroupProperties(string group, Hashtable properties)
    {

        base.OnSettedGroupProperties(group, properties);
        switch (group) { 
            case GroupProps.SHIP_BASE_STATE:
                _ship.ParseProps(properties);
                break;
            case GroupProps.SHIP_WEAPON_STATE:
                _ship.Weapon.ParseProps(properties);
                break;
        }
    }



    public bool IsDead()
    {
        return _ship.Destroyed;
    }

    public override void UseSkill(Hashtable skillProperties)
    {
        if (Component && (false == IsDead())) {
            Component.UseSkill(skillProperties);
        }
    }


    public bool IsPowerShieldEnabled()
    {
        return false;
    }


    public float GetHealth()
    {
        return _ship.Health;
    }

    public float GetMaxHealth()
    {
        return _ship.MaxHealth;
    }

    public float GetHealth01()
    {
        if (_ship.MaxHealth == 0.0f)
            return 0.0f;
        return Mathf.Clamp01(_ship.Health / _ship.MaxHealth);
    }


    public float GetOptimalDistance()
    {
        return _ship.Weapon.OptimalDistance;
    }

    public float GetRange()
    {
        return _ship.Weapon.Range;
    }

    public float GetFarHitProb()
    {
        return _ship.Weapon.FarProb;
    }

    public float GetNearHitProb()
    {
        return _ship.Weapon.NearProb;
    }

    public float GetMaxHitSpeed()
    {
        return _ship.Weapon.MaxHitSpeed;
    }

    public float GetMaxFireDistance()
    {
        return _ship.Weapon.MaxFireDistance;
    }

    public float GetSpeed()
    {
        return _ship.Speed;
    }


    public float GetNearDist()
    {
        Debug.LogError("not implemented 7");
        return 0;
    }

    public float GetFarDist()
    {
        Debug.LogError("not implemented 9");
        return 0;
    }

    public override void AdditionalUpdate()
    {
        
    }
}
*/