/*
namespace Game.Space.UI 
{
    using Common;
    using Game.Network;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class ChatView
    {

        private string prevText;
        private bool needScrollToNewMessage;
        private bool needScrollLogToLastMessage;

        private UIContainerEntity root;
        private UIButton closeButton;

        private UIScrollView chatScroll;
        //private UIScrollView logScroll;

        private UICombobox combobox;
        private UITextfield input;
        private UIButton sendButton;
        private UIGroup chatGroup;
        private UILabel newMessagesLabel;

        private int historyIndex;
        private List<string> history = new List<string>();

        public ChatView()
        {
            this.prevText = string.Empty;
            this.needScrollToNewMessage = false;
            this.root = G.UI.GetLayout("chat") as UIContainerEntity;
            this.chatGroup = this.root.GetChild<UIGroup>("chat_parent");
            this.newMessagesLabel = this.root.GetChild<UILabel>("new_messages");

            this.root.SetBlockMouse(false, true);

            this.closeButton =      this.root.GetChildrenEntityByName("close_button")   as UIButton;
            this.chatScroll =       this.root.GetChildrenEntityByName("chat_scroll")    as UIScrollView;
            this.combobox =         this.root.GetChildrenEntityByName("chat_group")     as UICombobox;
            this.input =            this.root.GetChildrenEntityByName("input_message")  as UITextfield;
            this.sendButton =       this.root.GetChildrenEntityByName("send_button")    as UIButton;

            this.chatScroll.SetUpdateInterval(NetworkGame.CHAT_UPDATE_INTERVAL);
            this.chatScroll.RegisterUpdate(Update);

            G.UI.RegisterEventHandler("SEND_MESSAGE_CLICK", null, OnSendMessage);
            G.UI.RegisterEventHandler("CHAT_MSG_KEY_ENTER", null, OnSendMessage);
            G.UI.RegisterEventHandler("CHAT_CLOSE_CLICK", null, (e) => this.SetVisible(false) );

            this.root.RegisterUpdate(RootUpdate);
        }

        private float hideTimer = 0f;
        private bool mouseOverChatOnPrevFrame;
        private bool mouseOverChatOnCurrentFrame;

        private bool IsAutoscroll()
        {
            return false;
        }

        private int CountOfNewMessages()
        {
            return G.Game.Chat.CountOfNewMessages();
        }

        private bool CheckNewMessagesLabelTag(int count)
        {
            if (newMessagesLabel.tag == null)
                return false;
            return ((int)newMessagesLabel.tag == count);
        }

        private void CheckAndStartNewMessagesAnimtion()
        {
            if (newMessagesLabel.GetAnimation("change_color").State != UIAnimation.UIAnimationState.Playing)
                newMessagesLabel.GetAnimation("change_color").Play();
        }

        private void StopNewMessagesAnimtion()
        {
            if (newMessagesLabel.GetAnimation("change_color").State == UIAnimation.UIAnimationState.Playing)
                newMessagesLabel.GetAnimation("change_color").Stop();
        }

        private void HideNewMessages()
        {
            if (newMessagesLabel.tag != null)
            {
                newMessagesLabel.SetTag(null);
                newMessagesLabel.SetText(string.Empty);
                StopNewMessagesAnimtion();
            }
        }

        private void RootUpdate(UIContainerEntity r)
        {
            if(this.Visible)
            {
                mouseOverChatOnPrevFrame = mouseOverChatOnCurrentFrame;
                mouseOverChatOnCurrentFrame = this.chatGroup.GetGlobalRect.Contains(G.UI.MousePosition);

                
                if(!mouseOverChatOnCurrentFrame)
                {
                    hideTimer += Time.deltaTime;
                }
                else if(mouseOverChatOnCurrentFrame && !mouseOverChatOnPrevFrame)
                {
                    hideTimer = 0f;
                    this.chatGroup.SetUseCustomColor(false, Color.white, true, new List<string> { "chat_message" });
                }
                else if(mouseOverChatOnCurrentFrame)
                {
                    hideTimer = 0f;
                }

                if(hideTimer >= 3.0f )
                {
                    this.chatGroup.SetUseCustomColor(true, new Color(1, 1, 1, 0), true, new List<string> { "chat_message" });
                }

                if(!IsAutoscroll() && CountOfNewMessages() > 0 )
                {
                    if (!CheckNewMessagesLabelTag(CountOfNewMessages()))
                    {
                        newMessagesLabel.SetTag(CountOfNewMessages());
                        CheckAndStartNewMessagesAnimtion();
                    } 
                }
                else
                {
                    HideNewMessages();
                }
            }
            else
            {
                if (CountOfNewMessages() > 0)
                {
                    if (!CheckNewMessagesLabelTag(CountOfNewMessages()))
                    {
                        newMessagesLabel.SetTag(CountOfNewMessages());
                        CheckAndStartNewMessagesAnimtion();
                    }
                }
                else
                {
                    HideNewMessages();
                }

                hideTimer = 0f;
            }
        }

        private void Update(UIContainerEntity e)
        {
            this.chatScroll.Clear();
            foreach (var message in G.Game.Chat.Messages())
            {
                var t = this.chatScroll.CreateElement();
                UILabel msgLabel = t.GetChildrenEntityByName("chat_message") as UILabel;
                msgLabel.SetText(message.DecoratedMessage);
                t.useComputedHeight = true;
                t.tag = "chat_message";
                t.SetUseCustomColor(true, Color.white);
                t.SetComputeHeight(size =>
                    {
                        return msgLabel._style.CalcHeight(new GUIContent(msgLabel._text), size.x) + 5;
                    });
                this.chatScroll.AddChild(t);
            }

            if (this.Visible)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    historyIndex--;
                    historyIndex = Mathf.Clamp(historyIndex, 0, history.Count - 1);
                    if (historyIndex >= 0 && historyIndex < history.Count)
                    {
                        this.input.SetText(history[historyIndex]);
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    historyIndex++;
                    historyIndex = Mathf.Clamp(historyIndex, 0, history.Count);
                    if (historyIndex >= 0 && historyIndex < history.Count)
                    {
                        this.input.SetText(history[historyIndex]);
                    }
                }
            }
        }

        private string[] SubArray(string[] array, int countSkip) {
            int newSize = array.Length - countSkip;
            if (newSize <= 0) {
                return new string[] { };
            }
            string[] newArray = new string[newSize];
            for (int i = 0; i < newArray.Length; i++) {
                newArray[i] = array[countSkip + i];
            }
            return newArray;
        }

        private void OnSendMessage(UIEvent evt)
        {
            if (string.IsNullOrEmpty(input.Text))
            {
                return;
            }

            history.Add(input.Text);
            historyIndex = history.Count;

            int spaceIndex = input.Text.IndexOf(' ');
            if (spaceIndex > 0)
            {
                string firstWorld = input.Text.Substring(0, spaceIndex);
                if (firstWorld == "/cmd" || firstWorld == "/c")
                {
                    string[] words = input.Text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length > 1)
                    {
                        words = SubArray(words, 1);
                        this.ExecCommand(words);
                    }
                    return;
                }
            }

            if (G.Game.Avatar == null)
            {
                return;
            }

            var player = G.Game.Avatar;
            if (player != null)
            {
                string login;
                string message;
                this.GetLoginAndMessage(this.input.Text, out login, out message);
                if (false == string.IsNullOrEmpty(message))
                {
                    G.Game.SendChatMessage(this.GetChatGroup(this.combobox), message, login);
                }
                G.Game.GetChatUpdate();
                this.prevText = this.input.Text;
                this.input.SetText(string.Empty);
            }
        }

        private void GetLoginAndMessage(string inpText, out string login, out string message)
        {
            login = string.Empty;
            message = string.Empty;
            if (string.IsNullOrEmpty(inpText))
                return;
            if (inpText.StartsWith("@"))
            {
                int spaceIndex = inpText.IndexOf(' ');
                if (spaceIndex >= 0)
                {
                    login = inpText.Substring(0, spaceIndex);
                    if (spaceIndex + 1 < inpText.Length)
                    {
                        message = inpText.Substring(spaceIndex + 1);
                    }
                    else
                    {
                        message = string.Empty;
                    }
                }
            }
            else
            {
                message = inpText;
            }
        }

        private ChatGroup GetChatGroup(UICombobox combo)
        {
            switch (this.combobox.SelectedContent.text)
            {
                case "zone":
                    return ChatGroup.zone;
                case "all":
                    return ChatGroup.all;
                case "whisper":
                    return ChatGroup.whisper;
                case "echo":
                    return ChatGroup.me;
                default:
                    return ChatGroup.all;
            }
        }

        private void ExecCommand(string[] command)
        {
            if (command.Length == 0)
                return;
            switch (command[0])
            {
                case "msg":
                    if (command.Length == 1)
                        MmoEngine.Get.Game.Avatar.TestSendServiceMessage(ServiceMessageType.Info, "hello, Nebula Online!");
                    else
                        MmoEngine.Get.Game.Avatar.TestSendServiceMessage(ServiceMessageType.Info, command.CustomJoin(1, " "));
                    break;
                case "use_skill":
                    //MmoEngine.Get.Game.Avatar.TestUseSkill(command[1]);
                    //NetworkGame.OperationsHelper.TestUseSkill("SA010001");
                    {
                        if (command.Length > 0)
                        {
                            string idOrAlis = command[1];
                            var skill = DataResources.Instance.SkillData(s => s.id == idOrAlis || s.alias == idOrAlis);
                            if (skill != null)
                            {
                                Debug.LogError("skill {0} requested".f(skill.id));
                                NetworkGame.OperationsHelper.TestUseSkill(skill.id);
                            }
                            else
                            {
                                Debug.LogError("skill data not found");
                            }
                        }
                        else
                        {
                            Debug.LogError("need skill id or alias");
                        }
                    }
                    break;
                case "enterw":
                    {
                        MmoEngine.Get.Game.TryEnterWorkshop( WorkshopStrategyType.Angar );
                    }
                    break;
                case "exitw":
                    Debug.Log("try extw".Color("orange"));
                    MmoEngine.Get.Game.TryExitWorkshop();
                    break;
                case "add_scheme":
                    {
                        NetworkGame.OperationsHelper.EA_AddScheme();
                    }
                    break;
                case "show_asteroid":
                    {
                        var asteroids = GameObject.FindObjectsOfType<Asteroid>();
                        foreach (var asteroid in asteroids)
                        {
                            GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Detonator"), asteroid.transform.position, Quaternion.identity);
                        }
                    }
                    break;
                case "add_ore":
                    {
                        MmoEngine.Get.Game.CmdAddOres();
                    }
                    break;
                case "rebuild":
                    {
                        NetworkGame.OperationsHelper.TestRebuild();
                    }
                    break;
                case "go":
                    {
                        //changing world, specified by second argument
                        if (command.Length > 1)
                        {
                            G.Game.ChangeWorld(command[1]);
                        }
                        else
                        {
                            //if no arguments try go to world "1" if not this world
                            if (G.Game.ClientWorld.Id != G.Game.Settings.DefaultZones[Race.Humans])
                                G.Game.ChangeWorld(G.Game.Settings.DefaultZones[Race.Humans]);
                        }
                    }
                    break;
                case "tc":
                    {
                        NetworkGame.OperationsHelper.ToggleCameraMode();
                    }
                    break;
                case "craft":
                    {
                        G.UI.ModuleCraftingView.Show(!G.UI.ModuleCraftingView.Visible);
                    }
                    break;
                case "tbuffs":
                    {
                        G.UI.BuffsView.SetVisibility(!G.UI.BuffsView.Visibility);
                    }
                    break;
                case "tbonuses":
                    {
                        G.UI.BonusesView.SetVisibility(!G.UI.BonusesView.Visible);
                    }
                    break;
                case "tcommands":
                    {
                        G.UI.CommandsView.SetVisible(!G.UI.CommandsView.Visible);
                    }
                    break;
                case "tservicemessages":
                    {
                        G.UI.ServiceMessageView.SetVisibility(!G.UI.ServiceMessageView.Visible);
                    }
                    break;
                case "detonator":
                    {
                        GameObject.Instantiate(PrefabCache.Get("Prefabs/detonator"), G.Game.Avatar.View.transform.position, Quaternion.identity);
                    }
                    break;
                case "levelname":
                    {
                        //G.UI.ActionResult.SetActionResult(new Hashtable { { "level", Application.loadedLevelName } });
                        G.Game.SendChatMessage(ChatGroup.me, "level: {0}".f(Application.loadedLevelName), G.Game.LoginInfo.loginName);
                    }
                    break;
                case "add_slots":
                    {
                        G.Game.AddInventorySlots();
                    }
                    break;
                case "clear_station":
                    {
                        G.Game.ClearStationHold();
                    }
                    break;
                case "clear_inventory":
                    {
                        G.Game.ClearInventory();
                    }
                    break;
                case "demo_rebuild":
                    {
                        G.Game.RebuildForDemo();
                    }
                    break;
                case "add_demo_schemes":
                    {
                        G.Game.AddDemoSchemes();
                    }
                    break;
                case "demo_prepare":
                    {
                        G.Game.DemoPrepare();
                    }
                    break;
                case "gen_weapon":
                    {
                        //generate and set new weapon command
                        G.Game.GenWeapon();
                    }
                    break;
                case "out":
                    {
                        if (command.Length > 1)
                        {
                            switch (command[1])
                            {
                                case "weapon_slots":
                                    Debug.Log("WEAPON SLOTS COUNT: {0}".f(G.Game.Ship.ShipModel.TotalWeaponSlots).Bold().Color(Color.green));
                                    break;
                                case "weapon":
                                    {
                                        //get weapon info as hashatble
                                        Hashtable weaponInfo = G.Game.Ship.Weapon.GetInfo();

                                        //construct string builder for weapon hashtable
                                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                        CommonUtils.ConstructHashString(weaponInfo, 1, ref stringBuilder);

                                        //print weapon string
                                        Debug.Log(stringBuilder.ToString().Color(Color.green).Bold());

                                    }
                                    break;
                                case "props":
                                    {
                                        //out all item properties
                                        foreach (var typedItems in G.Game.Items)
                                        {
                                            foreach (var itemPair in typedItems.Value)
                                            {
                                                var itemProperties = itemPair.Value.RawProperties;
                                                var stringBuilder = new StringBuilder();
                                                CommonUtils.ConstructHashString(itemProperties, 1, ref stringBuilder);
                                                Dbg.Print(stringBuilder.ToString(), "NPC");

                                            }
                                        }
                                    }
                                    break;
                                case "skills":
                                    {
                                        foreach(var s in G.Game.Skills.Skills)
                                        {
                                            Dbg.Print("at {0} skills {1}".f(s.Key, s.Value.Id), "PLAYER");
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "gett":
                    {
                        if (command.Length > 1)
                        {
                            var list = new List<string>();
                            for (int i = 1; i < command.Length; i++)
                            {
                                list.Add(command[i]);
                            }
                            NetworkGame.OperationsHelper.GetTargetCombatProperties(list.ToArray());
                        }
                    }
                    break;
                case "test":
                    {
                        //Debug.Log("event text view created");
                        //SpaceTest.Get.StartCoroutine(SpaceTest.Get.CorTestSkill());
                        //EventTextView eventTextView = new EventTextView(G.UI, "Министерство финансов РФ приступает к продаже своих валютных остатков на рынке. Покупку рублей на временно свободные бюджетные средства, размещенные в валюте, в ведомстве объяснили тем, что российская валюта сейчас «крайне недооценена». ");
                        //NetworkGame.OperationsHelper.PlaceStationAtMe();
                        G.Game.RPC("PlaceAsteroidAtMe", new object[] { });
                    }
                    break;
                case "add_health":
                    {
                        float num = 0;
                        if (float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out num))
                        {
                            NetworkGame.OperationsHelper.AddHealth(num);
                        }
                    }
                    break;
                case "respawn":
                    {
                        G.Game.Respawn();
                    }
                    break;
                case "set_skill":
                    {
                        string sID = DataResources.Instance.SkillData(s => s.alias == command[1]).id;
                        Operations.ExecAction(G.Game, G.Game.AvatarId, "SetSkill", new object[] { sID });
                    }
                    break;
                case "ctb":
                    {
                        try
                        {
                            int buff = int.Parse(command[1]);
                            NetworkGame.OperationsHelper.CheckTargetBonus((byte)buff);
                        }
                        catch(System.Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                    }
                    break;
                case "stats":
                    {
                        NetworkGame.OperationsHelper.CheckStats();
                        NetworkGame.OperationsHelper.ServerStats();
                    }
                    break;
                case "gc":
                    {
                        NetworkGame.OperationsHelper.CollectMemory();
                    }
                    break;
                case "mtt":
                    {
                        if (G.Game.Avatar == null)
                            return;
                        if (G.Game.Avatar.Target.HasTarget == false)
                            return;
                        if (G.Game.Avatar.Target.Item == null)
                            return;
                        if (!G.Game.Avatar.Target.Item.View)
                            return;
                        Vector3 position = G.Game.Avatar.Target.Item.View.transform.position;
                        G.Game.Avatar.View.transform.position = position;
                    }
                    break;
                case "add":
                    {
                        if (command.Length != 3)
                            Debug.LogError("invalid number of arguments for command add");
                        try
                        {
                            switch(command[1].ToLower())
                            {
                                case "exp":
                                    {
                                        int count = int.Parse(command[2]);
                                        G.Game.DebugRPC.AddExp(count);case
                                    }
                                    break;
                                default:
                                    Debug.LogError("unknown parameter {0} for command add".f(command[1]));
                                    break;
                            }

                        }
                        catch(System.Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                    break;
                case "tgm":
                    {
                        G.Game.RPC("ToggleGodMode", new object[] { });
                    }
                    break;
                case "cd":
                    {
                        G.Game.RPC("SetCooldownMult", new object[] { float.Parse(command[1], System.Globalization.CultureInfo.InvariantCulture) });
                    }
                    break;
                case "camera":
                    {
                        switch(command[1])
                        {
                            case "normal":
                                {
                                    MouseOrbitRotateZoom.Get.SetCameraStrategy(CameraStrategyType.Normal);
                                }
                                break;
                            case "ellipse":
                                {
                                    MouseOrbitRotateZoom.Get.SetCameraStrategy(CameraStrategyType.Ellipse);
                                }
                                break;
                            case "free":
                                {
                                    MouseOrbitRotateZoom.Get.SetCameraStrategy(CameraStrategyType.Free);
                                }
                                break;
                        }
                    }
                    break;
                case "mail":
                    {
                        string title = "Test title";
                        if (command.Length > 1)
                            title = command[1];
                        string body = "Some start test body";
                        if (command.Length > 2)
                            body = command[2];
                        G.Game.DebugRPC.SendTestMailFromServerToPlayer(title, body);
                    }
                    break;
                case "maila": {
                    G.Game.DebugRPC.SendTestMailFromServerToPlayerWithAttachments();
                    break;
                    }
            }
        }

        public void SetNeedScrollToNewMessages(bool value)
        {
            this.needScrollToNewMessage = value;
        }

        public void SetNeedScrollLogToLastMessage(bool need)
        {
            this.needScrollLogToLastMessage = need;
        }

        public bool Visible
        {
            get
            {
                return this.chatGroup.Visible;
            }
        }

        public void SetVisible(bool val)
        {
            this.chatGroup.SetVisibility(val);
        }
    }


}*/