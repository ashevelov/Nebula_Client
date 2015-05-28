using UnityEngine;
using System.Collections;

namespace Nebula.UI {
    public class AsteroidTargetView : BaseTargetView {

        public UnityEngine.UI.Image[] ContentImages;
        public UnityEngine.UI.Text DescriptionText;

        private float timer = UPDATE_INTERVAL;

        public override void SetObject(IObjectInfo objectInfo) {
            base.SetObject(objectInfo);

            if (!this.CheckObjectInfo()) {
                return;
            }

            UpdateContent();
            UpdateDescription();
        }

        private void UpdateContent() {
            var asteroidObjectInfo = this.objectInfo as IAsteroidObjectInfo;
            var contentSprites = asteroidObjectInfo.ContentSprites;

            for (int i = 0; i < this.ContentImages.Length; i++) {
                if (i < contentSprites.Count) {
                    this.ContentImages[i].gameObject.SetActive(true);
                    this.ContentImages[i].overrideSprite = contentSprites[i];
                } else {
                    this.ContentImages[i].gameObject.SetActive(false);
                }
            }
        }

        private void UpdateDescription() {
            var asteroidObjectInfo = this.objectInfo as IAsteroidObjectInfo;
            this.DescriptionText.text = asteroidObjectInfo.Description;
        }

        public override bool CheckObjectInfo() {
            if (this.objectInfo == null) {
                return false;
            }
            if (!(this.objectInfo is IAsteroidObjectInfo)) {
                return false;
            }
            return true;
        }

        public override void Update() {
            base.Update();

            timer -= Time.deltaTime;
            if (timer <= 0f) {
                timer += UPDATE_INTERVAL;
                if (CheckObjectInfo()) {
                    UpdateContent();
                    UpdateDescription();
                }
            }
        }
    }
}
