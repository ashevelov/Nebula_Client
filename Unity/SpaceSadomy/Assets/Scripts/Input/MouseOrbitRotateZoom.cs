#define MOUSE_CAMERA_ONLY
using Game.Space;
using Nebula;
using Nebula.Client;
using UnityEngine;


public class MouseOrbitRotateZoom : Singleton<MouseOrbitRotateZoom> 
{
    public Transform StartTarget;
    public NormalCameraParameters normalCameraParameters;
    public EllipseCameraParameters ellipseCameraParameters;
    public TouchCameraParameters touchCameraParameters;

    private bool cameraActive = true;

    private GrayscaleEffect grayScaleEffect;
    private float grayScaleFadeTimer = 0.0f;
    private bool grayScaleFadeFadeStarted = false;
    private float GRA_FADE_DUR = 1;


    private CameraStrategy cameraStrategy;

    public void SetCameraStrategy(CameraStrategyType type)
    {
        Transform prevTarget = null;
        if (this.cameraStrategy != null)
            prevTarget = this.cameraStrategy.TargetTransform();

        switch(type)
        {
            case CameraStrategyType.Normal:
                {

                    this.cameraStrategy = new NormalCameraStrategy(this.transform, this.normalCameraParameters);
                    break;
                }
            case CameraStrategyType.Ellipse:
                {
                    this.cameraStrategy = new EllipseCameraStrategy(this.transform, this.ellipseCameraParameters);
                    break;
                }
            case CameraStrategyType.Free:
                {
                    this.cameraStrategy = new FreeCameraStrategy(this.transform);
                    break;
                }
            case CameraStrategyType.Touch:
                {
                    this.cameraStrategy = new TouchCameraStrategy(this.transform, this.touchCameraParameters);
                    break;
                }
            default:
                throw new NebulaException("Not supported camera strategy type");
        }
        this.cameraStrategy.Initialize();
        if (prevTarget != null)
            this.cameraStrategy.SetTarget(prevTarget);
    }

    private void InitCameraStrategyAtStart () {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            this.SetCameraStrategy(CameraStrategyType.Touch);
        } else {
#if MOUSE_CAMERA_ONLY
            this.SetCameraStrategy(CameraStrategyType.Normal);
#else
                this.SetCameraStrategy(CameraStrategyType.Touch);
#endif
        }
    }

    /// <summary>
    /// Set camera target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        if (this.cameraStrategy == null) {
            this.InitCameraStrategyAtStart();
        }
        this.cameraStrategy.SetTarget(target);
    }

    void Start() 
    {
        this.InitCameraStrategyAtStart();
        if(this.StartTarget) {
            this.cameraStrategy.SetTarget(this.StartTarget);
        }

        GameObject tempGO = GameObject.FindGameObjectWithTag("SpaceScene_Camera");
        grayScaleEffect = GetComponent<GrayscaleEffect>();
    }

    public void SetCameraActive(bool value)
    {
        this.cameraActive = value;
    }

    public bool CameraActive
    {
        get
        {
            return this.cameraActive;
        }
    }
    public void StartGrayScale()
    {
        if (grayScaleEffect && (false == grayScaleFadeFadeStarted))
        {
            grayScaleEffect.enabled = true;
            grayScaleFadeTimer = 0;
            grayScaleFadeFadeStarted = true;
        }
    }

    public void StopGrayScae()
    {
        if (grayScaleEffect)
        {
            grayScaleEffect.enabled = false;
            grayScaleEffect.rampOffset = 0.7f;
            grayScaleFadeFadeStarted = false;
        }
    }

    private bool grayed = false;

    public void Gray()
    {
        grayScaleEffect.enabled = true;
        grayScaleEffect.rampOffset = -0.12f;
    }

    public void Ungray()
    {
        grayScaleEffect.enabled = false;
        grayScaleEffect.rampOffset = 0.7f;
    }

    void Update() 
    {
        if (grayScaleFadeFadeStarted)
        {
            grayScaleFadeTimer += Time.deltaTime;
            float normTimer = Mathf.Clamp01(grayScaleFadeTimer / GRA_FADE_DUR);
            float curFade = Mathf.Lerp(0.7f, -0.12f, normTimer);
            grayScaleEffect.rampOffset = curFade;
            if (normTimer > GRA_FADE_DUR)
            {
                grayScaleFadeFadeStarted = false;
            }
        }
    }
    void LateUpdate() 
    {
        if (!this.CameraActive) { return; }

        this.cameraStrategy.UpdateCamera();
        if(SU_SpaceSceneCamera.Get ) {
            SU_SpaceSceneCamera.Get.CameraUpdate();
        }
#if UNITY_EDITOR
        if(G.IsPlayerValid () ) {
            if(CrossPlatformInput.IsButton(1)) {
                G.PlayerComponent.SetTargetDirection(transform.forward);
            }
        }
#endif
    }

}
