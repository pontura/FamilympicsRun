using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour {

    private float lastDistance;
    private float distToLoop = 2.98f;
    private GameCamera gameCamera;

    public void Init(GameCamera _gameCamera)
    {
        gameCamera = _gameCamera;
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
