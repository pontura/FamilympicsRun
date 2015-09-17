using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SingleplayerResults : MonoBehaviour
{
    public GameObject ChallengesResultPanel;
    public GameObject panel;
    public GameObject share;
    public Button challengeButton;

    public Text title;
    
    public Text resultField;
    public Text hiscoreField;
    public Stars stars;

	void Start () {
        if (!FB.IsLoggedIn) share.SetActive(false);
        if (!FB.IsLoggedIn) challengeButton.interactable = false;

        panel.SetActive(false);
        ChallengesResultPanel.SetActive(false);
        Events.OnLevelComplete += OnLevelComplete;
        Events.OnNewHiscore += OnNewHiscore;
	}
    void OnDestroy()
    {
        Events.OnLevelComplete -= OnLevelComplete;
        Events.OnNewHiscore -= OnNewHiscore;
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
    public void Share()
    {

    }
    public void SetOnSingleResult()
    {
        panel.SetActive(true);        

        title.text = "LEVEL " + Data.Instance.levels.currentLevel;

        System.TimeSpan t = System.TimeSpan.FromSeconds(Data.Instance.levelData.time);
        string time =  string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);
        

        int numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        stars.Init(numStars);

        float lastScore = Data.Instance.levelsData.GetLevelScores(Data.Instance.levels.currentLevel).myScore;
        float score = 0;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
        {
            score = Data.Instance.levelData.laps;

            resultField.text = score + "m";
            hiscoreField.text = "BEST " + lastScore + "m";
            //timeField.text = time; 
        }
        else
        {
            score = Data.Instance.levelData.time;
            System.TimeSpan lastT = System.TimeSpan.FromSeconds(lastScore);
            string lastTime = string.Format("{0:00}:{1:00}:{2:00}", lastT.Minutes, lastT.Seconds, lastT.Milliseconds / 10);
            hiscoreField.text = "BEST " + lastTime ;

            string result = time;

            if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
                result = "";

            resultField.text = result;
        }
        if (lastScore == 0) hiscoreField.text = "NEW HISCORE";
        
        Events.OnSaveScore(Data.Instance.levels.currentLevel, score);        
    }
    void OnNewHiscore(int levelId, float score)
    {
        hiscoreField.text = "NEW HISCORE";
    }
    public void Ready()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }

}
