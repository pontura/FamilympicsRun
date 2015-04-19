using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour {

    private float lastDistance;

    public void UpdatePosition(float distance)
    {        
        Vector3 pos = transform.localPosition;
        if (distance > lastDistance+20)
        {

            pos.x = distance;
            transform.localPosition = pos;
            lastDistance = distance + 20;
        }
        
    }
}
