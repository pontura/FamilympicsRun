using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStart : MonoBehaviour {

    public Text title1;
    public Text title2;

    public GameObject panel;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        float totalLaps = Data.Instance.levels.GetCurrentLevelData().totalLaps;
        float totalTime = Data.Instance.levels.GetCurrentLevelData().totalTime;
        float gameOver = Data.Instance.levels.GetCurrentLevelData().gameOver;
        bool Sudden_Death = Data.Instance.levels.GetCurrentLevelData().Sudden_Death;

        float gameOverTime = Data.Instance.levels.GetCurrentLevelData().gameOver;

        if (totalTime > 0)
        {
            title1.text = "YOU HAVE " + totalTime + " SECONDS";
            title2.text = "TO RUN AS FAR AS YOU CAN";
        }
        else if (totalLaps > 0)
        {
            title1.text = "COMPLETE " + Data.Instance.levels.GetCurrentLevelData().totalLaps * 1000 + " METERS";
            title2.text = "IN " + gameOverTime + " SECONDS OR LESS TO QUALIFY";
        }
        else if (Sudden_Death)
        {
            title1.text = "SUDDEN DEATH!";
            title2.text = "SURVIVE AT LEAST " + gameOver + " SECONDS";
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
