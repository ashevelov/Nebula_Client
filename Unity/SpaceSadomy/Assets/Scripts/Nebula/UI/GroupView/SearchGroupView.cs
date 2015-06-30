namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Client;
    using UnityEngine.UI;
    using System.Linq;
    using System.Collections.Generic;

    public class SearchGroupView : MonoBehaviour {
        public SearchGroupMemberView[] MemberViews;
        public Button JoinButton;

        private ClientSearchGroup group;
        public void SetObject(ClientSearchGroup group) {
            this.group = group;

            List<ClientSearchGroupMember> members = group.Members().Select(kv => kv.Value).OrderByDescending(m => m.Level()).ToList();

            for(int i = 0; i < this.MemberViews.Length; i++) {
                if(i < members.Count) {
                    this.MemberViews[i].SetObject(members[i]);
                } else {
                    this.MemberViews[i].SetObject(null);
                }
            }

            //if (G.Game.CooperativeGroup().HasGroup()) {
            //    this.JoinButton.gameObject.SetActive(false);
            //}
        }

        //Handler Join Button
        public void OnJoinOpenedGroupButtonClick() {
            //NRPC.JoinToOpenedGroup(group.Id());
        }
    }
}
