using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using System.Collections.Generic;
using Game.Space.UI;
using Nebula;

public class LoreObject : MonoBehaviour
{
    //private GameObjectScreenSelection screenView = new GameObjectScreenSelection();

    void OnEnable()
    {
		//screenView.Initialize(gameObject, Vector2.one * 20f, TextureCache.Get("UI/Textures/Object_icons/asd"));
    }

    void OnDisable()
    {
        //this.screenView.Release();
    }
    private float activateDistance = 80;



    void Update()
    {
        if(G.Game == null )
        {
            return;
        }
        if (G.Game.Avatar == null)
        {
            Dbg.Print("Avatar is null", "NPC");
            return;
        }
        if (!G.Game.Avatar.Component)
        {
            Dbg.Print("Avatar component is null", "NPC");
            return;
        }
        if (G.Game.Avatar.ShipDestroyed)
        {
            Dbg.Print("Avatar ship destroyed", "NPC");
            return;
        }
        float distance = Vector3.Distance(G.Game.Avatar.Component.Position, transform.position);
        if (distance < this.activateDistance)
        {
			optimalDistans = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
				LoreWindow.Show();
				Destroy(gameObject);
            }
        }
        else
        {
			optimalDistans = false;
        }
    }

    private bool optimalDistans;

    void OnGUI()
    {
		if (optimalDistans)
        {
            Utils.SaveMatrix();
			if (GUI.Button(new Rect(Screen.width /2 / Utils.GameMatrix().m00 - 50, Screen.height / Utils.GameMatrix().m00 - 300, 300, 50), "<size=25>Explore object (E)</size>", GUI.skin.label))
			{
				LoreWindow.Show();
				Destroy(gameObject);
            }

            Utils.RestoreMatrix(); ;
        }
    }

    
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, this.activateDistance);
        }
    }
}
