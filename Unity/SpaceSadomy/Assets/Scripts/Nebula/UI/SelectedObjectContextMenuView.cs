using Game.Network;

namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UText = UnityEngine.UI.Text;
    using UButton = UnityEngine.UI.Button;
    using UAction = UnityEngine.Events.UnityAction;
    using Nebula.Mmo.Items;

    public class SelectedObjectContextMenuView : BaseView {
        //content layout object
        public Transform Content;
        //prefab for menu button
        public GameObject ButtonPrefab;

        private InputData data;

        public override void Setup(object objData) {
            base.Setup(objData);

            if (objData == null) {
                Debug.LogError("Input data for SelectedObjectContextMenuView must be not null");
                return;
            }

            if(objData.GetType() != typeof(SelectedObjectContextMenuView.InputData)) {
                Debug.LogError("Input data for SelectedObjectContextMenuView must be valid type");
                return;
            }

            //clear all content childrens from previous setup
            this.DeleteContentChildrens();

            //set new input data
            this.data = objData as InputData;

            //initialized buttons
            foreach(var iData in this.data.Inputs ) {
                GameObject buttonInstance = Instantiate(this.ButtonPrefab) as GameObject;
                buttonInstance.name = iData.ButtonText;
                buttonInstance.GetComponentInChildren<UText>().text = iData.ButtonText;
                buttonInstance.GetComponent<UButton>().onClick.AddListener(iData.ButtonAction);
                buttonInstance.transform.SetParent(this.Content, false);
            }
        }

        private void DeleteContentChildrens() {
            foreach(Transform t in this.Content) {
                Destroy(t.gameObject);
            }
        }

        public class InputEntry {
            public string ButtonText;
            public UAction ButtonAction;
        }

        public class InputData {
            public Item TargetItem;
            public List<InputEntry> Inputs;
        }
    }

    public interface ISelectedObjectContextMenuViewSource {
        SelectedObjectContextMenuView.InputData ContextViewData();
    }
}