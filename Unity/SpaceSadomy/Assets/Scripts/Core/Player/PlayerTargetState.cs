using UnityEngine;
using System.Collections;
using Common;
using Game.Network;
using Nebula.UI;
using Nebula.Mmo.Items;

namespace Nebula {
    public class PlayerTargetState : IServerPropertyParser {
        private Item _owner;
        private bool _hasTarget;
        private string _targetId;
        private byte _targetType;
        private Item _targetItem;

        private System.Action targetUpdated;

        public void SetTargetUpdated(System.Action action) {
            this.targetUpdated = action;
        }

        public void ResetTarget() {
            _hasTarget = false;
            _targetId = null;
            _targetType = 0;

            if (this._targetItem != null) {
                if (this._targetItem.View) {
                    //                this._targetItem.View.GetComponent<BaseSpaceObject>().UnsetSelection();
                }
                this._targetItem = null;
            }

            if (this.targetUpdated != null) {
                this.targetUpdated();
            }

            if (MainCanvas.Get) {
                MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);
            }
        }

        public PlayerTargetState(Item owner) {
            _owner = owner;
            ResetTarget();
        }

        public bool HasTarget { get { return _hasTarget; } }
        public string TargetId { get { return _targetId; } }
        public byte Type { get { return _targetType; } }

        public Item Item {
            get {
                if(!_hasTarget) {
                    _targetItem = null;
                    return _targetItem;
                } else {
                    if(string.IsNullOrEmpty(_targetId)) {
                        _targetItem = null;
                        return _targetItem;
                    } else {
                        if((_targetItem == null) || (_targetItem.Id != _targetId)) {
                            if (G.Game.TryGetItem(_targetType, _targetId, out _targetItem) == false) {
                                Debug.Log("target not founded");
                                _targetItem = null;
                                
                            }
                        }
                        return _targetItem;
                    }
                }
            }
        }

        public bool HasTargetAndTargetGameObjectValid {
            get {
                if (HasTarget) {
                    _targetItem = Item;

                    if (_targetItem != null) {
                        if (_targetItem.View && _targetItem.Component) {
                            return true;
                        } else {
                            if(!_targetItem.View) {
                                Debug.Log(""); ;
                            }
                        }
                    } else {
                        Debug.Log("Target item is null");
                    }
                } else {
                    Debug.Log("HasTarget Is False");
                }
                return false;
            }
        }


        public void ParseProp(byte propName, object value) {
            switch ((PS)propName) {
                case PS.HasTarget:
                    {
                        _hasTarget = (bool)value;
                        //Debug.Log("receive target value: " + _hasTarget);
                    }
                    break;
                case PS.TargetId:
                    {
                        _targetId = (string)value;
                    }
                    break;
                case PS.TargetType:
                    {
                        _targetType = (byte)value;
                    }
                    break;
            }
            if (this.targetUpdated != null) {
                this.targetUpdated();
            }


        }

        public void SetTarget(bool hasTarget, string targetId, byte targetType) {
            this._hasTarget = hasTarget;
            this._targetId = targetId;
            this._targetType = targetType;
        }


        public Hashtable GetTargetInfo() {
            var game = G.Game;

            Hashtable result = new Hashtable();
            if (HasTargetAndTargetGameObjectValid) {
                MyItem avatar = G.Game.Avatar;
                string name = string.Format("{0}({1})", _targetItem.Id.Substring(0, 3), _targetItem.Name);
                float distance = Vector3.Distance(avatar.GetPosition(), _targetItem.GetPosition());
                float speed = (_targetItem is IDamagable) ? ((IDamagable)_targetItem).GetSpeed() : 0;
                //float hitProb = GameBalance.ComputeHitProb(avatar.Ship.Weapon.RangeMin, avatar.Ship.Weapon.RangeMax, avatar.Ship.Weapon.MinHitProb, distance);
                float hitProb = 0.0f;
                if (game.Ship.Weapon.HasWeapon) {
                    hitProb = GameBalance.ComputeHitProb(game.Ship.Weapon.WeaponObject.OptimalDistance, game.Ship.Weapon.Range, distance, 0, 0);
                }

                if (_targetItem is IDamagable) {
                    float health = (_targetItem as IDamagable).GetHealth01();
                    result.Add("health", health);
                }
                result.Add("name", name);
                result.Add("distance", distance);
                result.Add("hit_prob", hitProb);
            }
            return result;
        }

        public bool TargetIsAsteroid {
            get {
                return (true == this._hasTarget) && (ItemType.Asteroid == this._targetType.toEnum<ItemType>());
            }
        }

        public float DistanceTo() {
            if (false == this._hasTarget || false == this._targetItem.View)
                return 0.0f;
            if (this._owner.IsDestroyed || this._owner.ShipDestroyed || (!this._owner.View))
                return 0.0f;
            return Vector3.Distance(this._owner.View.transform.position, this._targetItem.View.transform.position);
        }

        public float HitProb() {
            if (false == this._hasTarget || false == this._targetItem.View)
                return 0.0f;
            if (false == (this._targetItem is IDamagable))
                return 0.0f;

            return GameBalance.ComputeHitProb(((MyItem)this._owner).GetOptimalDistance(), ((MyItem)this._owner).GetRange(), this.DistanceTo(), ((MyItem)this._owner).GetMaxHitSpeed(), ((IDamagable)this._targetItem).GetSpeed());
        }

    }
}