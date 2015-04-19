using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    public void Move(float speed)
    {
        Vector3 pos = transform.localPosition;
        pos.x -= speed;
        transform.localPosition = pos;
    }
}
