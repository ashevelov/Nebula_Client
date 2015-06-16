namespace Nebula.Test {
    using Nebula;
    using UnityEngine;

    public class TestClockProgress : MonoBehaviour {

        public Material textureMaterial;
        public Texture2D tex;

        private float cutOff = 0f;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (textureMaterial)
                textureMaterial.SetFloat("_CutOff", cutOff);
        }

        void OnGUI() {
            Utils.SaveMatrix();

            if (Event.current.type == EventType.Repaint) {
                Graphics.DrawTexture(new Rect(0, 0, 200, 200), tex, textureMaterial);
            }

            cutOff = GUI.HorizontalSlider(new Rect(300, 20, 200, 20), cutOff, 0f, 1f);

            Utils.RestoreMatrix();
        }


    }
}