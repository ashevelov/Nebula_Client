namespace Nebula.Test {
    using UnityEngine;

    public class TestEvent : MonoBehaviour {

        void OnGUI() {
            /*
            Event e = Event.current;
            if (e.alt)
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    print("option key");
                }
                else
                {
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        print("alt key");
                    }
                }
            }
            if (e.command)
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                    print("command key");
                else if (Application.platform == RuntimePlatform.WindowsEditor)
                    print("windows key");
            }*/


            /*
            Event e = Event.current;
            if (e.button == 0 && e.isMouse)
            {
                Debug.Log("Left Click");
            }
            else
            {
                if (e.button == 1)
                {
                    Debug.Log("Right Click");
                }
                else
                {
                    if (e.button == 2)
                    {
                        Debug.Log("Middle Click");
                    }
                    else
                    {
                        if (e.button > 2)
                        {
                            Debug.Log("Another button in the mouse clicked");
                        }
                    }
                }
            }*/

            /*
            Event e = Event.current;
            if (e.capsLock)
            {
                GUI.Label(new Rect(10, 10, 100, 20), "CapsLock on.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, 100, 20), "CapsLock Off.");
            }*/

            /*
            Event e = Event.current;
            if (e.isKey)
            {
                print("Detected character: " + e.character);
            }
             */

            //Event e = Event.current;
            //if (e.isMouse)
            //{
            //    print("Mouse clicks: " + e.clickCount);
            //}

            //Event e = Event.current;
            //if (e.commandName != "")
            //    print("command recognized: " + e.commandName);

            //Event e = Event.current;
            //if (e.control)
            //    print("Control was pressed");

            //Event e = Event.current;
            //if (e.isMouse)
            //    Debug.Log(e.delta);

            //Event e = Event.current;
            //if (e.functionKey)
            //    print("Pressed: " + e.keyCode);

            //Event e = Event.current;
            //if (e.isKey)
            //    print("detected a keyboard event!");

            //Event e = Event.current;
            //if (e.isMouse)
            //    print("Detected a mouse event!");

            Event e = Event.current;
            //if (e.isKey)
            //    print("Detected key code: " + e.keyCode); 

            //Debug.Log(e.mousePosition);

            //if (e.numeric)
            //    GUI.Label(new Rect(10, 10, 150, 20), "Numeric Key pas is on");
            //else
            //    GUI.Label(new Rect(10, 10, 150, 20), "Numeric Key pas is off");

            //if (e.shift)
            //    print("shift was pressed :O");

            //print("Current event detected: " + e.type);
            if (GUI.Button(new Rect(0, 0, 100, 100), "test")) {

            }
            if (GUI.Button(new Rect(200, 200, 100, 100), "test")) {

            }

            Debug.Log("Available id: " + GUIUtility.GetControlID(FocusType.Passive));

            print("Type for control id(focus passive): " + Event.current.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)));
            print("Type for control id(focus native): " + Event.current.GetTypeForControl(GUIUtility.GetControlID(FocusType.Native)));
            print("Type for control id(focus keyborard): " + Event.current.GetTypeForControl(GUIUtility.GetControlID(FocusType.Keyboard)));
        }
    }
}