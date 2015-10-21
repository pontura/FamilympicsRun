using UnityEngine;
using System.Collections;

public class VerticalEnemy : Enemy {

    private float _speed;
    private int Y_limit = 20;

    public void InitVerticalEnemy(Levels.VerticalBar data)
    {
        float scale = data.size - 0.5f;
        transform.localScale = new Vector3(1, scale, 1);
        transform.localPosition = new Vector3(data._x, data._y, 0);
        this._speed = data.speed;
    }
    override public void OnUpdate()
    {
        print("ASDSASDASA" + _speed);
        Vector3 pos = transform.localPosition;
        pos.y -= Time.deltaTime * _speed;
       
        if (pos.y < -Y_limit) pos.y = Y_limit;
        if (pos.y > Y_limit) pos.y = -Y_limit;

        transform.localPosition = pos;

    }
}
