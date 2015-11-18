using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;

public class MainMenu : MonoBehaviour {

    public Text label;
    public Button startButton;
    private bool forceStart;

    void Start()
    {
        startButton.gameObject.SetActive(false);
        float vol = Data.Instance.musicVolume;
        Invoke("Init", 1);
    }
    void Init()
    {
        startButton.gameObject.SetActive(true);
        float vol = Data.Instance.musicVolume;
        if (Data.Instance.userData.facebookID == "")
        {
            forceStart = true;
            return;
        }
        else
        {
            label.text = "LOADING...";
            startButton.interactable = false;
        }
        Invoke("ResetLoadings", 8);
    }
    public void GotoLevelSelector()
    {
        Data.Instance.levelsData.ResetLoadings();
        Data.Instance.Load("LevelSelector");
    }
    void Update()
    {
        if (forceStart)
        {
            label.text = "START";
            startButton.interactable = true;
            return;
        }
        if (Data.Instance.levelsData && Data.Instance.levelsData.loadingScores)
        {
            if (Data.Instance.levelsData.totalLevels > 0 && Data.Instance.levelsData.totalLevels <= Data.Instance.levelsData.scoresLoaded)
            {
                forceStart = true;
            }
            else
            {
                label.text = "LOADING...";
                startButton.interactable = false;
            }
        }
    }
    void ResetLoadings()
    {
        forceStart = true;
    }
}
