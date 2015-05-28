namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Nebula.Client;
    using UnityEngine.UI;

    public class GroupView : MonoBehaviour {

        public GameObject MemberViewPrefab;
        public Transform MemberViewContent;
        public Button ExitFromGroupButton;
        public Button FreeGroupButton;
        public Toggle OpenGroupToggle;

        private Dictionary<string, GroupMemberView> currentViews = new Dictionary<string, GroupMemberView>();
        private List<ObjectChange<MemberChangeObject>> changes = new List<ObjectChange<MemberChangeObject>>();

        void Start() {
            UpdateViews(G.Game.CooperativeGroup());
        }

        void OnEnable() {
            Events.CooperativeGroupUpdated += Events_CooperativeGroupUpdated;
        }

        void OnDisable() {
            Events.CooperativeGroupUpdated -= Events_CooperativeGroupUpdated;
        }

        private void Events_CooperativeGroupUpdated(ClientCooperativeGroup obj) {
            this.UpdateViews(obj);
        }



        private void UpdateViews(ClientCooperativeGroup group) {

            changes.Clear();

            //even if only one (me) player we don't show group. Group with single player don't exists, need 2 players
            if (!group.HasGroup()) {
                this.DestroyAllViews();
                this.ExitFromGroupButton.gameObject.SetActive(true);
                return;
            } else {
                if(!this.ExitFromGroupButton.gameObject.activeSelf) {
                    this.ExitFromGroupButton.gameObject.SetActive(true);
                }
            }

            var newMembers = group.MembersDict();

            foreach (var pMember in newMembers ) {
                if(this.currentViews.ContainsKey(pMember.Key)) {
                    changes.Add(new ObjectChange<MemberChangeObject> {
                        ChangeType = ChangeType.UPDATE,
                        TargetObject = new MemberChangeObject {
                            MemberId = pMember.Key,
                            Member = pMember.Value
                        }
                    });
                } else {
                    changes.Add(new ObjectChange<MemberChangeObject> {
                        ChangeType = ChangeType.ADD,
                        TargetObject = new MemberChangeObject {
                            MemberId = pMember.Key,
                            Member = pMember.Value
                        }
                    });
                }
            }

            foreach(var pCurMember in this.currentViews ) {
                if(!newMembers.ContainsKey(pCurMember.Key)) {
                    changes.Add(new ObjectChange<MemberChangeObject> {
                        ChangeType = ChangeType.REMOVE,
                        TargetObject = new MemberChangeObject {
                            MemberId = pCurMember.Key,
                            Member = null
                        }
                    });
                }
            }

            foreach(var change in this.changes) {
                switch(change.ChangeType) {
                    case ChangeType.ADD: AddMember(change, group); break;
                    case ChangeType.REMOVE: RemoveMember(change); break;
                    case ChangeType.UPDATE: UpdateMember(change, group); break;
                }
            }

            this.HandleFreeGroupButtonVisibility(group);
            this.HandleOpenGroupToggle(group);
        }

        private void HandleOpenGroupToggle(ClientCooperativeGroup group) {
            this.OpenGroupToggle.onValueChanged.RemoveAllListeners();
            if (!group.HasGroup()) {
                if(this.OpenGroupToggle.gameObject.activeSelf)
                    this.OpenGroupToggle.gameObject.SetActive(false);
                return;
            } else {
                if (!this.OpenGroupToggle.gameObject.activeSelf)
                    this.OpenGroupToggle.gameObject.SetActive(true);
            }

            if (!group.IsLeader(G.Game.CharacterId())) {
                this.OpenGroupToggle.gameObject.SetActive(false);
                return;
            }

            if (group.Opened()) {
                if (!this.OpenGroupToggle.isOn) {
                    this.OpenGroupToggle.isOn = true;
                }
            } else {
                if (this.OpenGroupToggle.isOn) {
                    this.OpenGroupToggle.isOn = false;
                }
            }

            this.OpenGroupToggle.onValueChanged.AddListener((opened) => {
                if(group.IsLeader(G.Game.CharacterId())) {
                    NRPC.SetGroupOpened(opened);
                }
            });
        }

        //if my me is leader show Free Group Button else hide it
        private void HandleFreeGroupButtonVisibility(ClientCooperativeGroup group ) {
            ClientCooperativeGroupMember myMember = null;
            string myCharacterId = G.Game.CharacterId();
            if (string.IsNullOrEmpty(myCharacterId)) {
                Debug.LogError("error: No selected character");
                return;
            }
            if (!group.TryGetMember(myCharacterId, out myMember)) {
                Debug.LogError("error: my group member not found");
                return;
            }

            if (!myMember.IsLeader()) {
                this.FreeGroupButton.gameObject.SetActive(false);
            } else {
                this.FreeGroupButton.gameObject.SetActive(true);
            }
        }

        private void AddMember(ObjectChange<MemberChangeObject> change, ClientCooperativeGroup group) {

            GameObject inst = Instantiate(MemberViewPrefab) as GameObject;
            var view = inst.GetComponent<GroupMemberView>();
            view.SetObject(change.TargetObject.MemberId, change.TargetObject.Member, group);
            view.transform.SetParent(this.MemberViewContent, false);
            this.currentViews.Add(change.TargetObject.MemberId, view);
        }

        private void UpdateMember(ObjectChange<MemberChangeObject> change, ClientCooperativeGroup group) {
            if(this.currentViews.ContainsKey(change.TargetObject.MemberId)) {
                this.currentViews[change.TargetObject.MemberId].SetObject(change.TargetObject.MemberId, change.TargetObject.Member, group);
            }
        }

        private void RemoveMember(ObjectChange<MemberChangeObject> change) {

            string mId = change.TargetObject.MemberId;
            if(this.currentViews.ContainsKey(mId)) {
                var view = this.currentViews[mId];
                this.currentViews.Remove(mId);
                Destroy(view.gameObject);
                view = null;
            }
        }

        private void DestroyAllViews() {
            foreach(var pMember in currentViews) {
                Destroy(pMember.Value.gameObject);
            }
            this.currentViews.Clear();
        }

        //Click handler Exit From Group button, exit me from current group
        public void OnExitFromCurrentGroupButtonClicked() {
            NRPC.ExitFromCurrentGroup();
        }

        //Click handler Free Group Button, free all group
        public void OnFreeGroupButtonClick() {
            NRPC.FreeGroup();
        }

        //Object type for hold changes in groups
        public class MemberChangeObject {
            public string MemberId;
            public ClientCooperativeGroupMember Member;
        }


    } 

}
