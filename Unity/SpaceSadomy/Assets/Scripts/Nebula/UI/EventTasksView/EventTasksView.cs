using Game.Network;
namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Nebula.Client;
    using Nebula.Mmo.Games;

    public class EventTasksView : BaseView {

        public GameObject EventViewPrefab;
        public Transform Content;

        private List<ClientWorldEventInfo> currentEvents = new List<ClientWorldEventInfo>();
        private List<ObjectChange<ClientWorldEventInfo>> changes = new List<ObjectChange<ClientWorldEventInfo>>();

        void Start() {
            UpdateEventView();
            this.StartCoroutine(C_UpdateEventViewLoop());
        }

        private IEnumerator C_UpdateEventViewLoop() {
            while(true ) {
                yield return new WaitForSeconds(Settings.EVENTS_UPDATE_INTERVAL);
                this.UpdateEventView();
            }
        }
        public void UpdateEventView() {

            var events = G.ActiveWorldEvents();

            if(events == null ) {
                return;
            }

            foreach(var newEvent in events ) {
                int index = this.currentEvents.FindIndex(e => e.Id == newEvent.Id);
                if (index >= 0) {
                    if (EventStageValid(newEvent)) {
                        changes.Add(new ObjectChange<ClientWorldEventInfo> { TargetObject = newEvent, ChangeType = ChangeType.UPDATE });
                    } else {
                        changes.Add(new ObjectChange<ClientWorldEventInfo> { TargetObject = newEvent, ChangeType = ChangeType.REMOVE });
                    }
                } else {
                    if (EventStageValid(newEvent)) {
                        changes.Add(new ObjectChange<ClientWorldEventInfo> { TargetObject = newEvent, ChangeType = ChangeType.ADD });
                    }
                }
            }

            EventStageView[] stageViews = this.GetComponentsInChildren<EventStageView>();

            foreach(var change in this.changes ) {
                switch(change.ChangeType) {
                    case ChangeType.ADD:
                        {
                            GameObject eventViewInstance = Instantiate(this.EventViewPrefab) as GameObject;
                            //here set event data from change
                            eventViewInstance.GetComponent<EventStageView>().SetObject(change.TargetObject);
                            this.currentEvents.Add(change.TargetObject);
                            eventViewInstance.transform.SetParent(this.Content, false);
                            break;
                        }
                    case ChangeType.REMOVE:
                        {
                            this.RemoveEventStageView(stageViews, change.TargetObject);
                            int index = this.currentEvents.FindIndex(e => e.Id == change.TargetObject.Id);
                            if(index >= 0 ) {
                                this.currentEvents.RemoveAt(index);
                            }
                            break;
                        }
                    case ChangeType.UPDATE:
                        { 
                            this.UpdateEventStageView(stageViews, change.TargetObject);
                            int index = this.currentEvents.FindIndex(e => e.Id == change.TargetObject.Id);
                            if(index >= 0 ) {
                                this.currentEvents[index] = change.TargetObject;
                            }
                        break;
                        }
                }
            }

            this.changes.Clear();
        }


        private void RemoveEventStageView(EventStageView[] source, ClientWorldEventInfo evt) {
            for(int i = 0; i < source.Length; i++) {
                if(source[i] != null ) {
                    if(source[i].CurrentEventId() == evt.Id ) {
                        Destroy(source[i].gameObject);
                        source[i] = null;
                        return;
                    }
                }
            }
        }

        private void UpdateEventStageView(EventStageView[] source, ClientWorldEventInfo evt ) {
            for( int i = 0; i < source.Length; i++ ) {
                if(source[i] != null ) {
                    if(source[i].CurrentEventId() == evt.Id ) {
                        source[i].SetObject(evt);
                        return;
                    }
                }
            }
        }

        private bool EventStageValid(ClientWorldEventInfo evt) {
            if(evt.Stage == null ) {
                return false;
            }
            if(evt.IsFinalStage()) {
                return false;
            }
            return true;
        }

        void OnDestroy() {
            this.StopAllCoroutines();
        }

    }

    public enum ChangeType : byte { ADD, UPDATE, REMOVE }

    public class ObjectChange<T>  {
        public T TargetObject;
        public ChangeType ChangeType;
    }
}
