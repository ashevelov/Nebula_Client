using UnityEngine;
using System.Collections;
using System;
using Game.Network;

public class LoadScenes : MonoBehaviour
{

	//private AsyncOperation async;

	private GameObject gameObjCamera;

	public string loadSceneName;

	public static bool flag = true;

    public Camera _camera;

    private static LoadScenes _loadScanes = null;
    public static void Load(String Name)
    {
        if (_loadScanes == null)
        {
            _loadScanes = FindObjectOfType<LoadScenes>();
        }
        _loadScanes._camera.gameObject.SetActive(true);
        _loadScanes.loadSceneName = Name;
        Application.LoadLevelAsync("LoadScene");
    }


	void OnLevelWasLoaded( int level )
	{
		if( flag )
		{
			//if (level == 2) 
			if( Application.loadedLevelName=="LoadScene" )
			{
				flag = false;
				Debug.Log( "level = " + level );
				StartCoroutine( LoadScene() );
			}
		}
	}

	public IEnumerator LoadScene()
	{
		if( string.IsNullOrEmpty( loadSceneName ) )
			yield return 0;
		gameObjCamera = FindObjectOfType<Camera>().gameObject;
		Debug.Log( "ManagerGame: Level[" + loadSceneName + "] loading level" );

        //var go = GameObject.Find( "ContentRoot" );

        //if (go != null)
        //{
        //    DestroyImmediate(go);
        //    yield return null;
        //    GC.Collect();
        //    yield return null;
        AsyncOperation async1 = Resources.UnloadUnusedAssets();
        while (!async1.isDone)
            yield return new WaitForEndOfFrame();
        //}
        //else
        //{
        //    Debug.Log("Load Scene ContentRoot = null" );
        //}
		GC.Collect();
        //Resources.UnloadUnusedAssets();
		GC.WaitForPendingFinalizers();

		AsyncOperation async2 = Application.LoadLevelAsync( loadSceneName );
		while( !async2.isDone )
			yield return new WaitForEndOfFrame();

		yield return new WaitForSeconds( 1f );
        if (IsGameScene()) {
            while (FindObjectsOfType<MyPlayer>().Length == 0) {
                yield return new WaitForSeconds(1f);
            }
        }

        if (SU_SpaceSceneCamera.Get)
        {
            if (FindObjectOfType<MouseOrbitRotateZoom>()) {
                SU_SpaceSceneCamera.Get.SetCamera(FindObjectOfType<MouseOrbitRotateZoom>().transform.GetComponent<Camera>());
            }
        }

        yield return new WaitForSeconds(1f);
		//Destroy( gameObjCamera );

		loadSceneName = string.Empty;
        flag = true;
        _loadScanes._camera.gameObject.SetActive(false);

	}

    private bool IsGameScene() {
        return loadSceneName != "select_character";
    }
}