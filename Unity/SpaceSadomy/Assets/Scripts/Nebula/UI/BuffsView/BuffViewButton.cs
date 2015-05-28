namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Nebula.Client.Res;
    using Common;

    public class BuffViewButton : MonoBehaviour {

        public UnityEngine.UI.Button button;
        public Text text;


        private ResBuffData buff;
        private float buffValue;

        public void SetObject(BonusType bonusType, float value, ResBuffData buffData) {

            this.buff = buffData;
            this.buffValue = value;

            if(this.buff == null ) {
                Debug.LogErrorFormat("Not found buff: {0}", bonusType);
                return;
            }

            this.button.image.overrideSprite = SpriteCache.SpriteBuff(bonusType, this.buff.icon);
            this.text.text = string.Format("{0:F2}", value);
        }

        public BonusType BuffType() {
            if(buff == null ) {
                Debug.LogError("Buff is null");
                return BonusType.additionalDamage2EnemiesOnAreaPerFire;
            }
            return buff.bonusType;
        }

        public void OnBuffButtonClick() {
            if (this.buff == null) {
                Debug.Log("buff null");
                return;
            }

            TooltipView.TooltipData tooltipData = new TooltipView.TooltipData {
                TitleId = string.Empty,
                ContentId = this.buff.description.Trim()
            };
            MainCanvas.Get.Show(CanvasPanelType.TooltipView, tooltipData);
        }
    }
}
