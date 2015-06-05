using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private float lastDistance;
    private float distToLoop;
    private GameCamera gameCamera;

    public void Init(GameCamera _gameCamera)
    {
        lastDistance = distToLoop = transform.localPosition.x;
        gameCamera = _gameCamera;
    }
    void Update()
    {
        Vector3 pos = transform.localPosition;
        if (gameCamera.distance > lastDistance + distToLoop)
        {
            lastDistance = gameCamera.distance + distToLoop;
            pos.x = lastDistance;
            transform.localPosition = pos;
        }

    }

    public void Die()
    {
        GameObject.Destroy(this.gameObject);
    }
}
