using Game.Space;
namespace Nebula.UI {

	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	using System.Collections.Generic;
    using Common;

	public class MainCanvas : Singleton<MainCanvas> {

	    private static bool alreadyCreated = false;
		private readonly ObjectSubCache<GameObject, CanvasPanelType> panelPrefabs = new ObjectSubCache<GameObject, CanvasPanelType>();

        private readonly ObjectSubCache<GameObject, string> miscPrefabs = new ObjectSubCache<GameObject, string>();

		private readonly Dictionary<CanvasPanelType, string> panelPathDictionary = new Dictionary<CanvasPanelType, string>();
        private readonly Dictionary<CanvasPanelType, GameObject> existingViews = new Dictionary<CanvasPanelType, GameObject>();

        private RectTransform objectScreenPanel;

		private Canvas canvas;


	    void Awake() {
	        if (!alreadyCreated) {
	            alreadyCreated = true;
	            DontDestroyOnLoad(gameObject);
	        } else {
	            Destroy(gameObject);
	            return;
	        }
	    }

	    void Start() {
			this.canvas = GetComponent<Canvas>();
	        panelPathDictionary.Add(CanvasPanelType.MailBox,                            "Prefabs/UI/Mail_Box_Panel"                     );
            panelPathDictionary.Add(CanvasPanelType.LoginView,                          "Prefabs/UI/LoginView"              );
            panelPathDictionary.Add(CanvasPanelType.SelectCharacterView,                "Prefabs/UI/Select_Character_View"  );
            panelPathDictionary.Add(CanvasPanelType.CreateCharacterOrPlayView,          "Prefabs/UI/Create_Play_View"       );
            panelPathDictionary.Add(CanvasPanelType.SelectRaceView,                     "Prefabs/UI/Select_Race_View"       );
            panelPathDictionary.Add(CanvasPanelType.SelectWorkshopView,                 "Prefabs/UI/Select_Workshop_View"   );
            panelPathDictionary.Add(CanvasPanelType.ControlHUDView,                     "Prefabs/UI/Control_HUD_View"       );
            panelPathDictionary.Add(CanvasPanelType.TargetObjectView,                   "Prefabs/UI/Target_Info_View"       );
            panelPathDictionary.Add(CanvasPanelType.MenuHUDView,                        "Prefabs/UI/Menu_HUD_View"          );
            panelPathDictionary.Add(CanvasPanelType.ChatView,                           "Prefabs/UI/Chat_View"              );
            panelPathDictionary.Add(CanvasPanelType.InventoryView,                      "Prefabs/UI/Inventory_View"                         );
            panelPathDictionary.Add(CanvasPanelType.SelectedObjectContextMenuView,      "Prefabs/UI/Selected_Object_Context_Menu_View"      );
            panelPathDictionary.Add(CanvasPanelType.TooltipView,                        "Prefabs/UI/Tooltip_View"                           );
            panelPathDictionary.Add(CanvasPanelType.ItemInfoView,                       "Prefabs/UI/ItemInfoView/Item_Info_View");
            panelPathDictionary.Add(CanvasPanelType.InventorySourceView,                "Prefabs/UI/Inventory_Source_View");
            panelPathDictionary.Add(CanvasPanelType.ShipInfoView,                       "Prefabs/UI/ShipInfoVeiw");
            //panelPathDictionary.Add(CanvasPanelType.EventTasksView,                     "Prefabs/UI/Event_Tasks_View");
            panelPathDictionary.Add(CanvasPanelType.BuffsView,                          "Prefabs/UI/Buffs_View");
            panelPathDictionary.Add(CanvasPanelType.SchemeSelectorView,                 "Prefabs/UI/Scheme_Selector_View");
            panelPathDictionary.Add(CanvasPanelType.ActionProgressView,                 "Prefabs/UI/Action_Progress_View");
            panelPathDictionary.Add(CanvasPanelType.SchemeCraftView,                    "Prefabs/UI/Scheme_Craft_View");
            panelPathDictionary.Add(CanvasPanelType.StationView,                        "Prefabs/UI/Station_View");
            panelPathDictionary.Add(CanvasPanelType.StationHUD,                         "Prefabs/UI/Station_HUD_View");
            panelPathDictionary.Add(CanvasPanelType.MessageBox,                         "Prefabs/UI/Message_Box_View");
            panelPathDictionary.Add(CanvasPanelType.GroupView,                          "Prefabs/UI/Group_View");
            panelPathDictionary.Add(CanvasPanelType.ShipDestroyView,                    "Prefabs/UI/ShipDestroyView");
            

            Show(CanvasPanelType.TargetObjectView);
            Show(CanvasPanelType.BuffsView);

            this.objectScreenPanel = gameObject.GetChildrenWithName("ObjectScreenPanel").GetComponent<RectTransform>();
            this.miscPrefabs.Preload("TargetView", "Prefabs/UI/Target_View");
	    }

