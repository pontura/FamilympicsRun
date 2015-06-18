using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void GotoScore()
    {
        Data.Instance.Load("Score");
    }
    public void ReadScores()
    {
        Data.Instance.Load("ReadScores");
    }
}
