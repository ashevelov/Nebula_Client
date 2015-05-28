using UnityEngine;
using System.Collections;
using Game.Space;
using Game.Network;
using Nebula;

public class VideoCamera : Singleton<VideoCamera> {

    public Camera videoCamera;
    public Texture2D guiGraphic;

    private Transform target;


    void Start()
    {
        this.videoCamera.enabled = false;
    }

    void Update()
    {
        if (!target)
        {
            this.TryFindTarget();
        }

        if (this.target)
        {
            Quaternion q = Quaternion.LookRotation((this.target.position - this.videoCamera.transform.position).normalized);
            this.videoCamera.transform.rotation = q;
        }
    }

    void OnGUI()
    {
        //if (false == NetworkGame.OperationsHelper.GuiHided)
        //{
        //    Utils.SaveMatrix();

        //    var oldColor = GUI.color;
        //    GUI.color = Color.magenta;

        //    var rect = Utils.WorldPos2ScreenRect(transform.position, new Vector2(15, 15));
        //    GUI.DrawTexture(rect, guiGraphic);

        //    GUI.color = oldColor;

        //    Utils.RestoreMatrix();
        //}
    }

    /// <summary>
    /// find player ship
    /// </summary>
    private void TryFindTarget()
    {
        if (G.Game.Avatar != null)
        {
            if (G.Game.Avatar.View)
            {
                this.target = G.Game.Avatar.View.transform;
            }
        }
    }

    public void setVideoCameraEnabled(bool videoCameraEnabled)
    {
        this.videoCamera.enabled = videoCameraEnabled;
    }

    public bool VideoCameraEnabled
    {
        get
        {
            return this.videoCamera.enabled;
        }
    }
}
