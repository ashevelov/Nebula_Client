using UnityEngine;
using System.Collections;
using Common;

namespace Nebula.UI {
    public class TargetInfoView : BaseView {

        public BaseTargetView[] TargetViews;
        private IObjectInfo prevObjectInfo;



        void Start() {
            SetTargetsTo(null);
        }

        public void SetTarget(IObjectInfo objectInfo) {
            //protection from frequent calling
            if (objectInfo == prevObjectInfo) {
                return;
            }
            prevObjectInfo = objectInfo;
            /*
            if (objectInfo != null) {
                Debug.Log("<color=green>set target of type: {0} and type: {1}</color>".f(objectInfo.InfoType, objectInfo.GetType().ToString()));
            }*/
            this.SetTargetsTo(objectInfo);
        }

        private void SetTargetsTo(IObjectInfo objectInfo) {
            for (int i = 0; i < this.TargetViews.Length; i++) {
                this.TargetViews[i].SetObject(objectInfo);
            }
        }
    }
}
