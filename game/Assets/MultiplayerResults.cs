using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultiplayerResults : MonoBehaviour {

    public GameObject panel;

    public Text title;
    public Text timeField;

    public Stars stars;
    public MultiplayerResultLine puesto1;
    public MultiplayerResultLine puesto2;
    public MultiplayerResultLine puesto3;
    public MultiplayerResultLine puesto4;

	void Start () {
        panel.SetActive(false);
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
    private string GetTimeFormat(float timer)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        string time = string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);
        return time;
    }
    public void SetOn()
    {
        panel.SetActive(true);        

        title.text = "LEVEL " + Data.Instance.levels.currentLevel  + " COMPLETED!";

        string time = GetTimeFormat(Data.Instance.levelData.time);

        MultiplayerData  multiplayerData = Data.Instance.multiplayerData;
        List<MultiplayerData.PlayerData> playersData = multiplayerData.players.OrderBy(x => x.meters).ToList();
        playersData.Reverse();

        int numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        stars.Init(numStars);

        float score = 0;
        //si es por tiempo:
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
        {
            score = Data.Instance.levelData.laps;
            timeField.text = "TIME: " + time;
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
            playersData = multiplayerData.players.OrderBy(x => x.time).ToList();
            playersData.Reverse();

            timeField.text = "SUDDEN DEATH";

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

            string result = playersData[0].meters.ToString() + "m";


            timeField.text = result;

            if (playersData.Count > 1)
                puesto2.Init(playersData[1].username, "---", playersData[1].color);
            if (playersData.Count > 2)
                puesto3.Init(playersData[2].username, "---", playersData[2].color);
            if (playersData.Count > 3)
                puesto4.Init(playersData[3].username, "---", playersData[3].color);
        }

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



         
    }
    public void Ready()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }

}
