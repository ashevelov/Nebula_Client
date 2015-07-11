namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using System.Collections.Generic;


    public class LoadScreenImage : MonoBehaviour {

        public Sprite[] spriteImages;

        private Dictionary<string, int> mBannerWeights = new Dictionary<string, int>();


        public void SetRandom() {
            CheckWeights();

            //int index = Random.Range(0, spriteImages.Length - 1);
            int index = GetMinWeightIndex();
            GetComponent<Image>().overrideSprite = spriteImages[index];
            mBannerWeights[spriteImages[index].name]++;
        }

        private void CheckWeights() {
            foreach(var sImage in spriteImages) {
                if(!mBannerWeights.ContainsKey(sImage.name)) {
                    mBannerWeights.Add(sImage.name, 0);
                }
            }
        }

        private string GetMinWeightImageName() {
            int min = int.MaxValue;
            string imgName = string.Empty;
            foreach(var pair in mBannerWeights) {
                if(pair.Value < min ) {
                    min = pair.Value;
                    imgName = pair.Key;
                }
            }
            return imgName;
        }

        private int GetMinWeightIndex() {
            string imgName = GetMinWeightImageName();

            for(int i = 0; i < spriteImages.Length; i++) {
                if(spriteImages[i].name == imgName ) {
                    return i;
                }
            }
            return 0;
        }
    }
}
