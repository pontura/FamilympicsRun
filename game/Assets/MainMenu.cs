using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void NumPlayers(int num)
    {
        Data.Instance.numPlayers = num;
        Application.LoadLevel("Game");
    }
}
