using UnityEngine;
using System.Collections;

public class LoadingMask : MonoBehaviour {

	public bool infoLoaded;

	// Update is called once per frame
	void Update () {
        if (infoLoaded)
        {
            Destroy(gameObject);
        }
	}
}
