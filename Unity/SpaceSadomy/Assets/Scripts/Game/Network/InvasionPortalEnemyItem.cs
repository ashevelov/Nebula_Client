using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Nebula.UI;
using System;
using System.Collections.Generic;
using Nebula.Client;

namespace Nebula
{
    public class InvasionPortalEnemyItem : NpcItem, IDamagable, IBonusHolder,
        Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject _component;
        private ForeignShip _ship;
        private ActorBonuses bonuses;

        public override BaseSpaceObject Component
        {
            get { return _component; }
        }

        public ForeignShip Ship
        {
            get
            {
                return _ship;
            }
        }

        public InvasionPortalEnemyItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name)
            : base(id, type, game, subType, name)
        {
            _ship = new ForeignShip(this);
            this.bonuses = new ActorBonuses();
        }

        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            _component = _view.AddComponent<InvasionPortalEnemy>();
            _component.Initialize(Game, this);
        }*/

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            _component = _view.AddComponent<InvasionPortalEnemy>();
            _component.Initialize(Game, this);
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    _ship.ParseProp(propName, newValue);
                    break;
                case GroupProps.DEFAULT_STATE:
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    _ship.Weapon.ParseProp(propName, newValue);
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
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    _ship.ParseProps(properties);
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    _ship.Weapon.ParseProps(properties);
                    break;
                case GroupProps.BONUSES:
                    {
                        Hashtable bons = properties.GetValue<Hashtable>(Props.BONUSES, new Hashtable());
                        this.bonuses.Replace(bons);
                    }
                    break;
            }
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
            Debug.LogError("not implemented 5");
            return 0;
        }

        public float GetFarDist()
        {
            Debug.LogError("not implemented 6");
            return 0;
        }

        public ActorBonuses Bonuses
        {
            get { return this.bonuses; }
        }

        public override void AdditionalUpdate()
        {

        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                new SelectedObjectContextMenuView.InputEntry {
                     ButtonText = "Info",
                      ButtonAction = ()=> { Debug.Log("Show chest info"); }
                },
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }
    }
}