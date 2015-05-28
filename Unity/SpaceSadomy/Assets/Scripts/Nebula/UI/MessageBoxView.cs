namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class MessageBoxView : BaseView {

        public Button[] C3_Buttons;
        public Button[] C2_Buttons;
        public Button[] C1_Buttons;
        public Transform NoImageConfig;
        public Transform ImageConfig;
        public Text Title;


        public override void Setup(object objData) {
            base.Setup(objData);

            MessageBoxConfig config = objData as MessageBoxConfig;
            Button[] targetButtons = GetButtons(config.CountOfButtons);

            for(int i = 0; i < Mathf.Min(targetButtons.Length, config.ButtonNames.Length); i++) {
                targetButtons[i].onClick.RemoveAllListeners();
                var action = config.ButtonActions[i];
                targetButtons[i].onClick.AddListener(() => {
                    if (action != null) {
                        action();
                    }
                    MainCanvas.Get.Destroy(CanvasPanelType.MessageBox);
                });
                targetButtons[i].GetComponentInChildren<Text>().text = config.ButtonNames[i];
            }

            if(config.HasImage) {
                NoImageConfig.gameObject.SetActive(false);
                ImageConfig.gameObject.SetActive(true);
                ImageConfig.GetComponentInChildren<Image>().overrideSprite = config.Icon;
                ImageConfig.GetComponentInChildren<Text>().text = config.Text;
            } else {
                NoImageConfig.gameObject.SetActive(true);
                ImageConfig.gameObject.SetActive(false);
                NoImageConfig.GetComponentInChildren<Text>().text = config.Text;
            }

            if(string.IsNullOrEmpty(config.Title)) {
                this.Title.text = string.Empty;
            } else {
                this.Title.text = config.Title;
            }
        }

        private Button[] GetButtons(int countOfButtons) {
            switch (countOfButtons) {
                case 1:
                    {
                        C1_Buttons[0].transform.parent.gameObject.SetActive(true);
                        C2_Buttons[0].transform.parent.gameObject.SetActive(false);
                        C3_Buttons[0].transform.parent.gameObject.SetActive(false);
                        return C1_Buttons;
                    }
                case 2:
                    {
                        C1_Buttons[0].transform.parent.gameObject.SetActive(false);
                        C2_Buttons[0].transform.parent.gameObject.SetActive(true);
                        C3_Buttons[0].transform.parent.gameObject.SetActive(false);
                        return C2_Buttons;
                    }
                case 3:
                    {
                        C1_Buttons[0].transform.parent.gameObject.SetActive(false);
                        C2_Buttons[0].transform.parent.gameObject.SetActive(false);
                        C3_Buttons[0].transform.parent.gameObject.SetActive(true);
                        return C3_Buttons;
                    }
                default:
                    goto case 1;
            }
        }

        public class MessageBoxConfig {
            public int CountOfButtons;
            public bool HasImage;
            public string[] ButtonNames;
            public System.Action[] ButtonActions;
            public string Title;
            public string Text;
            public Sprite Icon;
            
        }
    }
}
