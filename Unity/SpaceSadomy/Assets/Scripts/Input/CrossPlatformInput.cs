using UnityEngine;
using System.Collections;

namespace Game.Space {
    public static class CrossPlatformInput 
    {
        public static float HorizontalAxis {
            get {
                return Input.GetAxis("Mouse X");
            }
        }

        public static float VerticalAxis {
            get {
                return Input.GetAxis("Mouse Y");
            }
        }

        public static float Scroll {
            get {
                return Input.GetAxis("Mouse ScrollWheel");
            }
        }

        public static bool IsButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public static bool IsButtonDown(int button) {
            return Input.GetMouseButtonDown(button);
        }
        public static bool IsButtonUp(int button) {
            return Input.GetMouseButtonUp(button);
        }
    }
}

