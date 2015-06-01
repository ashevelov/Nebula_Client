namespace Nebula.UI {
	using UnityEngine;
	using System.Collections;
    using UnityEngine.UI;

	public class LoginView : BaseView {

        public InputField LoginField;
        public InputField DisplayNameField;
        public InputField PasswordField;
        public Button LoginButton;

        void Start() {
            //init to default values from preferences
            this.SetStartValuesFromPreferences();
        }

        private void SetStartValuesFromPreferences() {
            LoginField.text = PlayerPrefs.GetString(PrefKeys.LOGIN, string.Empty);
            DisplayNameField.text = PlayerPrefs.GetString(PrefKeys.DISPLAY_NAME, string.Empty);
            PasswordField.text = string.Empty;
        }

        //here need test valid or not strings in fields
        private bool CheckInputFields() {
            Debug.LogError("CheckInputFields must be realized later");
            return true;
        }

        //Handler for login button
        public void OnLoginButtonClicked() {
            if (this.CheckInputFields()) {
                //G.Game.Login(LoginField.text.Trim(), PasswordField.text.Trim(), DisplayNameField.text.Trim());
                NRPC.Login(MmoEngine.Get.LoginGame, LoginField.text.Trim(), "qwerty", "");
            }
        }
	}
}