        public static bool ObjectIconsEnabled() {
            return true;
        }

        void OnEnable() { 
            Events.GameStateChanged += Events_GameStateChanged;
            Events.CooperativeGroupUpdated += Events_CooperativeGroupUpdated;
            Events.GameBehaviourChanged += Events_GameBehaviourChanged;
        }



        void OnDisable() {
            Events.GameStateChanged -= Events_GameStateChanged;
            Events.CooperativeGroupUpdated -= Events_CooperativeGroupUpdated;
            Events.GameBehaviourChanged -= Events_GameBehaviourChanged;
        }
        private void Events_GameBehaviourChanged(Mmo.Games.GameType gameType, GameState gameState) {
            if(gameType == Mmo.Games.GameType.Login && gameState == GameState.LoginConnected) {
                Show(CanvasPanelType.LoginView);
            } else {
                Destroy(CanvasPanelType.LoginView);
            }
        }

        void Events_GameStateChanged(GameState oldState, GameState newState) {
            //if (newState == GameState.Connected) {
            //    if (!this.Exists(CanvasPanelType.LoginView)) {
            //        Show(CanvasPanelType.LoginView);
            //    }
            //} else {
            //    if (this.Exists(CanvasPanelType.LoginView)) {
            //        Destroy(CanvasPanelType.LoginView);
            //    }
            //}

            //show station HUD at station mode
            if(newState == GameState.NebulaGameWorkshopEntered) {
                Show(CanvasPanelType.StationHUD);
            } else {
                if (Exists(CanvasPanelType.StationHUD)) {
                    this.Destroy(CanvasPanelType.StationHUD);
                }
            }
        }

        private void Events_CooperativeGroupUpdated(Client.ClientCooperativeGroup obj) {
            //if(obj.HasGroup()) {
            //    if (!Exists(CanvasPanelType.GroupView)) {
            //        Show(CanvasPanelType.GroupView);
            //    }
            //} else {
            //    if (Exists(CanvasPanelType.GroupView)) {
            //        Destroy(CanvasPanelType.GroupView);
            //    }
            //}
        }


        public Canvas Canvas() {
	        return this.canvas;
	    }

        public void ToggleView(CanvasPanelType type, object arg = null, System.Action willDestroy = null) {
            if (this.Exists(type)) {
                this.Destroy(type);
            } else {
                this.Show(type, arg, willDestroy);
            }
        } 

