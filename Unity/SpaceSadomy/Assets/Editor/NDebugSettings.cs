using UnityEngine;
using System.Collections;
using UnityEditor;

public class NDebugSettings : EditorWindow {

}

public class NDebugFilter {
    public string filterName { get; set; }
    public string color { get; set; }
    public bool isDefault { get; set; }

    public override string ToString() {
        return string.Format("{0}-{1}-{2}", filterName, color, isDefault);
    }

    //public static NDebugFilter FromString(string source) {
    //    string[] arr = source.Split(new char[] { '-' }, System.StringSplitOptions.RemoveEmptyEntries);
    //    if (arr.Length != ) {
    //        return null;
    //    }


    //}
}
