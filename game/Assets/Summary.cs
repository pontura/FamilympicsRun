using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Summary : MonoBehaviour {

    public Text score_txt;
    public Text time_txt;
    public Text laps_txt;
    private LevelData levelData;

	void Start () {

        levelData = Data.Instance.levelData;

        System.TimeSpan t = System.TimeSpan.FromSeconds(levelData.time);
        string timerFormatted = string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);

        score_txt.text = "asd";
        time_txt.text = timerFormatted;
        laps_txt.text = levelData.laps.ToString();
	}

    public void GotoLevelSelector()
    {
        Application.LoadLevel("LevelSelector");
    }

}
