namespace Nebula.Test {
    using Nebula.Client.Guilds;
    using Nebula.Mmo.Games;
    using ServerClientCommon;
    using System.Collections;
    using UnityEngine;

    public class TestGuilds : MonoBehaviour {

        public GUISkin skin;
        private GUIStyle mLabelStyle;
        private bool mVisible = false;
        private string mDescription = string.Empty;


        void Start() {
            mLabelStyle = skin.GetStyle("font_upper_left");
            StartCoroutine(RequestGuild());
        }

        private IEnumerator RequestGuild() {
            while(MmoEngine.Get.ActiveGame != GameType.Game) {
                yield return null;
            }
            while(MmoEngine.Get.NebulaGame.CurrentStrategy != GameState.NebulaGameWorldEntered) {
                yield return null;
            }

            SelectCharacterGame.Instance().GetGuild();
        }

        void Update() {
            if(Input.GetKeyUp(KeyCode.Alpha3)) {
                mVisible = !mVisible;
                if(mVisible) {
                    SelectCharacterGame.Instance().GetGuild();
                }
            }
        }

        void OnGUI () {
            if(!mVisible) {
                return;
            }

            if(GameData.instance.guild == null ) { return; }

            if(GameData.instance.guild.has) {
                DrawGuild(30, 20);
            } else {
                DrawCreateGuild(30, 20);
            }
        }

        private void DrawGuild(float x, float y) {
            GUI.Label(pcrect(x, y, 0, 0), GameData.instance.guild.name, mLabelStyle);
            GUI.Label(pcrect(x, y + 4, 20, 0), GameData.instance.guild.description, mLabelStyle);

            mDescription = GUI.TextField(pcrect(x, y + 8, 20, 4), mDescription);
            if(GUI.Button(pcrect(x + 22, y + 8, 10, 4), "Set Description") ) {
                SelectCharacterGame.Instance().SetGuildDescription(mDescription);
            }

            float my = y + 20;
            foreach(var member in GameData.instance.guild.members) {
                DrawMember(x, my, member.Value);
                my += 7;
            }
        }

        private void DrawMember(float x, float y, GuildMember member) {
            GUI.Label(pcrect(x, y, 0, 0), 
                string.Format("login = {0}, exp = {1}, status = {2}", member.login, member.exp, (GuildMemberStatus)member.guildStatus), 
                mLabelStyle);
            if(member.characterID == SelectCharacterGame.Instance().PlayerCharacters.SelectedCharacterId) {
                if(GUI.Button(pcrect(x, y + 3, 10, 4), "Exit")) {
                    SelectCharacterGame.Instance().ExitGuild();
                }

                
                if(GameData.instance.guild.IsOwner(member.characterID)) {
                    if(GUI.Button(pcrect(x + 11, y + 3, 10, 4), "Delete Guild")) {
                        SelectCharacterGame.Instance().DeleteGuild();
                    }
                }
            }


        }

        private void DrawCreateGuild(float x, float y) {

            GUI.Label(pcrect(x, y, 0, 0), "You don't have guild. Create new.", mLabelStyle);
            if(GUI.Button(pcrect(x, y + 2, 10, 4), "Create new guild")) {
                //here create guild
                SelectCharacterGame.Instance().CreateGuild("My guild");
            }
        }


        private float px(float p) {
            return Screen.width * (p / 100.0f);
        }

        private float py(float p) {
            return Screen.height * (p / 100.0f);
        }

        private Rect pcrect(float x, float y, float w, float h) {
            return new Rect(px(x), py(y), px(w), py(h));
        }

    }
}
