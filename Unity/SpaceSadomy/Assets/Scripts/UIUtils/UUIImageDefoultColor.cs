using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityUIUtils
{
    [ExecuteInEditMode]
    public class UUIImageDefoultColor : MonoBehaviour
    {
        private Image _image;
        void Start()
        {
            UpdateImageColor();
        }

        public void UpdateImageColor()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            if (_image != null)
                _image.color = UUISettings.Get.uiColor;
        }
    }
}