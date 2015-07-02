using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chronometer : MonoBehaviour {

    public Text label1;
    public Text label2;

    private bool timeStarted;
    public float timer;
    public string timerFormatted;
    public int levelSeconds;


	void Start () {
        Events.StartGame += StartGame;
        Events.OnLevelComplete += OnLevelComplete;
	}
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.OnLevelComplete -= OnLevelComplete;
    }
    void StartGame()
    {
        timeStarted = true;
        levelSeconds = Data.Instance.levels.GetCurrentLevelData().totalTime;
    }
    void OnLevelComplete()
    {
        timeStarted = false;
    }
    void Update()
    {
        if (timeStarted)
        {
            timer += Time.deltaTime;
        }     
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
      

        timerFormatted = string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
        label1.text = timerFormatted;
        label2.text = timerFormatted;

          if (levelSeconds >0)
          {
              if (timer >= levelSeconds)
              {
                  Events.OnTimeOver();
              }
          }
    }
}
