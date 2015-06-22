namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using UnityEngine.UI;
    using Nebula.Resources;

    public class GroupMemberBuffView : MonoBehaviour {

        public Image BonusImage;
        public Text BonusValueText;

        private BonusType bonusType;
        private float bonusValue;

        public void SetObject(BonusType bonusType, float bonusValue) {
            this.bonusType = bonusType;
            this.bonusValue = bonusValue;

            var buffData = DataResources.Instance.Buff(bonusType);
            this.BonusImage.overrideSprite = SpriteCache.SpriteBuff(bonusType, buffData.icon);
            this.BonusValueText.text = bonusValue.ToString();
        }

        public BonusType BonusType() {
            return this.bonusType;
        }

        public float BonusValue() {
            return this.bonusValue;
        }
    }

}