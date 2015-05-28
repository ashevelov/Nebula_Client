namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;

    public abstract class BaseView : MonoBehaviour {

        protected System.Action willDestroyHandler;

        public virtual void Setup(object objData) { }

        public virtual void OnWillDestroy() {
            if (this.willDestroyHandler != null) {
                this.willDestroyHandler();
            }
        }

        public void SetWillDestroyHandler(System.Action willDestroy) {
            this.willDestroyHandler = willDestroy;
        }
    }
}
