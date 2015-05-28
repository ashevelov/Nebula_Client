using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityUIUtils
{
    [ExecuteInEditMode]
    public class UUITextDefoultColor : MonoBehaviour
    {
        private Text _text;
        void Start()
        {
            UpdateImageColor();
        }

        public void UpdateImageColor()
        {
            if (_text == null)
                _text = GetComponent<Text>();

            if (_text != null)
                _text.color = UUISettings.Get.textColor;
        }
    }
}