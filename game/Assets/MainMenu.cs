using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Text label;
    public Button startButton;
    private bool forceStart;

    void Start()
    {
        //forzar a cargar el Data instance:
        float vol = Data.Instance.musicVolume;
        Invoke("ResetLoadings", 6);
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
