using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityUIUtils
{
    [ExecuteInEditMode]
    public class UUIRotation : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public Vector3 speed = Vector3.zero;


        void FixedUpdate()
        {
            if (_rectTransform == null)
                _rectTransform = transform as RectTransform;

            _rectTransform.localEulerAngles = _rectTransform.localEulerAngles + (speed * Time.fixedDeltaTime);
        }
    }
}
