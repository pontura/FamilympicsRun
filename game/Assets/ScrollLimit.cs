using UnityEngine;
using System.Collections;

public class ScrollLimit : MonoBehaviour {

    public Vector2 limitX;
    public GameObject container;

	void Update () {

        if (container.transform.localPosition.x > limitX.x)
            container.transform.localPosition = new Vector3(limitX.x, container.transform.localPosition.y, 0);
        else if (container.transform.localPosition.x < limitX.y)
            container.transform.localPosition = new Vector3(limitX.y, container.transform.localPosition.y, 0);
	}
    public void SetLimit(Vector2 _limitX)
    {
        this.limitX = _limitX;
    }
}
