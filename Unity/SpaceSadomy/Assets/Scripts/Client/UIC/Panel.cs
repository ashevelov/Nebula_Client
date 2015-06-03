using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {

    public static string PrefabPath
    {
        get
        {
            return "Prefabs/UIC/PlayerInfo";
        }
    }

    public static Panel Creat()
    {
        return Instantiate(Resources.Load(PrefabPath) as Panel) as Panel;
    }
}
