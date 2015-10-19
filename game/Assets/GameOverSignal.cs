using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverSignal : MonoBehaviour {

    public GameObject panel;
    public GameObject panelTimeOver;
    public Text field;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panelTimeOver.SetActive(false);
        panel.SetActive(false);
        Events.GameOver += GameOver;
	}
    void OnDestroy()
    {
        Events.GameOver -= GameOver;
    }
    public void Buy()
    {
        Events.BuyEnergyPack();
        Replay();
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
       int totalPlayers = Data.Instance.multiplayerData.players.Count;
        if(totalPlayers>1)
            Data.Instance.Load("Game");
        else Data.Instance.Load("GameSingle");
    }
    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }
}
