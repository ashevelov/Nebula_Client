namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Nebula.Client;
    using Common;
    using UnityEngine.UI;

    public class GroupMemberView : MonoBehaviour {

        public Transform BuffContent;
        public GameObject BuffViewPrefab;
        public Image WorkshopImage;
        public Text DisplayNameText;
        public Text LevelText;
        public Text WorldNameText;
        public Text WorldPositionText;
        public Image IsLeaderImage;
        public Button SetLeaderButton;
        public Button VoteForExcludeButton;



        private string memberId;
        private ClientCooperativeGroupMember member;

        private Dictionary<BonusType, GroupMemberBuffView> currentBuffs = new Dictionary<BonusType, GroupMemberBuffView>();
        private List<ObjectChange<BuffChangeObject>> buffChanges = new List<ObjectChange<BuffChangeObject>>();


        public void SetObject(string memberId, ClientCooperativeGroupMember member, ClientCooperativeGroup group) {
            this.memberId = memberId;
            this.member = member;

            this.UpdateBuffs(this.member.Buffs());
            this.WorkshopImage.overrideSprite = SpriteCache.WorkshopSprite(member.Workshop());
            this.DisplayNameText.text = member.DisplayName();
            this.LevelText.text = member.Level().ToString();

            var zone = DataResources.Instance.ZoneForId(member.WorldId());
            if(zone != null ) {
                this.WorldNameText.text = StringCache.Get(zone.DisplayName());
            }

            if(member.Position() != null ) {
                this.WorldPositionText.text = string.Format("[{0}, {1}, {2}]", (int)member.Position()[0], (int)member.Position()[1], (int)member.Position()[2]);
            }

            if(member.IsLeader()) {
                if (!this.IsLeaderImage.gameObject.activeSelf) {
                    this.IsLeaderImage.gameObject.SetActive(true);
                }
            } else {
                if(this.IsLeaderImage.gameObject.activeSelf) {
                    this.IsLeaderImage.gameObject.SetActive(false);
                }
            }

            //if this member not leader and this member not me and i am leader show change leadership button
            if((!member.IsLeader()) && (group.IsLeader(G.Game.CharacterId())) && (member.CharacterId() != G.Game.CharacterId() )) {
                this.SetLeaderButton.gameObject.SetActive(true);
            }  else {
                this.SetLeaderButton.gameObject.SetActive(false);
            }
            
            if(group.MemberCount() <= 2 || member.CharacterId() == G.Game.CharacterId() ) {
                this.VoteForExcludeButton.gameObject.SetActive(false);
            } else {
                this.VoteForExcludeButton.gameObject.SetActive(true);
            }
        }



        private void UpdateBuffs(Hashtable newBuffs ) {

            this.buffChanges.Clear();

            foreach(DictionaryEntry entry in newBuffs ) {
                BonusType buffType = (BonusType)(byte)entry.Key;
                float buffValue = (float)entry.Value;

                if(Mathf.Approximately(buffValue, 0f)) {
                    if(this.currentBuffs.ContainsKey(buffType)) {
                        this.buffChanges.Add(new ObjectChange<BuffChangeObject> {
                            ChangeType = ChangeType.REMOVE,
                            TargetObject = new BuffChangeObject {
                                BuffType = buffType,
                                BuffValue = buffValue
                            }
                        });
                    }
                } else {
                    if(this.currentBuffs.ContainsKey(buffType)) {
                        if(!Mathf.Approximately(this.currentBuffs[buffType].BonusValue(), buffValue)) {
                            this.buffChanges.Add(new ObjectChange<BuffChangeObject> {
                                ChangeType = ChangeType.UPDATE,
                                TargetObject = new BuffChangeObject {
                                    BuffType = buffType,
                                    BuffValue = buffValue
                                }
                            });
                        }
                    } else {
                        this.buffChanges.Add(new ObjectChange<BuffChangeObject> {
                            ChangeType = ChangeType.ADD,
                            TargetObject = new BuffChangeObject {
                                BuffType = buffType,
                                BuffValue = buffValue
                            }
                        });
                    }
                }
            }

            foreach(var pcb in this.currentBuffs) {
                if(!newBuffs.ContainsKey((byte)pcb.Key)) {
                    this.buffChanges.Add(new ObjectChange<BuffChangeObject> {
                        ChangeType = ChangeType.REMOVE,
                        TargetObject = new BuffChangeObject {
                            BuffType = pcb.Key,
                            BuffValue = 0f
                        }
                    });
                }
            }

            foreach(var change in this.buffChanges ) {
                switch(change.ChangeType) {
                    case ChangeType.ADD: this.AddBuff(change); break;
                    case ChangeType.REMOVE: this.RemoveBuff(change); break;
                    case ChangeType.UPDATE: this.UpdateBuff(change); break;
                }
            }
        }

        private void RemoveBuff(ObjectChange<BuffChangeObject> change) {
            if(!this.currentBuffs.ContainsKey(change.TargetObject.BuffType)) {
                return;
            }
            GroupMemberBuffView view = this.currentBuffs[change.TargetObject.BuffType];
            this.currentBuffs.Remove(change.TargetObject.BuffType);
            Destroy(view.gameObject);
            view = null;
        }

        private void UpdateBuff(ObjectChange<BuffChangeObject> change) {
            if (!this.currentBuffs.ContainsKey(change.TargetObject.BuffType)) {
                return;
            }
            GroupMemberBuffView view = this.currentBuffs[change.TargetObject.BuffType];
            view.SetObject(change.TargetObject.BuffType, change.TargetObject.BuffValue);
        }

        private void AddBuff(ObjectChange<BuffChangeObject> change) {
            if(this.currentBuffs.ContainsKey(change.TargetObject.BuffType)) {
                UpdateBuff(change);
                return;
            }

            GameObject inst = Instantiate(BuffViewPrefab) as GameObject;
            var view = inst.GetComponent<GroupMemberBuffView>();
            view.SetObject(change.TargetObject.BuffType, change.TargetObject.BuffValue);
            view.transform.SetParent(this.BuffContent, false);
            this.currentBuffs.Add(change.TargetObject.BuffType, view);
        }

        //handler for tap SetLeader Button
        public void OnSetLeaderToCharacterButtonClick() {
            NRPC.SetLeaderToCharacter(this.member.CharacterId());
        }

        public void OnVoteForExcludeButtonClick() {
            NRPC.VoteForExclude(member.CharacterId(), member.DisplayName());
        }

        public string MemberId() {
            return this.memberId;
        }

        public class BuffChangeObject {
            public BonusType BuffType;
            public float BuffValue;
        }
    }

}