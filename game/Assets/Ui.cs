using UnityEngine;
using System.Collections;

public class Ui : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void LevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }
}
