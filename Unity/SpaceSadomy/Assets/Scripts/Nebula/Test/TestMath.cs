namespace Nebula.Test {
    using UnityEngine;

    [ExecuteInEditMode]
    public class TestMath : MonoBehaviour {

        void Update() {

            /*
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            Debug.LogFormat("Unity Euler Angles: {0}", eulerAngles);

            var eulerRad = eulerAngles * Mathf.Deg2Rad;
            var rotMatr = GameMath.Matrix3.MakeEulerYXZ(xAngle: eulerRad.x, yAngle: eulerRad.y, zAngle: eulerRad.z);
            var quat = new GameMath.Quat();
            quat.FromRotationMatrix(rotMatr);

            var rotMatrOut = new GameMath.Matrix3();
            quat.ToRotationMatrix(out rotMatrOut);

            float xAngle, yAngle, zAngle;
            rotMatrOut.ExtractEulerYXZ(out xAngle, out yAngle, out zAngle);
            var eulerOut = new Vector3(xAngle, yAngle, zAngle) * Mathf.Rad2Deg;
            Debug.LogFormat("My Euler Angles: {0}", eulerOut);

            Debug.LogFormat("U Forward: {0}", transform.forward);
            float[] mf = rotMatrOut.forward;
            Debug.LogFormat("M Forward: {0}", new Vector3(mf[0], mf[1], mf[2]));

            print("UMATRIX---------------");
            print(transform.localToWorldMatrix);
            print("MMATRIX----------------");
            print(rotMatrOut.ToString());
            print("SOURCE MMATR--------------");
            print(rotMatr.ToString());*/

            /*
            Quaternion lookRotation = Quaternion.LookRotation(transform.forward);
            Debug.Log("FORWARD: " + transform.forward);
            Vector3 unityEulersAngles = lookRotation.eulerAngles;
            Debug.LogFormat("U look angles " + unityEulersAngles);

            GameMath.Quat qlook = new GameMath.Quat();
            qlook.Look(new GameMath.Vector3(transform.forward.x, transform.forward.y, transform.forward.z));
            float xa, ya, za;
            GameMath.Matrix3 m = new GameMath.Matrix3();
            qlook.ToRotationMatrix(out m);
            m.ExtractEulerYXZ(out xa, out ya, out za);
            Debug.LogFormat("M look angles " + new Vector3(xa, ya, za) * Mathf.Rad2Deg);*/
        }

        private void Out(Quaternion q) {
            Debug.LogFormat("UQ: {0},{1},{2}|{3}", q.x, q.y, q.z, q.w);
        }

        private void Out(GameMath.Quat q) {
            Debug.Log("MQ: " + q.ToString());
        }
    }
}
