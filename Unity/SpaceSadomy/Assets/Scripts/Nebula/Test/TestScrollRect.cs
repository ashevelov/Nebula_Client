
namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class TestScrollRect : MonoBehaviour {
        private UnityEngine.UI.ScrollRect scrollRect;

        void Start() {
            scrollRect = GetComponentInChildren<UnityEngine.UI.ScrollRect>();
        }

        void Update() {
            //print("scroll rect normalized position: {0}".f(scrollRect.normalizedPosition));

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                print("set scroll to end");
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}
