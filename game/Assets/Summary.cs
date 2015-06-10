using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Summary : MonoBehaviour {

    public Text title_txt;
    public Text results_txt;

    private LevelData levelData;

	void Start () {

        levelData = Data.Instance.levelData;

        System.TimeSpan t = System.TimeSpan.FromSeconds(levelData.time);
        string timerFormatted = string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
       Levels.LevelData CurrentlevelData = Data.Instance.levels.GetCurrentLevelData();

       float score = 0;
       if (CurrentlevelData.totalLaps > 0)
       {
           title_txt.text = "TIME";
           results_txt.text = timerFormatted;
           score = levelData.time;
       }
       else if (CurrentlevelData.totalTime > 0)
       {
           title_txt.text = "LAPS";
           results_txt.text = levelData.laps.ToString();
           score = levelData.laps;
       }

       int currentLevel = Data.Instance.levels.currentLevel;
       Events.OnSaveScore(currentLevel, score);

       if (Data.Instance.levelData.challenge_username != "")
       {
           string username = Data.Instance.levelData.challenge_username;
           string facebookID = Data.Instance.levelData.challenge_facebookID;
           Events.OnChallengeCreate(username, facebookID, currentLevel, score);
           Data.Instance.levelData.ResetChallenge();
       }
       
	}

    public void GotoLevelSelector()
    {
        Application.LoadLevel("LevelSelector");
    }

}
