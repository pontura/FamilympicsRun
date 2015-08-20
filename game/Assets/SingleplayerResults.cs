using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SingleplayerResults : MonoBehaviour
{
    public GameObject ChallengesResultPanel;
    public GameObject panel;

    public Text title;
    public Text timeField;
    public Text resultField;

    public Stars stars;

	void Start () {
        panel.SetActive(false);
        ChallengesResultPanel.SetActive(false);
        Events.OnLevelComplete += OnLevelComplete;
	}
    void OnDestroy()
    {
        Events.OnLevelComplete -= OnLevelComplete;
    }
    void OnLevelComplete()
    {
        Invoke("SetOn", 4);
    }
    public void SetOn()
    {
        if(Data.Instance.levelData.challenge_facebookID == "")
            SetOnSingleResult();
        else
            SetOnChallengeResult();
    }
    private void SetOnChallengeResult()
    {
        ChallengesResultPanel.SetActive(true);
        ChallengesResultPanel.GetComponent<ChallengeResult>().Init();
    }
    public void SetOnSingleResult()
    {
        panel.SetActive(true);        

        title.text = "LEVEL " + Data.Instance.levels.currentLevel  + " COMPLETED!";

        System.TimeSpan t = System.TimeSpan.FromSeconds(Data.Instance.levelData.time);
        timeField.text = "TIME: " + string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);

        int numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        stars.Init(numStars);

        float score = 0;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            score = Data.Instance.levelData.laps;
        else
            score = Data.Instance.levelData.time;

        Events.OnSaveScore(Data.Instance.levels.currentLevel, score);
       // Events.OnAddMultiplayerScore(Data.Instance.levels.currentLevel, score, playersData[0].playerID, playersData[0].username);

        string result = Data.Instance.levelData.laps + " Mts";

        if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
            result = "";

        resultField.text = result;
    }
    public void Ready()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }

}
