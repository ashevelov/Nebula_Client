namespace Nebula.UI {
    using Common;
    using Nebula.Client.Res;
    using Nebula.Resources;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BuffsView : BaseView {

        public GameObject BuffButtonPrefab;
        public Transform Content;

        public class BuffViewData {
            public BuffData Data;
            public BuffViewButton BuffButton;
            
        }

        public class BuffData {
            public ResBuffData Data;
            public float Value;
        }

        private Dictionary<BonusType, BuffViewData> currentBuffs = new Dictionary<BonusType, BuffViewData>();
        private List<ObjectChange<BuffData>> changes = new List<ObjectChange<BuffData>>();

        void Start() {
            StartCoroutine(C_UpdateBuffsView());
        }

        private IEnumerator C_UpdateBuffsView() {
            while(true) {
                yield return new WaitForSeconds(Settings.BUFFS_UPDATE_INTERVAL);
                this.UpdateBuffsView();
            }
        }

        public void UpdateBuffsView() {
            this.changes.Clear();

            if(G.Game == null ) {
                return;
            }
            if(GameData.instance.bonuses == null ) {
                return;
            }

            foreach(var pBuff in GameData.instance.bonuses.Bonuses ) {
                if(pBuff.Value > 0.0f ) {
                    if(this.currentBuffs.ContainsKey(pBuff.Key)) {
                        this.changes.Add(new ObjectChange<BuffData> {
                            ChangeType = ChangeType.UPDATE,
                            TargetObject = new BuffData {
                                Data = DataResources.Instance.Buff(pBuff.Key),
                                Value = pBuff.Value
                            }
                        });
                    } else {
                        var buff = DataResources.Instance.Buff(pBuff.Key);
                        if (buff == null) {
                            Debug.LogErrorFormat("Not found buff data for: {0}", pBuff.Key);
                            continue;
                        }
                        this.changes.Add(new ObjectChange<BuffData> {
                            ChangeType = ChangeType.ADD,
                            TargetObject = new BuffData {
                                Data = buff,
                                Value = pBuff.Value
                            }
                        });
                    }
                } else {
                    if (this.currentBuffs.ContainsKey(pBuff.Key)) {
                        this.changes.Add(new ObjectChange<BuffData> {
                            ChangeType = ChangeType.REMOVE,
                            TargetObject = new BuffData {
                                Data = DataResources.Instance.Buff(pBuff.Key),
                                Value = pBuff.Value
                            }
                        });
                    }
                }
            }

            foreach(var cBuff in this.currentBuffs) {
                if(GameData.instance.bonuses.IsNon(cBuff.Key)) {
                    this.changes.Add(new ObjectChange<BuffData> {
                        ChangeType = ChangeType.REMOVE,
                        TargetObject = new BuffData {
                            Data = cBuff.Value.Data.Data,
                            Value = cBuff.Value.Data.Value
                        }
                    });
                }
            }

            foreach(var change in this.changes ) {
                switch(change.ChangeType) {
                    case ChangeType.ADD:
                        ChangeAdd(change);
                        break;
                    case ChangeType.REMOVE:
                        ChangeRemove(change);
                        break;
                    case ChangeType.UPDATE:
                        ChangeUpdate(change);
                        break;
                }
            }
            
        }

        private void ChangeUpdate(ObjectChange<BuffData> change) {
            var buffType = change.TargetObject.Data.bonusType;
            var targ = this.currentBuffs[buffType];
            targ.BuffButton.SetObject(buffType, change.TargetObject.Value, change.TargetObject.Data);
        }

        private void ChangeAdd(ObjectChange<BuffData> change) {


            var buffType = change.TargetObject.Data.bonusType;
            GameObject buttonObject = Instantiate(this.BuffButtonPrefab) as GameObject;
            BuffViewButton button = buttonObject.GetComponent<BuffViewButton>();
            button.SetObject(buffType, change.TargetObject.Value, change.TargetObject.Data);
            buttonObject.transform.SetParent(this.Content, false);

            this.currentBuffs.Add(buffType, new BuffViewData {
                BuffButton = button,
                Data = new BuffData {
                    Data = change.TargetObject.Data,
                    Value = change.TargetObject.Value
                }
            });
        }

        private void ChangeRemove(ObjectChange<BuffData> change) {
            var buffType = change.TargetObject.Data.bonusType;
            if(this.currentBuffs.ContainsKey(buffType)) {
                var obj = this.currentBuffs[buffType];
                this.currentBuffs.Remove(buffType);
                if(obj.BuffButton != null ) {
                    Destroy(obj.BuffButton.gameObject);
                    obj.BuffButton = null;
                    obj.Data = null;
                    obj = null;
                }
            }
        }
    }
}