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

        MultiplayerData multiData = Data.Instance.multiplayerData;

        string username = "";

        switch(id)
        {
            case 1: username = multiData.playerName1; break;
            case 2: username = multiData.playerName2; break;
            case 3: username = multiData.playerName3; break;
            case 4: username = multiData.playerName4; break;
        }

        usernameLabel.text = username;

        usernameLabel.text = Data.Instance.multiplayerData.GetPlayer(id).username;

        LevelData levelData = Data.Instance.levelData;

        System.TimeSpan t = System.TimeSpan.FromSeconds(levelData.time);

        string timerFormatted = string.Format("{0:00}:{1:00}.{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
        Levels.LevelData CurrentlevelData = Data.Instance.levels.GetCurrentLevelData();

        float score = 0;
        if (CurrentlevelData.totalLaps > 0)
        {
            score = levelData.time;
        }
        else if (CurrentlevelData.totalTime > 0)
        {
            score = levelData.laps;
        }

        Events.OnSaveScore(Data.Instance.levels.currentLevel, score);
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
