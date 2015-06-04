using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour {

    private float lastDistance;
    private float distToLoop = 2.98f;
    private float scaleFactor;
    private GameCamera gameCamera;

    public void Init(GameCamera _gameCamera)
    {
        gameCamera = _gameCamera;
     //   scaleFactor = GetComponentInParent<Canvas>().scaleFactor;
        //distToLoop /= scaleFactor;
    }
    public void UpdatePosition()
    {        
        Vector3 pos = transform.localPosition;
        if (gameCamera.distance > lastDistance + distToLoop)
        {

            pos.x = gameCamera.distance;
            transform.localPosition = pos;
            lastDistance = gameCamera.distance + distToLoop;
        }
        
    }
}
