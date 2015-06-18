using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Results : MonoBehaviour {

    public Image player;
    public Text usernameLabel;

    public int id;

	void Start () {
        id = Data.Instance.levelData.winnerID;
        player.color = Data.Instance.colors[id - 1];
        usernameLabel.text = Data.Instance.multiplayerData.GetPlayer(id).username;
	}
	
	public void Back () {
        float score = 0;

        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            score = Data.Instance.levelData.laps;
        else
            score = Data.Instance.levelData.time;

        Events.OnAddMultiplayerScore(Data.Instance.levels.currentLevel, score, id, usernameLabel.text);
        Data.Instance.Load("LevelSelector");
	}
}
