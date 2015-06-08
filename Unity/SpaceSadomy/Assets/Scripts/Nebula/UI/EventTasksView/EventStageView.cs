//namespace Nebula.UI {
//    using Nebula.Client;
//    using UnityEngine;
//    using UnityEngine.UI;

//    public class EventStageView : MonoBehaviour {
//        public Text StageTitle;
//        public Text StageText;

//        private ClientWorldEventInfo currentEvent;

//        public void SetObject(ClientWorldEventInfo evt) {
//            this.currentEvent = evt;

//            this.StageTitle.text = StringCache.Get(evt.Name).Trim();

//            string currentTaskTextId = CurrentStageTextId(evt);
//            if (string.IsNullOrEmpty(currentTaskTextId)) {
//                Debug.LogErrorFormat("Task text empty {0}:{1}", evt.Id, evt.Stage.StageId);
//                this.StageText.text = string.Empty;
//                return;
//            }
//            string taskText = StringCache.Get(currentTaskTextId);
//            string replacedText = evt.ReplaceTextWithVAriables(taskText).Trim();

//            this.StageText.text = replacedText;
//        }

//        private string CurrentStageTextId(ClientWorldEventInfo evt) {
//            return evt.StageTaskTextId();
//        }

//        public string CurrentEventId() {
//            if(this.currentEvent == null ) {
//                return string.Empty;
//            }
//            return this.currentEvent.Id;
//        }
//    }
//}
