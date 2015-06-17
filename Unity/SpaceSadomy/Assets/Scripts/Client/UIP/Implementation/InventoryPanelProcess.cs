using UnityEngine;
using System.Collections;
using Nebula.Client.Inventory;
using System.Collections.Generic;
using UIC;

public class InventoryPanelProcess : MonoBehaviour {

    private IInventoryPanel uicPanel;

    private List<ClientInventoryItem> currentItems =new List<ClientInventoryItem>();

	void Start () {
        StartCoroutine(UpdateInfo());
    }

    IEnumerator UpdateInfo()
    {
        if (CheckCondition())
        {
            List<ClientInventoryItem> newItems = null;
            newItems = G.Inventory().OrderedItems();

            //first search new and modified items in new item list
            foreach (var item in newItems)
            {
                var foundedItem = this.currentItems.Find(it => it.Object.Id == item.Object.Id);
                if (foundedItem == null)
                {
                    uicPanel.AddItem(new InventoryItem(item.Object.Id, "name hz", item.Object.Type.ToString(), item.Count, 9999));
                }
                else if (foundedItem.Count != item.Count)
                {
                    uicPanel.ModifiedItem(item.Object.Id, item.Count);
                }
            }

            //then search items not presented in new list ( its removed )
            foreach (var cItem in currentItems)
            {
                var foundedItem = newItems.Find(it => it.Object.Id == cItem.Object.Id);
                if (foundedItem == null)
                {
                    uicPanel.RemoveItem(cItem.Object.Id);
                }
            }

            this.currentItems = newItems;
            uicPanel.AddItem(new InventoryPanelItem());
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateInfo());
    }

    public bool CheckCondition()
    {
        if (uicPanel == null)
        {
            uicPanel = FindObjectOfType<InventoryPanel>();
        }
        if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
            return false;
        return true;
    }
}
