using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;

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

    private string result;

	void Start () {
       // if (!FB.IsLoggedIn) share.SetActive(false);
        //if (!FB.IsLoggedIn) challengeButton.interactable = false;
        panel.transform.localScale = Data.Instance.screenManager.scale;
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

    string time;
    int numStars;

    void OnLevelComplete()
    {
        title.text = "LEVEL " + Data.Instance.levels.currentLevel;
        Invoke("SaveScore", 1);
    }

    private float score = 0;

    void SaveScore()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(Data.Instance.levelData.time);
        time = string.Format("{0:00}:{1:00}.{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);

        numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            score = Data.Instance.levelData.laps;
        else
            score = Data.Instance.levelData.time;

        if (Data.Instance.levelData.dontSaveScore)
            Debug.Log("NO GRABA SCORE PORQUE ES UN CHALLENGE DE UN LEVEL QUE NO JUGUE");
        else
           Events.OnSaveScore(Data.Instance.levels.currentLevel, score);

        Invoke("SetOn", 3);
    }
    public void SetOn()
    {
        if (Data.Instance.levelData.challenge_facebookID == "")
        {
            SetOnSingleResult();
            Events.OnCheckIfAutomaticChallenge(score);
        }
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
        if (FB.IsLoggedIn)
        {
            Data.Instance.facebookShare.NewHiscore("My high-score in level " + Data.Instance.levels.currentLevel + " is " +  result);
        }
        else
        {
            Events.OnFacebookNotConnected();
        }
    }
    public void SetOnSingleResult()
    {
       
        panel.SetActive(true);
        stars.Init(numStars);

        float lastScore = Data.Instance.levelsData.GetLevelScores(Data.Instance.levels.currentLevel).myScore;
        float score = 0;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
        {
            score = Data.Instance.levelData.laps;

            result = score + "m";
            hiscoreField.text = "MY BEST " + lastScore + "m";
            //timeField.text = time; 
        }
        else
        {
            score = Data.Instance.levelData.time;
            System.TimeSpan lastT = System.TimeSpan.FromSeconds(lastScore);
            string lastTime = string.Format("{0:00}:{1:00}.{2:00}", lastT.Minutes, lastT.Seconds, lastT.Milliseconds / 10);
            hiscoreField.text = "MY BEST " + lastTime;

            result = time;

         //   if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
            //    result = "";
            
        }
        resultField.text = result;
        if (lastScore == 0) hiscoreField.text = "NEW HISCORE";

       // Events.OnSaveScore(Data.Instance.levels.currentLevel, score);

        Invoke("LoadResults", 1);
    }
    void LoadResults()
    {
         GetComponent<Ranking>().LoadSinglePlayerWinners(Data.Instance.levels.currentLevel);
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
