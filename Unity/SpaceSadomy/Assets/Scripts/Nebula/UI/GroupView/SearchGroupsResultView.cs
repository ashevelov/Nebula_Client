namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Nebula.Client;

    public class SearchGroupsResultView : MonoBehaviour {

        public GameObject GroupViewPrefab;
        public Transform Content;


        private List<SearchGroupView> groupViews = new List<SearchGroupView>();

        void Start() {
            this.UpdateViews();
        }

        void OnEnable() {
            Events.SearchGroupResultUpdated += Events_SearchGroupResultUpdated;
            
        }


        void OnDisable() {
            Events.SearchGroupResultUpdated -= Events_SearchGroupResultUpdated;
        }

        private void Events_SearchGroupResultUpdated() {
            this.UpdateViews();
        }


        private void UpdateViews() {
            foreach(var g in this.groupViews) {
                Destroy(g.gameObject);
            }
            this.groupViews.Clear();

            foreach(var g in GameData.instance.searchGroupResult.Groups() ) {
                GameObject instance = Instantiate(this.GroupViewPrefab) as GameObject;
                instance.GetComponent<SearchGroupView>().SetObject(g.Value);
                instance.transform.SetParent(this.Content, false);
                this.groupViews.Add(instance.GetComponent<SearchGroupView>());
            }
        }

        public void OnRefreshButtonClick() {
            //NRPC.RequestOpenedGroups();
        }
    }
}
