using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverSignal : MonoBehaviour {

    public GameObject panel;
    public Text field;

	void Start () {
        panel.SetActive(false);
        Events.GameOver += GameOver;
	}
    void OnDestroy()
    {
        Events.GameOver -= GameOver;
    }
    void GameOver()
    {
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
        int levelId = Data.Instance.levels.currentLevel;
        field.text = "LEVEL " +levelId+ " FAIL!";

        Invoke("Reset", 3f);
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
