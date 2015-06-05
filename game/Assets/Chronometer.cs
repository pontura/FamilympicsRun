using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chronometer : MonoBehaviour {

    private Text label;
    private bool timeStarted;
    public float timer;
    public string timerFormatted;
    public int levelSeconds;


	void Start () {
        label = GetComponent<Text>();
        Events.StartGame += StartGame;
	}
    
    void StartGame()
    {
        timeStarted = true;
        levelSeconds = Data.Instance.levels.GetCurrentLevelData().totalTime;
    }
    void Update()
    {
        if (timeStarted)
        {
            timer += Time.deltaTime;
        }     
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
      

        timerFormatted = string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
        label.text = timerFormatted;

          if (levelSeconds >0)
          {
              if (timer >= levelSeconds)
              {
                  Events.OnTimeOver();
              }
          }
    }
}
