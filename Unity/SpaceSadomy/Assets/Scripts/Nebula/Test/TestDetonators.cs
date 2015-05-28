using UnityEngine;
using System.Collections;
using Game.Space;
using Nebula;

public class TestDetonators : MonoBehaviour 
{
    public GameObject[] detonatorPrefabs;
    private int currentDetonatorIndex = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Instantiate(detonatorPrefabs[currentDetonatorIndex], G.PlayerComponent.transform.position, Quaternion.identity);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentDetonatorIndex++;
            if (currentDetonatorIndex >= detonatorPrefabs.Length)
                currentDetonatorIndex = 0;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentDetonatorIndex--;
            if (currentDetonatorIndex < 0)
                currentDetonatorIndex = detonatorPrefabs.Length - 1;
        }
    }

    void OnGUI()
    {
        Utils.SaveMatrix();
        GUI.Label(new Rect(0, 0, 200, 200), detonatorPrefabs[currentDetonatorIndex].name);
        Utils.RestoreMatrix();
    }
}
