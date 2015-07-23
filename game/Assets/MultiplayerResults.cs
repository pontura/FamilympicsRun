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
    public void SetOn()
    {
        panel.SetActive(true);        

        title.text = "LEVEL " + Data.Instance.levels.currentLevel  + " COMPLETED!";

        System.TimeSpan t = System.TimeSpan.FromSeconds(Data.Instance.levelData.time);
        timeField.text = "TIME: " + string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds / 10);

        MultiplayerData  multiplayerData = Data.Instance.multiplayerData;
        List<MultiplayerData.PlayerData> playersData = multiplayerData.players.OrderBy(x => x.meters).ToList();
        playersData.Reverse();

        int numStars = Data.Instance.levels.GetCurrentLevelStars(Data.Instance.levelData.time, Data.Instance.levelData.laps);
        stars.Init(numStars);

        float score = 0;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            score = Data.Instance.levelData.laps;
        else
            score = Data.Instance.levelData.time;

        Events.OnSaveScore(Data.Instance.levels.currentLevel, score);
        Events.OnAddMultiplayerScore(Data.Instance.levels.currentLevel, score, playersData[0].playerID, playersData[0].username);

        if (multiplayerData.players.Count == 1)
        {
            puesto1.Init(playersData[0].username, playersData[0].meters.ToString() + " Mts", playersData[0].color);
            return;
        }
        
        puesto2.gameObject.SetActive(false);
        puesto3.gameObject.SetActive(false);
        puesto4.gameObject.SetActive(false);

        if (multiplayerData.players.Count > 1)
            puesto2.gameObject.SetActive(true);
        if (multiplayerData.players.Count > 2)
            puesto3.gameObject.SetActive(true);
        if (multiplayerData.players.Count > 3)
            puesto4.gameObject.SetActive(true);


         puesto1.Init(playersData[0].username, playersData[0].meters.ToString() + " Mts", playersData[0].color);
         if (playersData.Count>1)
             puesto2.Init(playersData[1].username, playersData[1].meters.ToString() + " Mts", playersData[1].color);
         if (playersData.Count > 2)
             puesto3.Init(playersData[2].username, playersData[2].meters.ToString() + " Mts", playersData[2].color);
         if (playersData.Count > 3)
             puesto4.Init(playersData[3].username, playersData[3].meters.ToString() + " Mts", playersData[3].color);
    }
    public void Ready()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }

}
