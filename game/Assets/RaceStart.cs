using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStart : MonoBehaviour {

    public Text title1;
    public Text title2;

    public GameObject panel;

	void Start () {

        float totalLaps = Data.Instance.levels.GetCurrentLevelData().totalLaps;
        float totalTime = Data.Instance.levels.GetCurrentLevelData().totalTime;

        float gameOverTime = Data.Instance.levels.GetCurrentLevelData().gameOver;

        if (totalTime > 0)
        {
            title1.text = "COMPLETE " + Data.Instance.levels.GetCurrentLevelData().gameOver * 1000 + " MTS";
            title2.text = "IN LESS THAN " + totalTime + " SECONDS";
        }
        else if (totalLaps > 0)
        {
            title1.text = "COMPLETE " + Data.Instance.levels.GetCurrentLevelData().totalLaps * 1000 + " MTS";
            title2.text = "IN LESS THAN " + gameOverTime + " SECONDS";
        }

        panel.SetActive(true);
        
        Invoke("Ready", 3);
	}
    void Ready()
    {
        panel.SetActive(false);
        Events.OnRaceStartReady();
    }
}
