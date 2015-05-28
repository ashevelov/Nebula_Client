using UnityEngine;
using System.Collections;

namespace Game.Space
{
    [System.Serializable]
    public class NormalCameraParameters : ZoomedCameraParameters
    {
        public Vector2 XYMouseSpeed = new Vector2(12, 12);
        public MinMax YAngleMinMax = new MinMax(-80, 80);
        public float RotationSmoothTime = 0.3f;
    }

    [System.Serializable]
    public class EllipseCameraParameters : ZoomedCameraParameters
    {
        public float OrbitSpeed = 90.0f;
        public Vector3 UpVector = Vector3.up;
    }

    [System.Serializable]
    public class ZoomedCameraParameters
    {
        public MinMax OrbitDistanceRange = new MinMax(1, 300);
        public float DistanceChangeSpeed = 0.3f;
        public float MouseScrollSpeed = 10;
    }

    [System.Serializable]
    public class TouchCameraParameters {
        public Vector2 RotateSpeed = new Vector2(0.1f, 0.1f);
        public MinMax DistanceLimits = new MinMax(10f, 20.0f);
        public float ChangeDistanceSpeed = 0.01f;
        public MinMax YAngleLimits = new MinMax(-80.0f, 80.0f);
    }
}
