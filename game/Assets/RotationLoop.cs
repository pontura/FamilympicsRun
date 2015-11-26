using UnityEngine;
using System.Collections;

public class RotationLoop : MonoBehaviour {

	void Update () {
        Vector3 rot = transform.localEulerAngles;
        rot.z -= 2;
        transform.localEulerAngles = rot;
	}
}
