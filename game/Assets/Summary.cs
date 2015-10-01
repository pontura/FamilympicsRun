using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Summary : MonoBehaviour {

    public Text title_txt;
    public Text results_txt;

    private LevelData levelData;

	void Start () {

        print("start summary");

        levelData = Data.Instance.levelData;

        System.TimeSpan t = System.TimeSpan.FromSeconds(levelData.time);

        string timerFormatted = string.Format("{0:00}:{1:00}.{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
       Levels.LevelData CurrentlevelData = Data.Instance.levels.GetCurrentLevelData();

       float score = 0;
       if (CurrentlevelData.totalLaps > 0)
       {
           title_txt.text = "TIME";
           results_txt.text = timerFormatted + "SEC";
           score = levelData.time;
       }
       else if (CurrentlevelData.totalTime > 0)
       {
           title_txt.text = "DISTANCE";
           results_txt.text = levelData.laps.ToString() + " MTS";
           score = levelData.laps;
       }
       int currentLevel = Data.Instance.levels.currentLevel;
       Events.OnSaveScore(currentLevel, score);
	}

    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }

}
