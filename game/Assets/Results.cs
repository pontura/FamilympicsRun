using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Results : MonoBehaviour {

    public Image player;
	// Use this for initialization
	void Start () {
        int id = Data.Instance.levelData.winnerID;
        player.color = Data.Instance.colors[id - 1];
        Events.OnAddMultiplayerScore(Data.Instance.levels.currentLevel, Data.Instance.levelData.score, id, "username");
	}
	
	// Update is called once per frame
	public void Back () {
        Application.LoadLevel("LevelSelector");
	}
}
