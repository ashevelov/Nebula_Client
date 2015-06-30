namespace Nebula.UI {
    using Common;
    using Nebula.Client;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class StationModulesView : MonoBehaviour {


        public Transform Content;
        public GameObject ModuleViewPrefab;
        public Button EquipButton;
        public Button InfoButton;
        public Button DestroyButton;

        private List<ClientShipModule> newModules = new List<ClientShipModule>();
        private List<ClientShipModule> currentModules = new List<ClientShipModule>();
        private List<ObjectChange<ClientShipModule>> changes = new List<ObjectChange<ClientShipModule>>();
        private ClientShipModule selectedModule;

        void Start() {
            this.UpdateModules();
            StartCoroutine(C_UpdateModules());
        }

        void OnEnable() {
            Events.StationUpdated += Events_StationUpdated;
        }



        void OnDisable() {
            Events.StationUpdated -= Events_StationUpdated;
            StopAllCoroutines();
        }

        private void Events_StationUpdated() {
            this.UpdateModules();
        }

        private IEnumerator C_UpdateModules() {
            while(true) {
                yield return new WaitForSeconds(Settings.STATION_UPDATE_INTERVAL);
                this.UpdateModules();
            }
        }


        void UpdateModules() {

            if(GameData.instance.station == null ) {
                return;
            }

            newModules.Clear();
            changes.Clear();

            Dictionary<string, IStationHoldableObject> moduleObjects = new Dictionary<string, IStationHoldableObject>();
            Hashtable moduleObjectsHash = null;
            if(GameData.instance.station.Hold.TryGetObjects(StationHoldableObjectType.Module, out moduleObjectsHash)) {
                foreach(DictionaryEntry entry in moduleObjectsHash) {
                    moduleObjects.Add(entry.Key.ToString(), entry.Value as IStationHoldableObject);
                }
            }

            foreach(var moduleObj in moduleObjects) {
                newModules.Add(moduleObj.Value as ClientShipModule);
            }

            foreach(var module in newModules) {
                var foundedModule = this.currentModules.Find(m => m.Id == module.Id);
                if(foundedModule == null ) {
                    changes.Add(new ObjectChange<ClientShipModule> { ChangeType = ChangeType.ADD, TargetObject = module });
                }
            }

            foreach(var module in this.currentModules) {
                var foundedModule = newModules.Find(m => m.Id == module.Id);
                if(foundedModule == null ) {
                    changes.Add(new ObjectChange<ClientShipModule> { ChangeType = ChangeType.REMOVE, TargetObject = module });
                }
            }

            ModuleView[] moduleViews = this.Content.GetComponentsInChildren<ModuleView>();

            foreach(var change in changes ) {
                switch(change.ChangeType) {
                    case ChangeType.ADD:
                        AddModule(change.TargetObject);
                        break;
                    case ChangeType.REMOVE:
                        RemoveModule(moduleViews, change.TargetObject);
                        break;
                }
            }
            //this.currentModules = this.newModules;
        }

        private void AddModule(ClientShipModule module ) {
            this.currentModules.Add(module);
            GameObject inst = Instantiate(ModuleViewPrefab) as GameObject;
            inst.transform.SetParent(this.Content, false);
            var moduleView = inst.GetComponent<ModuleView>();
            moduleView.ModuleSelectToggle.group = this.Content.GetComponent<ToggleGroup>();
            moduleView.SetObject(module, (isOn) => {
                if(isOn) {
                    SelectModule(module);
                }
            });
        }

        private void RemoveModule(ModuleView[] source, ClientShipModule module ) {
            this.currentModules.Remove(module);
            for(int i = 0; i < source.Length; i++ ) {
                if(source[i] == null ) {
                    continue;
                }
                if(source[i].Module().Id == module.Id ) {
                    Destroy(source[i].gameObject);
                    source[i] = null;
                }
            }
        }

        //Called when Equip button tapped
        public void OnEquipButtonClick() {
            
            if(this.selectedModule == null ) {
                Debug.LogWarning("No selected module");
                return;
            }

            NRPC.EquipModuleFromHoldToShip(this.selectedModule.Id);
            this.SelectModule(null);
        }

        //Called when Info module button tapped
        public void OnInfoButtonClick() {
            if(this.selectedModule == null ) {
                Debug.LogWarning("No selected module");
                return;
            }

            ItemInfoView.ItemContentData data = new ItemInfoView.ItemContentData { IsOnPlayer = false, Data = this.selectedModule };
            MainCanvas.Get.Show(CanvasPanelType.ItemInfoView, data);
        }

        public void OnDestroyButtonClick() {
            if (this.selectedModule == null) {
                Debug.LogWarning("No selected module");
                return;
            }
            NRPC.DestroyModule(this.selectedModule.Id);
            SelectModule(null);
        }

        private void SelectModule(ClientShipModule module) {
            this.selectedModule = module;
            if(this.selectedModule == null ) {
                this.EquipButton.interactable = false;
                this.InfoButton.interactable = false;
                this.DestroyButton.interactable = false;
            } else {
                this.EquipButton.interactable = true;
                this.InfoButton.interactable = true;
                this.DestroyButton.interactable = true;
            }
        }
    }
}
