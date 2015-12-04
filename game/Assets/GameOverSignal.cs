using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverSignal : MonoBehaviour {

    public GameObject panel;
    public GameObject panelTimeOver;
    public Text field;
    public GameObject EnergyIcon;
    public Text EndRaceButtonField;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panelTimeOver.SetActive(false);
        panel.SetActive(false);
        Events.GameOver += GameOver;

        if (Data.Instance.tournament.isOn)
        {
            EnergyIcon.SetActive(false);
            EndRaceButtonField.text = "FINISH";
        }
	}
    void OnDestroy()
    {
        Events.GameOver -= GameOver;
    }
    public void Buy()
    {
        Data.Instance.Load("Buy");
    }
    void GameOver(bool byTime)
    {
        //if (byTime)
        //{
        //    print("GameOverByTime");
        //    panelTimeOver.SetActive(true);
        //}
        //else
        //{
            print("GameOver");
            panel.SetActive(true);
           
            int levelId = Data.Instance.levels.currentLevel;
            field.text = "LEVEL " + levelId + " FAIL!";

            
       // }
        panel.GetComponent<Animation>().Play("PopupOn");
       // Invoke("Reset", 3f);
    }
    public void Replay()
    {
       if(Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            Data.Instance.Load("Game");
        else 
           Data.Instance.Load("GameSingle");
    }
    public void GotoLevelSelector()
    {
        Time.timeScale = 1;
        if (Data.Instance.tournament.isOn)
        {
            FinishTournament();
        }
        else
        {
            Data.Instance.Load("LevelSelector");
        }
    }
    public void FinishTournament()
    {
        if (Data.Instance.tournament.GetTotalMatches() == 8)
        {
            Events.OnTournamentFinish();
        }
        else
        {
            Events.OnTournamentFinishAskForConfirmation();
        }
    }
}
