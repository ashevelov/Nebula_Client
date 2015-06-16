namespace Nebula.Test {
    using Nebula;
    using UnityEngine;

    public class TestGUITextures : MonoBehaviour {

        public GUISkin skin;
        private GUIStyle boxStyle;
        public float x;
        public float y;
        public float width;
        public float height;

        // Use this for initialization
        void Start() {
            this.boxStyle = skin.GetStyle("box_1");
        }


        void OnGUI() {
            Utils.SaveMatrix();
            GUI.Box(new Rect(x, y, width, height), string.Empty, this.boxStyle);
            Utils.RestoreMatrix();
        }
    }
}
