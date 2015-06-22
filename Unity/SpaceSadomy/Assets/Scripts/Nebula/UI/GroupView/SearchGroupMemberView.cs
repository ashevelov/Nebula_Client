namespace Nebula.UI {
    using Nebula.Client;
    using Nebula.Resources;
    using UnityEngine;
    using UnityEngine.UI;

    public class SearchGroupMemberView : MonoBehaviour {
        public Image WorkshopImage;
        public Text NameLevelText;


        private ClientSearchGroupMember member;
        public void SetObject(ClientSearchGroupMember member) {
            this.member = member;

            if(member == null ) {
                gameObject.SetActive(false);
                return;
            } else {
                if(!gameObject.activeSelf) {
                    gameObject.SetActive(true);
                }
            }

            this.WorkshopImage.overrideSprite = SpriteCache.WorkshopSprite(member.Workshop());
            this.NameLevelText.text = string.Format("{0} ({1})", member.DisplayName(), member.Level());

        }
    }
}