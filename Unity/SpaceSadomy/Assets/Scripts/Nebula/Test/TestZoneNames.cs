using UnityEngine;
using System.Collections;

public class TestZoneNames : MonoBehaviour 
{
    private StringSubCache<string> stringsCache = new StringSubCache<string>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(DataResources.Instance.ZoneForId("H1").DisplayName());
            Debug.Log(stringsCache.String(DataResources.Instance.ZoneForId("H1").DisplayName(), DataResources.Instance.ZoneForId("H1").DisplayName()));
            Debug.Log(StringCache.Get("H1_NAME"));
        }
    }
}