	    public void Show(CanvasPanelType panelType, object arg = null, System.Action willDestroy = null) {
            print("Show {0} view".f(panelType));
            if (this.existingViews.ContainsKey(panelType)) {
                //Debug.LogError("Already exists view: {0}. Creation failed. Maker re setup".f(panelType));
                if (this.existingViews[panelType] != null) {
                    this.existingViews[panelType].GetComponentInChildren<BaseView>().Setup(arg);
                    this.existingViews[panelType].GetComponentInChildren<BaseView>().SetWillDestroyHandler(willDestroy);
                }
                return;
            }
	        GameObject obj = Instantiate(panelPrefabs.GetObject(panelType, panelPathDictionary[panelType])) as GameObject;
            if (obj.GetComponentInChildren<BaseView>() != null) {
                obj.GetComponentInChildren<BaseView>().Setup(arg);
                obj.GetComponentInChildren<BaseView>().SetWillDestroyHandler(willDestroy);
            } 
	        obj.transform.SetParent(Canvas().transform, false);

            this.existingViews.Add(panelType, obj);
            
	    }

        public void Destroy(CanvasPanelType panelType) {
            if (!this.existingViews.ContainsKey(panelType)) {
                //Debug.LogError("Not exists panel of type: {0}. Delete failed".f(panelType));
                return;
            }
            if (this.existingViews[panelType] != null) {

                //call on will destroy action before actually destroying
                var baseView = this.existingViews[panelType].GetComponentInChildren<BaseView>();
                if (baseView != null) {
                    baseView.OnWillDestroy();
                }
                Destroy(this.existingViews[panelType]);
            }
            this.existingViews.Remove(panelType);
        }

        public bool Exists(CanvasPanelType panelType) {
            return this.existingViews.ContainsKey(panelType);
        }
        public GameObject GetView(CanvasPanelType panelType) {
            GameObject ret = null;
            this.existingViews.TryGetValue(panelType, out ret);
            return ret;
        }

        public GameObject GetMiscPrefab(string path) {
            return this.miscPrefabs.GetObject(path, path);
        }

        public void SetTarget(IObjectInfo objectInfo) {
            if (!this.existingViews.ContainsKey(CanvasPanelType.TargetObjectView)) {
                this.Show(CanvasPanelType.TargetObjectView);
            }
            this.existingViews[CanvasPanelType.TargetObjectView].GetComponent<TargetInfoView>().SetTarget(objectInfo);
        }

        public RectTransform ObjectScreenPanel() {
            return this.objectScreenPanel;
        }

        //void Update() {
        //    if(Input.GetKeyDown(KeyCode.Alpha1)) {
        //        MessageBoxView.MessageBoxConfig config = new MessageBoxView.MessageBoxConfig {
        //            ButtonActions = new System.Action[] { () => { Debug.Log("first clicked"); }, () => { Debug.Log("Second clicked"); }, () => { Debug.Log("Third clicked"); } },
        //            ButtonNames = new string[] { "First Butt", "Second Butt", "Third Butt" },
        //            CountOfButtons = Random.Range(1, 4),
        //            HasImage = false,
        //            Icon = SpriteCache.RaceSprite(Race.Criptizoids),
        //            Text = "Assets/ThirdPartyAssets/Marmoset/External/Lightmapping Extended/Editor/LMExtendedWindow.cs(635,40): warning CS0618: `UnityEditor.LightmapEditorSettings.aoContrast' is obsolete: `LightmapEditorSettings.aoContrast has been deprecated.'",
        //            Title = "Title for Messafe Box"
        //        };
        //        Show(CanvasPanelType.MessageBox, config);
        //    }
        //}
	}

	public enum CanvasPanelType : byte {
        MailBox,
        LoginView,
        SelectCharacterView,
        CreateCharacterOrPlayView,
        SelectRaceView,
        SelectWorkshopView,
        ControlHUDView,
        TargetObjectView,
        MenuHUDView,
        ChatView,
        InventoryView,
        SelectedObjectContextMenuView,
        TooltipView,
        ItemInfoView,
        InventorySourceView,
        ShipInfoView,
       // EventTasksView,
        BuffsView,
        SchemeSelectorView,
        ActionProgressView,
        SchemeCraftView,
        StationView,
        StationHUD,
        MessageBox,
        GroupView,
        ShipDestroyView
    }
}
