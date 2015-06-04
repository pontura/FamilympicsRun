using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	void Start () {
	
	}

    public void StartLevel(int id)
    {
        Application.LoadLevel("Players");
        Data.Instance.GetComponent<Levels>().currentLevel = id;
    }
}
