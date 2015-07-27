using UnityEngine;
using System.Collections;
using Nebula.Client.Auction;
using Nebula.Mmo.Games;
using System.Collections.Generic;
using Nebula.UI;
using System.Linq;
using UnityEngine.UI;

namespace Nebula.Test {
    public class TestAuction : MonoBehaviour {

        public Transform content;
        public GameObject auctionItemView;

        private List<ObjectChange<AuctionItem>> mChanges = new List<ObjectChange<AuctionItem>>();

        private List<TestAuctionItemView> items = new List<TestAuctionItemView>();

        void OnEnable() {

            StopAllCoroutines();
            StartCoroutine(CorUpdate());
            Debug.Log("Coroutine started");

            SelectCharacterGame.Instance().GetCurrentAuctionPage(false);

            int maxSibling = -1;
            foreach(Transform t in MainCanvas.Get.transform) {
                if(t.GetSiblingIndex() > maxSibling && t.name != name ) {
                    maxSibling = t.GetSiblingIndex();
                }
            }
            transform.SetSiblingIndex(maxSibling + 1);
            Debug.LogFormat("setted sibling index to {0}", maxSibling + 1);

        }

        private IEnumerator CorUpdate() {
            
            if (GameData.instance.auction.items != null) {
                CollectChanges(GameData.instance.auction.items);
                ApplyChanges();
            }
            yield return new WaitForSeconds(2);
            StartCoroutine(CorUpdate());
        }

        private void CollectChanges(List<AuctionItem> source) {

            mChanges.Clear();

            foreach(var item in source ) {
                if (!Contains(item)) {
                    mChanges.Add(new ObjectChange<AuctionItem> { ChangeType = ChangeType.ADD, TargetObject = item });
                }
            }

            foreach(var item in items.Select(it => it.item).ToList()) {
                var founded = source.Find((it) => it.storeItemId == item.storeItemId);
                if(founded == null ) {
                    mChanges.Add(new ObjectChange<AuctionItem> { ChangeType = ChangeType.REMOVE, TargetObject = item });
                }
            }
        }

        private void ApplyChanges() {

            foreach(var change in mChanges ) {
                if(change.ChangeType == ChangeType.ADD ) {
                    GameObject instance = (GameObject)Instantiate(auctionItemView);
                    instance.transform.SetParent(content, false);
                    instance.GetComponent<TestAuctionItemView>().selectionToggle.group = content.GetComponent<ToggleGroup>();
                    instance.GetComponent<TestAuctionItemView>().Set(change.TargetObject);
                    items.Add(instance.GetComponent<TestAuctionItemView>());
                } else if(change.ChangeType == ChangeType.REMOVE ) {
                    TestAuctionItemView targetView = items.Find(it => it.item.storeItemId == change.TargetObject.storeItemId);
                    if(targetView != null ) {
                        items.Remove(targetView);
                        Destroy(targetView.gameObject);
                    }
                    
                }
            }
            mChanges.Clear();
        }

        private bool Contains(AuctionItem item) {
            foreach(var it in items) {
                if(it.item.storeItemId == item.storeItemId) {
                    return true;
                }
            }
            return false;
        }

        public void OnReloadClick() {
            //SelectCharacterGame.Instance().GetCurrentAuctionPage()
            bool reset = GameData.instance.auction.filtersChanged;

            SelectCharacterGame.Instance().GetCurrentAuctionPage(reset);
            if(reset) {
                GameData.instance.auction.ResetChanged();
            }

        }

        public void OnNextPageClick() {
            SelectCharacterGame.Instance().GetNextAuctionPage();
        }

        public void OnPrevPageClick() {
            SelectCharacterGame.Instance().GetPrevAuctionPage();
        }
    }
}
