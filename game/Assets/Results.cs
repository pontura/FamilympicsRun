using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Results : MonoBehaviour {

    public Image player;
	// Use this for initialization
	void Start () {
        int id = Data.Instance.levelData.winnerID;
        player.color = Data.Instance.colors[id - 1];
	}
	
	// Update is called once per frame
	public void Back () {
        Application.LoadLevel("LevelSelector");
	}
}
