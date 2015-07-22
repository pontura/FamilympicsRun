using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

    public GameObject FinishAsset;

	void Start () {
        Events.OnLevelComplete += OnLevelComplete;
        FinishAsset.SetActive(false);
	}
	
	// Update is called once per frame
	void OnDestroy () {
        Events.OnLevelComplete -= OnLevelComplete;
	}
    void OnLevelComplete()
    {
        FinishAsset.SetActive(true);
        FinishAsset.GetComponent<Animation>().Play("FinishFlagOpen");
    }
}
