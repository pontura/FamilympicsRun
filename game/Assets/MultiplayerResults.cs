using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultiplayerResults : MonoBehaviour {

    public Button RePlay;
    public GameObject panel;
    public GameObject finishButton;
    public GameObject shareButton;
    public GameObject energyIcon;
    public Image retryIcon;

    public Text title;
    public Text timeField;

    private string result;

    public Stars stars;
    public MultiplayerResultLine puesto1;
    public MultiplayerResultLine puesto2;
    public MultiplayerResultLine puesto3;
    public MultiplayerResultLine puesto4;



	void Start () {
        finishButton.SetActive(false);
        result = "A";
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(false);
        Events.OnLevelComplete += OnLevelComplete;
        if (Data.Instance.tournament.isOn)
        {
            RePlay.interactable = false;
            retryIcon.color = new Color(retryIcon.color.r, retryIcon.color.g, retryIcon.color.b, 0.2f);
            energyIcon.SetActive(false);
            shareButton.SetActive(false);
        }
	}
    void OnDestroy()
    {
        Events.OnLevelComplete -= OnLevelComplete;
    }
    void OnLevelComplete()
    {
        Invoke("SetOn", 4);
    }
    private string GetTimeFormat(float timer)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        string time = string.Format("{0:00}:{1:00}.{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);
        return time;
    }
    public void Share()
    {
        //SHARE
        Debug.Log("SHARE");
        if (FB.IsLoggedIn)
        {
            Data.Instance.facebookShare.MultiplayerHiscore("My high-score in level " + Data.Instance.levels.currentLevel + " is " + result);
        } 
        else
            Events.OnFacebookNotConnected();
    }
    public void Exit()
    {
        Time.timeScale = 1;
        if (Data.Instance.tournament.isOn)
        {
            Events.OnTournamentFinishAskForConfirmation();
            //panel.SetActive(false);
        }
        else
            Data.Instance.Load("LevelSelector");
    }
    public void SetOn()
    {
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");    

        title.text = "LEVEL " + Data.Instance.levels.currentLevel  + " COMPLETED!";

        string time = GetTimeFormat(Data.Instance.levelData.time);

        MultiplayerData  multiplayerData = Data.Instance.multiplayerData;
        List<MultiplayerData.PlayerData> playersData = multiplayerData.players.OrderBy(x => x.meters).ToList();
        playersData.Reverse();

        int numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        stars.Init(numStars);

        print("STARS: " + numStars);

        float score = 0;
        //si es por tiempo:
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
        {
            score = Data.Instance.levelData.laps;
            result = "TIME: " + time;
            puesto1.Init(playersData[0].username, playersData[0].meters.ToString() + "m", playersData[0].color);

            if (playersData.Count > 1)
                puesto2.Init(playersData[1].username, playersData[1].meters.ToString() + "m", playersData[1].color);
            if (playersData.Count > 2)
                puesto3.Init(playersData[2].username, playersData[2].meters.ToString() + "m", playersData[2].color);
            if (playersData.Count > 3)
                puesto4.Init(playersData[3].username, playersData[3].meters.ToString() + "m", playersData[3].color);
        }
        else if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
        {
            score = Data.Instance.levelData.time;

            playersData = multiplayerData.players.OrderBy(x => x.time).ToList();
            playersData.Reverse();

            result = "SUDDEN DEATH";

            puesto1.Init(playersData[0].username, GetTimeFormat(playersData[0].time), playersData[0].color);

            if (playersData.Count > 1)
                puesto2.Init(playersData[1].username, GetTimeFormat(playersData[1].time), playersData[1].color);
            if (playersData.Count > 2)
                puesto3.Init(playersData[2].username, GetTimeFormat(playersData[2].time), playersData[2].color);
            if (playersData.Count > 3)
                puesto4.Init(playersData[3].username, GetTimeFormat(playersData[3].time), playersData[3].color);

            
        }
        else
        {
            score = Data.Instance.levelData.time;
            puesto1.Init(playersData[0].username, time, playersData[0].color);

            result = playersData[0].meters.ToString() + "m";


            

            if (playersData.Count > 1)
                puesto2.Init(playersData[1].username, "---", playersData[1].color);
            if (playersData.Count > 2)
                puesto3.Init(playersData[2].username, "---", playersData[2].color);
            if (playersData.Count > 3)
                puesto4.Init(playersData[3].username, "---", playersData[3].color);
        }
        timeField.text = result;

        Events.OnSaveScore(Data.Instance.levels.currentLevel, score);
        Events.OnAddMultiplayerScore(Data.Instance.levels.currentLevel, score, playersData[0].playerID, playersData[0].username);      


        //if (multiplayerData.players.Count == 1)
        //{
        //    string result = playersData[0].meters.ToString() + " Mts";

        //    if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
        //        result = "";

        //    puesto1.Init(playersData[0].username, result, playersData[0].color);
        //    return;
        //}
        
        //puesto2.gameObject.SetActive(false);
        puesto3.gameObject.SetActive(false);
        puesto4.gameObject.SetActive(false);

       // if (multiplayerData.players.Count > 1)
          //  puesto2.gameObject.SetActive(true);
        if (multiplayerData.players.Count > 2)
            puesto3.gameObject.SetActive(true);
        if (multiplayerData.players.Count > 3)
            puesto4.gameObject.SetActive(true);


        List<int> arr = new List<int>();
        foreach (MultiplayerData.PlayerData data in playersData)
            arr.Add(data.playerID);

        Events.OnTournamentAddScores(Data.Instance.levels.currentLevel, arr);
        if (Data.Instance.tournament.GetTotalMatches() == 8)
        {
            finishButton.SetActive(true);
            shareButton.SetActive(true);
        }
    }
    public void FinishTournament()
    {
        Events.OnTournamentFinishAskForConfirmation();
    }
    public void Ready()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }

}
