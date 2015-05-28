// ForeignPlayerItem.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Thursday, December 4, 2014 5:08:10 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
namespace Nebula
{
    using Common;
    using Nebula.UI;
    using System.Collections;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Nebula.Client;

    //Item for other human players
    public class ForeignPlayerItem : ForeignItem, IDamagable, IBonusHolder, ICombatObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject _component;
        private ForeignShip _ship;
        private ForeignShipModules modules;
        private ActorBonuses bonuses;
        private float updatePropertiesInterval = 1;
        private float lastUpdateCall;
        private int level = 0;


        //flag for differ first creation from next respawns
        private bool initiallyCreatedFlag = false;
        private TextureSubCache<string> texSubCache = new TextureSubCache<string>();

        public ForeignPlayerItem(string id, byte type, NetworkGame game, string name)
            : base(id, type, game, name)
        {
            _ship = new ForeignShip(this);
            modules = new ForeignShipModules();
            this.bonuses = new ActorBonuses();

        }


        public override void Create(GameObject obj)
        {
            base.Create(obj);
            _component = _view.AddComponent<ForeignPlayer>();
            _component.Initialize(Game, this);
            this.initiallyCreatedFlag = true;
        }

        public override BaseSpaceObject Component
        {
            get { return _component; }
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    _ship.ParseProp(propName, newValue);
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    _ship.Weapon.ParseProp(propName, newValue);
                    break;
                case GroupProps.SHIP_MODULES:
                    modules.ParseProp(propName, newValue);
                    break;
                case GroupProps.BONUSES:
                    {
                        if (propName == Props.BONUSES)
                        {
                            Hashtable bons = newValue as Hashtable;
                            if (bons != null)
                            {
                                this.bonuses.Replace(bons);
                            }
                        }
                    }
                    break;
                case GroupProps.DEFAULT_STATE:
                    if (propName == Props.DEFAULT_STATE_LEVEL)
                        this.level = (int)newValue;
                    break;
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    {
                        _ship.ParseProps(properties);
                    }
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    {
                        _ship.Weapon.ParseProps(properties);
                    }
                    break;
                case GroupProps.POWER_FIELD_SHIELD_STATE:
                    {
                        _ship.PowerField.ParseProps(properties);
                    }
                    break;
                case GroupProps.SHIP_MODULES:
                    {
                        modules.ParseProps(properties);
                    }
                    break;
                case GroupProps.BONUSES:
                    {
                        Hashtable bons = properties.GetValue<Hashtable>(Props.BONUSES, new Hashtable());
                        this.bonuses.Replace(bons);
                    }
                    break;
                case GroupProps.DEFAULT_STATE:
                    foreach (DictionaryEntry entry in properties)
                        if (entry.Key.ToString() == Props.DEFAULT_STATE_LEVEL)
                            this.level = (int)entry.Value;
                    break;
            }
        }

        public ForeignShip Ship
        {
            get
            {
                return _ship;
            }
        }

        public ForeignShipModules Modules
        {
            get { return modules; }
        }

        public bool IsDead()
        {
            return _ship.Destroyed;
        }

        public override void UseSkill(Hashtable skillProperties)
        {
            if (Component && (false == IsDead()))
            {
                Component.UseSkill(skillProperties);
            }
        }


        public bool IsPowerShieldEnabled()
        {
            if (false == IsDead())
            {
                return _ship.PowerField.Enabled;
            }
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
            Debug.LogError("not implemented 3");
            return 0;
        }

        public float GetFarDist()
        {
            Debug.LogError("not implemented 4");
            return 0;
        }

        public ActorBonuses Bonuses
        {
            get { return this.bonuses; }
        }

        public override void SetShipDestroyed(bool shipDetroyed)
        {
            base.SetShipDestroyed(shipDetroyed);

            //handle respwan other player, when 
            if (false == shipDetroyed)
            {
                if (false == ExistsView)
                {
                    if (this.Modules.Prefabs.Count == 5)
                    {
                        if (this.initiallyCreatedFlag)
                        {
                            GameObject obj = ShipModel.Init(this.modules.Prefabs, false);
                            this.Create(obj);
                        }
                    }
                }
            }
        }

        public override void AdditionalUpdate()
        {
            if (Time.time - lastUpdateCall > updatePropertiesInterval)
            {
                this.GetProperties(new string[] { GroupProps.SHIP_BASE_STATE, 
                GroupProps.SHIP_WEAPON_STATE, 
                GroupProps.MECHANICAL_SHIELD_STATE, 
                GroupProps.POWER_FIELD_SHIELD_STATE, 
                GroupProps.DEFAULT_STATE,
                GroupProps.SHIP_MODULES, 
                GroupProps.BONUSES,
                GroupProps.target_info
            });
            }
        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                new SelectedObjectContextMenuView.InputEntry {
                     ButtonText = "Info",
                      ButtonAction = ()=> { Debug.Log("Show chest info"); }
                },
                new SelectedObjectContextMenuView.InputEntry {
                    ButtonText = "Invite To Group",
                    ButtonAction = () => {
                        //here send invite
                        NRPC.SendInviteToGroup(this.Id);
                    }
                }
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }

        #region ICombatObjectInfo implementation
        public int Level
        {
            get { return this.level; }
        }

        public float CurrentHealth
        {
            get
            {
                if (this._ship == null)
                    return 0f;
                return this._ship.Health;
            }
        }

        public float MaxHealth
        {
            get
            {
                if (this._ship == null)
                    return 0f;
                return this._ship.MaxHealth;
            }
        }

        public float HitProb
        {
            get
            {
                var playerGO = G.PlayerComponent;

                if (G.Game != null && G.PlayerItem != null && playerGO)
                {
                    float optimalDistance = G.Game.Ship.Weapon.OptimalDistance;
                    float range = G.Game.Ship.Weapon.Range;
                    float distance = Vector3.Distance(playerGO.transform.position, this.GetPosition());
                    float playerMaxSpeed = G.Game.Ship.MaxLinearSpeed;
                    return GameBalance.ComputeHitProb(optimalDistance, range, distance, playerMaxSpeed, this.Ship.Speed);
                }
                return 0f;
            }
        }

        public Sprite Icon
        {
            get
            {
                return SpriteCache.RaceSprite(this.Race);
            }
        }

        public ObjectInfoType InfoType
        {
            get { return ObjectInfoType.StandardCombatNpc; }
        }

        public string Description
        {
            get { return "One of the pilots in space"; }
        }

        public Color Relation
        {
            get
            {
                if (G.Game != null && G.PlayerItem != null)
                {
                    Race playerRace = G.PlayerItem.Race;
                    if (playerRace == this.Race)
                        return Color.green;
                    else
                        return Color.red;
                }
                return Color.yellow;
            }
        }

        public float DistanceToPlayer
        {
            get
            {
                if (G.Game != null && G.Game.Avatar != null && G.Game.Avatar.Component && this._component)
                {
                    var playerPosition = G.PlayerComponent.transform.position;
                    var selfPosition = this._component.transform.position;
                    return Vector3.Distance(playerPosition, selfPosition);
                }
                return float.NaN;
            }
        }
        #endregion
    }

}