namespace Nebula.Test {
    using UnityEngine;

    public class UpdateShipModel : MonoBehaviour {

        [System.Serializable]
        public class MyUI {
            public GUIStyle style;
            public Rect group;
            public Rect DT;
            public Rect EQ;
            public Rect RE;
        }
        public MyUI ui;
        void Start() {
            ui.style = (Resources.Load("UI/Skins/game") as GUISkin).button;
        }

        void OnGUI() {
            /*
            Utils.SaveMatrix();
            ui.group.x = Screen.width / GUI.matrix.m00 - ui.group.width;
            GUI.BeginGroup(ui.group);
            if (GUI.Button(ui.DT, "Darth Tribe", ui.style))
            {
                MyPlayer mp = FindObjectOfType<MyPlayer>();
                if (mp != null)
                {
                    mp.GetComponent<ShipModel>().SetWorkShop("DT");
                }
            }
            if (GUI.Button(ui.EQ, "Equilibrium", ui.style))
            {
                MyPlayer mp = FindObjectOfType<MyPlayer>();
                if (mp != null)
                {
                    mp.GetComponent<ShipModel>().SetWorkShop("EQ");
                }
            }
            if (GUI.Button(ui.RE, "Red Eye", ui.style))
            {
                MyPlayer mp = FindObjectOfType<MyPlayer>();
                if (mp != null)
                {
                    mp.GetComponent<ShipModel>().SetWorkShop("RE");
                }
            }
            GUI.EndGroup();

            Utils.RestoreMatrix();
             */
        }
    }


    //darth tribe
    //red eye
    //equilibrium
}