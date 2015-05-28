using UnityEngine;
using System.Collections;
namespace Nebula.UI {
    using USlider = UnityEngine.UI.Slider;
    using UText = UnityEngine.UI.Text;
    using UImage = UnityEngine.UI.Image;

    

    public class NpcTargetView : BaseTargetView {

        public USlider HealthProgress;
        public UText HealthText;
        public UImage HitChanceImage;
        public UText HitChanceText;

        private float timer = UPDATE_INTERVAL;

        public override void SetObject(IObjectInfo obj) {
            Debug.Log("Set NpcTargetView");
            base.SetObject(obj);

            if (!CheckObjectInfo()) {
                Debug.Log("Checl not succeed");
                return;
            }
            Debug.Log("Check Succeded");

            var combatObjectInfo = this.objectInfo as ICombatObjectInfo;
            this.UpdateHealth(combatObjectInfo);
            this.UpdateHitChance(combatObjectInfo);
        }

        public override bool CheckObjectInfo() {
            if (this.objectInfo == null) {
                return false;
            }

            if (!(this.objectInfo is ICombatObjectInfo)) {
                return false;
            }
            return true;
        }

        private void UpdateHealth(ICombatObjectInfo o) {
            int ch = Mathf.RoundToInt(o.CurrentHealth);
            int mh = Mathf.RoundToInt(o.MaxHealth);
            if (mh == 0) {
                Debug.LogError("Max Health for ICombatObject null");
                return;
            }
            this.HealthProgress.value  = Mathf.Clamp01(o.CurrentHealth / o.MaxHealth);
            this.HealthText.text = string.Format("{0}/{1}", ch, mh);
        }

        private void UpdateHitChance(ICombatObjectInfo o) {
            this.HitChanceImage.fillAmount = o.HitProb;
            this.HitChanceText.text = string.Format("{0}%", Mathf.RoundToInt(o.HitProb * 100f).ToString());
        }

        public override void Update() {
            base.Update();

            timer -= Time.deltaTime;
            if (timer <= 0f) {
                timer += UPDATE_INTERVAL;
                if (CheckObjectInfo()) {
                    var co = this.objectInfo as ICombatObjectInfo;
                    this.UpdateHealth(co);
                    this.UpdateHitChance(co);
                }
            }
        }
    }
}