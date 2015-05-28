using UnityEngine;
using System.Collections;
using Game.Space;

public class TestScreenSpot : MonoBehaviour {

    /*
    public float spotRadius = 0.02f;
    public Color spotColor = Color.white;
    public float attenuation = 0.5f;
    public float minDistance = 10;
    private float maxDistance = 20000;
    private float curDistance = 1000;


    public Texture2D texture;
    private Material material;

    private bool draw = false;


    void Start()
    {
        this.material = new Material(Shader.Find("Custom/ScreenSpot"));
        this.UpdateShaderParameters();
    }

    void OnBecameVisible()
    {
        draw = true;
    }

    void OnBecameInvisible()
    {
        draw = false;
    }



    private void UpdateShaderParameters()
    {
        var game = MmoEngine.Get.Game;
        this.curDistance = 0;

        if (game.Avatar != null)
        {
            if (game.Avatar.View)
            {
                var playerPos = game.Avatar.View.transform.position;
                curDistance = Vector3.Distance(playerPos, transform.position);
            }
        }

        var v = Utils.UVVector(transform.position);
        material.SetVector("_SpotUV", new Vector4(v.x, v.y, 0, 0));

        material.SetTexture("_MainTex", texture);
        material.SetFloat("_SpotRadius", this.spotRadius);
        material.SetColor("_SpotColor", this.spotColor);
        material.SetFloat("_Attenuation", this.attenuation);
        material.SetFloat("_MinDistance", this.minDistance);
        material.SetFloat("_MaxDistance", this.maxDistance);
        material.SetFloat("_CurDistance", this.curDistance);
    }
    void Update()
    {
        if (draw)
        {
            this.UpdateShaderParameters();
        }
    }

    void OnGUI()
    {
        if (draw)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Utils.SaveMatrix();
                Graphics.DrawTexture(new Rect(0, 0, Utils.NativeWidth, Utils.NativeHeight), texture, material);
                Utils.RestoreMatrix();
            }
        }
    }
     * */

}
