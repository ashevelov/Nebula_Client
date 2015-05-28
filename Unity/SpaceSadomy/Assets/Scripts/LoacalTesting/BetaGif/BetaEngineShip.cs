using UnityEngine;
using System.Collections;

namespace LocalTest
{
    public class BetaEngineShip : MonoBehaviour
    {
        public float rotationSpeed = 0.5f;
        Vector3 rot;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                rot += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            }
            rot -= rot * Time.deltaTime;
            transform.eulerAngles += rot * Time.deltaTime * rotationSpeed;
        }
    }
}
