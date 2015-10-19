using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private float distanceToDie;
   // private float distToLoop;
    private GameCamera gameCamera;

    public void Init(GameCamera _gameCamera, Vector3 pos)
    {
        transform.localPosition = pos;
        distanceToDie = pos.x;
        gameCamera = _gameCamera;
    }
    public void InitInScene(Vector3 pos)
    {
        transform.localPosition = pos;
        distanceToDie = pos.x;
    }
    void Update()
    {
        if (!gameCamera) return;
        if (gameCamera.distance > distanceToDie + 30)
            Die();
    }

    public void Die()
    {
        GameObject.Destroy(this.gameObject);
    }
}
