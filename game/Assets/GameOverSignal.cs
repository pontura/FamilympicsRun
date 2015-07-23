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
        field.text = "LEVEL " +levelId+ "FAILED!";

        Invoke("Reset", 3f);
    }
    public void Replay()
    {

    }
    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }
}
