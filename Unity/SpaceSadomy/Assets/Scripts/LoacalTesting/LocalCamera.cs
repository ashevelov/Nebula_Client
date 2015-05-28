/*
using UnityEngine;
using System.Collections;

namespace LocalTest
{

    public class LocalCamera : MonoBehaviour
    {

        public Transform target;
        public float distance = 10.0f;
        public float minDistance = 1.5f;

        public float xSpeed = 0.2f;
        public float ySpeed = 0.2f;

        public bool newTarget = true;


        public float touchSensitivity = 3f;

        private float sensitivityZoom = 0.3f;

        public float yMinLimit = -20;
        public float yMaxLimit = 80;

        private float x = 0.0f;
        private float y = 0.0f;


        private Vector3 deltaCameraPosition;


        private bool moveToDefaultPusitionInIsland = false;


        private Vector2 oldMpos = new Vector2();
        private Vector2 prevDist = new Vector2();
        private bool resetzoom = true;

        private Vector2 deltaPositionEndTouch;



        public Vector3 sDefaultRot;
        public Vector3 sDefaultPos;
        public Vector3 sMaxPos;

        public Camera _camera;
        private SU_SpaceSceneCamera _su_camera;
        public bool su_cameraLoad = false;

        void Start()
        {
            _camera = GetComponent<Camera>();
            if (su_cameraLoad)
            {
                GameObject tempGO = GameObject.FindGameObjectWithTag("SpaceScene_Camera");
                if (tempGO != null)
                    _su_camera = tempGO.GetComponent<SU_SpaceSceneCamera>();
            }
        }

        LocalPlayerShip player = null;

        void FixedUpdate()
        {
            distance -= (Input.GetAxis("Mouse ScrollWheel")) * (distance / 3);



            if (target)
            {
                if(player == null)
                {
                    player = target.GetComponent<LocalPlayerShip>();
                }
                //if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) )
                //{
                //    oldMpos = Input.mousePosition;
                //    deltaPositionEndTouch = new Vector2(0.0f, 0.0f);
                //}
                //if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                //{
                if(player.GetBattleMod())
                {
                    Vector2 f = Input.mousePosition;
                    //deltaPositionEndTouch = f - oldMpos;
                    deltaPositionEndTouch.x += Input.GetAxis("Mouse X") * xSpeed;
                    deltaPositionEndTouch.y += Input.GetAxis("Mouse Y") * ySpeed;
                    oldMpos = f;
                    moveToDefaultPusitionInIsland = false;
                }
                //}


                deltaPositionEndTouch.x *= (xSpeed * (2048.0f / (float)Screen.width));
                deltaPositionEndTouch.y *= (ySpeed * (1536.0f / (float)Screen.height));

                x += deltaPositionEndTouch.x;
                y -= deltaPositionEndTouch.y;

                if (x < 0)
                {
                    x += 360;
                }
                if (x > 360)
                {
                    x -= 360;
                }

                if (distance < minDistance)
                    distance = minDistance;
                if (distance > 5000)
                    distance -= Time.deltaTime * 300;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
                //if (Input.GetMouseButton(1))
                //{
                //    x = 0;
                //    y = 16;
                //}

                //deltaCameraPosition = oldCameraPosition - transform.position;


                transform.localRotation = Quaternion.Euler(y, x, 0);
                transform.localPosition = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position + deltaCameraPosition;
            }

            deltaCameraPosition -= deltaCameraPosition * Time.deltaTime;

            if (Mathf.Abs(deltaCameraPosition.x) < 0.2f)
                deltaCameraPosition.x = 0f;
            if (Mathf.Abs(deltaCameraPosition.y) < 0.2f)
                deltaCameraPosition.y = 0f;
            if (Mathf.Abs(deltaCameraPosition.z) < 0.2f)
                deltaCameraPosition.z = 0f;




            if (su_cameraLoad)
                _su_camera.CameraUpdate();
            //deltaPositionEndTouch -= deltaPositionEndTouch * 2 * Time.deltaTime;
        }



        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }

    }

}*/