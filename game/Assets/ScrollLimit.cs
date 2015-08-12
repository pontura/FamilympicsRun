using UnityEngine;
using System.Collections;

public class ScrollLimit : MonoBehaviour {

    public Vector2 limitY;
    public Vector2 limitX;
    public GameObject container;

	void Update () {

        if (limitX != null)
        {
            if (container.transform.localPosition.x > limitX.x)
                container.transform.localPosition = new Vector3(limitX.x, container.transform.localPosition.y, 0);
            else if (container.transform.localPosition.x < limitX.y)
                container.transform.localPosition = new Vector3(limitX.y, container.transform.localPosition.y, 0);
        }
        if (limitY != null)
        {
            if (container.transform.localPosition.y > limitY.y)
                container.transform.localPosition = new Vector3(container.transform.localPosition.x, limitY.y, 0);
            else if (container.transform.localPosition.y < limitY.x)
                container.transform.localPosition = new Vector3(container.transform.localPosition.x, limitY.x, 0);
        }
	}
    public void SetLimit(Vector2 _limitX)
    {
        this.limitX = _limitX;
    }

}
