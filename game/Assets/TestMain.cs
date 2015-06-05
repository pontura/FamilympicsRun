using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void GotoScore()
    {
        Application.LoadLevel("Score");
    }
    public void ReadScores()
    {
        Application.LoadLevel("ReadScores");
    }
}
