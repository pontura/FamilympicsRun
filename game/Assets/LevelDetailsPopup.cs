using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDetailsPopup : MonoBehaviour {

    public Text field;
    public Text goalText;
    public GameObject panel;
    private bool isActive;
    public GameObject Logout;
    private int levelId;

	void Start () {
        panel.SetActive(false);
        if (Data.Instance.OnlyMultiplayer || FB.IsLoggedIn) Logout.SetActive(false);
	}
    public void StartRace()
    {
        Events.OnSoundFX("buttonPress");
        GetComponent<LevelSelector>().GotoLevel(levelId);
    }
    public void Open(int levelId)
    {
        Events.OnSoundFX("buttonPress");
        this.levelId = levelId;
        isActive = true;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
        field.text = "LEVEL " + levelId;

        Levels.LevelData levelData = Data.Instance.levels.levels[levelId];
        if (levelData.totalLaps > 0)
            goalText.text = (levelData.totalLaps * 1000).ToString() + " MTS";
        else
            goalText.text = (levelData.totalTime).ToString() + " SECS";
    }
    public void Close()
    {
        Events.OnSoundFX("buttonPress");
        panel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        isActive = false;
        panel.SetActive(false);
    }
    public void Login()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("Login");
    }
}
