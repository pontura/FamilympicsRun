using UnityEngine;
using System.Collections;

public class ResizeToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ResizeSpriteToScreen();
	}

    void ResizeSpriteToScreen()
    {

        float width = 21;
        float height = 21;

        double worldScreenHeight = Camera.main.orthographicSize;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float scaleFactor = ((float)worldScreenWidth / width);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
    }
}
