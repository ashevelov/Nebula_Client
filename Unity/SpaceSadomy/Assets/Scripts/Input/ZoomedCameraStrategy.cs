using UnityEngine;
using System.Collections;

namespace Game.Space
{
    public abstract class ZoomedCameraStrategy : CameraStrategy 
    {
        private const float DEFAULT_ZOOM_DISTANCE_MULT = 0.1f;

        private MinMax orbitDistanceRange;
        private float scrollSpeed;
        private float distanceChangeSpeed;
   
        private float targetOrbitDistance;
        private float currentOrbitDistance;

        public ZoomedCameraStrategy(Transform cameraTransform, ZoomedCameraParameters parameters)
            : base(cameraTransform)
        {
            this.orbitDistanceRange = parameters.OrbitDistanceRange;
            this.scrollSpeed = parameters.MouseScrollSpeed;
            this.distanceChangeSpeed = parameters.DistanceChangeSpeed;
        }

        protected float ScrollSpeed()
        {
            return this.scrollSpeed;
        }

        protected float MinOrbitDistance()
        {
            return this.orbitDistanceRange.Min;
        }

        protected float MaxOrbitDistance()
        {
            return this.orbitDistanceRange.Max;
        }
       
        protected float DistanceChangeSpeed()
        {
            return this.distanceChangeSpeed;
        }

        protected float TargetOrbitDistance()
        {
            return this.targetOrbitDistance;
        }

        protected float CurrentOrbitDistance()
        {
            return this.currentOrbitDistance;
        }

        public override void SetTarget(Transform targetTransform)
        {
            base.SetTarget(targetTransform);
        }

        public override void Initialize()
        {
            this.targetOrbitDistance = this.DefaultZoomDistance();
        }

        protected float DefaultZoomDistance()
        {
            return (this.MinOrbitDistance() + this.MaxOrbitDistance()) * DEFAULT_ZOOM_DISTANCE_MULT;
        }

        protected void UpdateTargetOrbitDistance()
        {
            this.targetOrbitDistance -= CrossPlatformInput.Scroll * this.ScrollSpeed();
            this.targetOrbitDistance = Mathf.Clamp(this.targetOrbitDistance, this.MinOrbitDistance(), this.MaxOrbitDistance());
        }

        protected void UpdateCurrentOrbitDistance()
        {
            this.currentOrbitDistance = Mathf.Lerp(this.CurrentOrbitDistance(), this.TargetOrbitDistance(), Time.deltaTime * this.DistanceChangeSpeed());
            this.currentOrbitDistance = Mathf.Clamp(this.CurrentOrbitDistance(), this.MinOrbitDistance(), this.MaxOrbitDistance());
        }
    }
}
