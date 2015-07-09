namespace Nebula.UI {
	using UnityEngine;
	using System.Collections;
    using UnityEngine.UI;

	public class LoginView : BaseView {

        public InputField LoginField;
        public Button LoginButton;

        public Text loginText;
        public Text enterLoginText;

        void Start() {
            //init to default values from preferences
            this.SetStartValuesFromPreferences();

            this.loginText.text = Nebula.Resources.StringCache.Get("LOGIN");
            this.enterLoginText.text = Nebula.Resources.StringCache.Get("ENTER_LOGIN");
        }

        private void SetStartValuesFromPreferences() {
            LoginField.text = PlayerPrefs.GetString(PrefKeys.LOGIN, string.Empty);
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
                NRPC.Login(MmoEngine.Get.LoginGame, LoginField.text.Trim(), "qwerty", LoginField.text.Trim());
                PlayerPrefs.SetString(PrefKeys.LOGIN, LoginField.text.Trim());
                PlayerPrefs.Save();
            }
        }
	}
}
