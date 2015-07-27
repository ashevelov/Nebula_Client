namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Nebula;
    using Common;
    using Nebula.Resources;
    using Nebula.Client.Res;
    using Game.Space;
    using ServerClientCommon;
    using Nebula.Mmo.Games;
    using Nebula.UI;
    using Nebula.Mmo.Items;

    public class DebugView : Singleton<DebugView> {

        private int mIndex = -1;
        private int mViewCount = 5;
        public GUISkin skin;
        private GUIStyle mStyle;
        private Rect mRect;
        public Hashtable lastShot { get; private set; }
        public Hashtable lastSkill { get; private set; }
        private float mGetWorldsTimer = 0;

        void Start() {
            mStyle = skin.GetStyle("font_upper_left");
        }

        void Update() {
            if (Input.GetKeyUp(KeyCode.RightArrow)) {
                mIndex++;
                if (mViewCount == mIndex) {
                    mIndex = -1;
                }
            }

            if(MmoEngine.Get.ActiveGame == GameType.Game &&
                MmoEngine.Get.NebulaGame.CurrentStrategy == GameState.NebulaGameWorldEntered) {
                mGetWorldsTimer -= Time.deltaTime;
                if(mGetWorldsTimer <= 0f ) {
                    mGetWorldsTimer = 5;
                    Operations.GetWorlds(MmoEngine.Get.NebulaGame);
                }
            } 
        }

        void OnGUI() {
            //check the index valid
            if (mIndex < 0) {
                return;
            }

            switch (mIndex) {
                case 0:
                    DrawWeapon();
                    break;
                case 1:
                    DrawLastShot();
                    break;
                case 2:
                    DrawLastSkill();
                    break;
                case 3:
                    DrawTargetView();
                    break;
                case 4:
                    DrawWorlds();
                    break;
            }
        }



        public void SetShot(Hashtable shot) {
            lastShot = shot;


        }



        public void SetSkill(Hashtable skill) {
            lastSkill = skill;
        }

        private void DrawWorlds() {
            ToStartLine();
            PrintLine("WORLDS:");
            ToNextLine();
            var world = GameData.instance.worlds.GetWorld(GameData.instance.World.Name);
            if(world != null ) {
                PrintLine(world.ToString().Color("orange"));
            }
            /*
            foreach (var p in GameData.instance.worlds.worlds) {
                PrintLine(p.Value.ToString().Color("orange"));
                ToNextLine();
            }*/
        }

        private void DrawLastSkill() {
            ToStartLine();
            PrintLine("MY LAST SKILL USE PROPERTIES:");
            ToNextLine();
            if (lastSkill == null) {
                return;
            }



            foreach (DictionaryEntry entry in lastSkill) {

                if ((int)entry.Key == (int)SPC.Info) {
                    PrintLine("Info:");
                    ToNextLine();
                    string s = "    ";
                    foreach (DictionaryEntry infoEntry in (entry.Value as Hashtable)) {
                        PrintLine(s + string.Format("{0}:    {1}", (SPC)(int)infoEntry.Key, infoEntry.Value.ToString()).Color("orange"));
                        ToNextLine();
                    }
                } else if ((int)entry.Key == (int)SPC.Data) {
                    PrintLine("Data:");
                    ToNextLine();
                    string s = "    ";
                    foreach (DictionaryEntry infoEntry in (entry.Value as Hashtable)) {

                        if ((int)infoEntry.Key == (int)SPC.Inputs) {
                            PrintLine(s + "Inputs:".Color("orange"));
                            ToNextLine();

                            string ss = s + s;
                            foreach (DictionaryEntry inp in (infoEntry.Value as Hashtable)) {
                                PrintLine(ss + string.Format("{0}:    {1}", inp.Key.ToString(), inp.Value.ToString()).Color("orange"));
                                ToNextLine();
                            }
                        } else {
                            PrintLine(s + string.Format("{0}:    {1}", (SPC)(int)infoEntry.Key, infoEntry.Value.ToString()).Color("orange"));
                            ToNextLine();
                        }
                    }
                } else {
                    PrintLine(string.Format("{0}:    {1}", (SPC)(int)entry.Key, entry.Value.ToString()).Color("orange"));
                    ToNextLine();
                }

            }

        }
        private void DrawLastShot() {
            ToStartLine();
            PrintLine("MY LAST SHOT PROPERTIES");
            ToNextLine();

            if (lastShot == null) {
                return;
            }




            foreach (DictionaryEntry entry in lastShot) {
                PrintLine(string.Format("{0}:    {1}", (SPC)(int)entry.Key, entry.Value.ToString()).Color("orange"));
                ToNextLine();
            }
        }

        private void DrawTargetView() {
            ToStartLine();
            PrintLine("TARGET VIEW");
            ToNextLine();
            if (NetworkGame.Instance().Avatar == null) {
                return;
            }
            if (NetworkGame.Instance().Avatar.Target.Item == null) {
                return;
            }

            foreach (var entry in NetworkGame.Instance().Avatar.Target.Item.props) {
                PrintLine(string.Format("{0}:  {1}", (PS)entry.Key, entry.Value.ToString().Color("orange")));
                ToNextLine();
            }
            PrintLine("Components:");
            ToNextLine();
            foreach (var c in NetworkGame.Instance().Avatar.Target.Item.componentIDS) {
                PrintLine(c.ToString().Color("orange"));
                ToNextLine();
            }

        }

        private void DrawWeapon() {
            var weapon = GameData.instance.ship.Weapon;

            if (weapon == null) {
                return;
            }

            ToStartLine();
            PrintLine(string.Format("Has = {0}", weapon.HasWeapon));
            ToNextLine();
            PrintLine(string.Format("Damage = {0}", weapon.damage));
            ToNextLine();
            PrintLine(string.Format("Critical Damage = {0}", weapon.critDamage));
            ToNextLine();
            PrintLine(string.Format("Optimal Distance = {0}", weapon.optimalDistance));
            ToNextLine();

            if (weapon.HasWeapon) {
                PrintLine("==========================Weapon object:");
                ToNextLine();
                PrintLine(string.Format("           ID = {0}", weapon.WeaponObject.Id));
                ToNextLine();
                PrintLine(string.Format("           TEMPLATE = {0}", weapon.WeaponObject.Template));
                ToNextLine();
                PrintLine(string.Format("           LEVEL = {0}", weapon.WeaponObject.Level));
                ToNextLine();
                PrintLine(string.Format("           COLOR = {0}", weapon.WeaponObject.Color));
                ToNextLine();
                PrintLine(string.Format("           DAMAGE TYPE = {0}", weapon.WeaponObject.DamagetType));
                ToNextLine();
                PrintLine(string.Format("           DAMAHE = {0}", weapon.WeaponObject.damage));
                ToNextLine();
                PrintLine(string.Format("           OPTIMAL DIST = {0}", weapon.WeaponObject.OptimalDistance));
                ToNextLine();
                PrintLine(string.Format("           INVENTORY OBJECT TYPE = {0}", weapon.WeaponObject.Type));
                ToNextLine();
                PrintLine(string.Format("           PLACING TYPE = {0}", (PlacingType)weapon.WeaponObject.placingType));
                ToNextLine();
                PrintLine(string.Format("           HAS COLOR = {0}", weapon.WeaponObject.HasColor()));
                ToNextLine();
                PrintLine("=============================Additional infor from resources:");
                ResWeaponTemplate weaponTemplate = DataResources.Instance.Weapon(weapon.WeaponObject.Template);
                ToNextLine();
                if (weaponTemplate != null) {
                    PrintLine(string.Format("TEMPLATE ID = {0}", weaponTemplate.Id));
                    ToNextLine();
                    PrintLine(string.Format("NAME = {0}", weaponTemplate.Name));
                    ToNextLine();
                    PrintLine(string.Format("WORKSHOP = {0}", weaponTemplate.Workshop));
                    ToNextLine();
                    PrintLine(string.Format("DESCRIPTION = {0}", weaponTemplate.Description));
                    ToNextLine();
                } else {
                    PrintLine("(not found = (null))");
                    ToNextLine();
                }
                PrintLine("WeaponInventoryObjectInfo <- IInventoryObjectInfo <- { IInventoryObjectBase, IColorInfo } <- { IInfo, IPlacingType }");

            } else {
                PrintLine(string.Format("Weapon Object = {0}", "(null)"));
            }
        }

        private void ToStartLine() {
            mRect = new Rect(150, 100, 0, 0);
        }

        private void ToNextLine() {
            mRect = mRect.moveVert(20);
        }
        private void PrintLine(string msg) {
            GUI.Label(mRect, msg, mStyle);
        }
    }
}
