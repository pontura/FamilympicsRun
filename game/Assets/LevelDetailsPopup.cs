using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDetailsPopup : MonoBehaviour {

    public Button challengesButton;
    public Text field;
    public Text goalText;
    public GameObject panel;
    private bool isActive;
    public GameObject Logout;
    private int levelId;
    public Stars stars;
    int starsInLevel;
    

	void Start () {
        panel.SetActive(false);
        if (Data.Instance.OnlyMultiplayer || FB.IsLoggedIn)
        {
            Logout.SetActive(false);
            Vector3 pos = panel.transform.localPosition;
            pos.y = -41;
            panel.transform.localPosition = pos;
        }
        else
        {
            Vector3 pos = panel.transform.localPosition;
            pos.y = 0;
            panel.transform.localPosition = pos;
        }
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

        float _myScore = PlayerPrefs.GetFloat("Run_Level_" + levelId);

        if (_myScore == 0)
            stars.Reset();
        else
        {
            starsInLevel = Data.Instance.levels.GetCurrentLevelStarsByScore(levelId, _myScore);
            stars.Init(starsInLevel);
        }

        if(starsInLevel<1)
            challengesButton.interactable = false;
        else
            challengesButton.interactable = true;

        
        if (levelData.Sudden_Death)
            goalText.text = "SUDDEN DEATH!";
        else if (levelData.totalLaps > 0)
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
        Close();
        Data.Instance.loginManager.FBLogin();
    }
    public void Challenge()
    {
        if (FB.IsLoggedIn)
        {
            Data.Instance.levels.currentLevel = levelId;
            Data.Instance.Load("ChallengeCreator");
        }
        else
        {
            Login();
        }
    }
}
