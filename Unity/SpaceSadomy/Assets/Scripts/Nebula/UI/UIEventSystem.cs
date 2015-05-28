using UnityEngine;
using System.Collections;
using Game.Space;

public class UIEventSystem : Singleton<UIEventSystem> {

    private static bool alreadyCreated = false;
    void Awake(){
        if(!alreadyCreated) {
            alreadyCreated = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
}
