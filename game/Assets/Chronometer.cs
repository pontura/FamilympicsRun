using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chronometer : MonoBehaviour {

    public Text label1;
    public Text label2;

    private bool singlePlayerMode;
    private bool timeStarted;
    private bool timeReady;
    public float timer;
    public string timerFormatted;
    public int levelSeconds;


	void Start () {

        if (GameObject.Find("Game").GetComponent<GameManager>().ForceMultiplayer)
            singlePlayerMode = true;

        Events.StartGame += StartGame;
        Events.OnLevelComplete += OnLevelComplete;

        ActivateChronometers();

	}
    void ActivateChronometers()
    {
        MultiplayerData md = Data.Instance.multiplayerData;

        

        if (!singlePlayerMode)
        {
            label1.enabled = false;
            label2.enabled = false;

            foreach (MultiplayerData.PlayerData playerData in md.players)
            {
                if (playerData.playerID == 1 || playerData.playerID == 4) label1.enabled = true;
                if (playerData.playerID == 2 || playerData.playerID == 3) label2.enabled = true;
            }

        }
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
        if (timeReady) return;
        if (timeStarted)
            timer += Time.deltaTime;

        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
      

        timerFormatted = string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds/10);

        if (singlePlayerMode)
        {
            label1.text = timerFormatted;
        }
        else
        {
            if (label1.enabled)
                label1.text = timerFormatted;
            if (label2.enabled)
                label2.text = timerFormatted;
        }

          if (levelSeconds >0)
          {
              if (timer >= levelSeconds)
              {
                  Events.OnTimeOver();
                  timeReady = true;
              }
          }
    }
}
