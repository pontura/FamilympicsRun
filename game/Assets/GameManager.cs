using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public bool ForceMultiplayer;

    [SerializeField]
    Lane lane;
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject container;
    [SerializeField]
    GameObject gameCanvas;
    [SerializeField]
    Chronometer chronometer;

    public List<Lane> lanes;
    public List<Player> players;
    public int totalPlayers;

    public List<GameObject> containers;

    private float speed = 0;
    private float targetSpeed;
    private float acceleration;

    public GameCamera gameCamera;

    private float LaneSeparation;
    private float scaleFactor;

    public states state;

    public enum states
    {
        IDLE,
        PLAYING,
        READY
    }

	void Start () {
        Events.OnMusicChange("crowds");

        Levels.LevelData levelData = Data.Instance.GetComponent<Levels>().GetCurrentLevelData();
        targetSpeed = levelData.speed;
        acceleration = levelData.acceleration;

        totalPlayers = 1;
        if (ForceMultiplayer || Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            totalPlayers = Data.Instance.multiplayerData.players.Count;

        LaneSeparation = Data.Instance.gameSettings.LaneSeparation;

        float offsetY = (totalPlayers-1)*(LaneSeparation/2);

        for (int a = 0; a < totalPlayers; a++)
        {
            int id = 1;

            if(ForceMultiplayer || Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
                id = Data.Instance.multiplayerData.players[a].playerID;

            GameObject newContainer = Instantiate(container) as GameObject;
            newContainer.GetComponent<GameCamera>().id = a + 1;
            newContainer.GetComponent<GameCamera>()._y = a * -LaneSeparation;
            containers.Add(newContainer);
            newContainer.transform.SetParent(gameCanvas.transform);
            Vector3 pos = new Vector3(0, offsetY, 0);
            newContainer.transform.localPosition = pos;
            newContainer.transform.localScale = Vector3.one;

            Lane newLane = Instantiate(lane) as Lane;
            newLane.transform.SetParent(newContainer.transform);
            lanes.Add(newLane);
            newLane.GetComponent<Transform>().localPosition = new Vector2(0, a * -LaneSeparation);
            newLane.Init(newContainer.GetComponent<GameCamera>());


            Player newPlayer = Instantiate(player) as Player;
            newPlayer.SetColor( Data.Instance.colors[id-1] );
            newPlayer.transform.SetParent(newContainer.transform);
            players.Add(newPlayer);
            newPlayer.GetComponent<Transform>().localPosition = new Vector2(-18f, a * -LaneSeparation);
            newPlayer.id = id;
            newPlayer.Init(newContainer.GetComponent<GameCamera>());
        }

        //scaleFactor = containers[0].GetComponentInParent<Canvas>().scaleFactor;
        Events.StartGame += StartGame;
        Events.OnAvatarDie += OnAvatarDie;
        Events.OnAvatarWinLap += OnAvatarWinLap;
        Events.OnTimeOver += OnTimeOver;
        Events.GameOver += GameOver;

        LaneSeparation /= scaleFactor;

        if(!ForceMultiplayer)
            Invoke("OnPowerUp", Random.Range(8,12));

	}
    public void Restart()
    {
        Data.Instance.Load("Game");
    }
    
    
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.OnAvatarDie -= OnAvatarDie;
        Events.OnAvatarWinLap -= OnAvatarWinLap;
        Events.OnTimeOver -= OnTimeOver;
        Events.GameOver -= GameOver;
    }
    void OnPowerUp()
    {
        if (state == states.READY) return;
        int powerUpActive = Random.Range(1, 4);
        Events.OnPowerUpOn(powerUpActive);
        Invoke("OnPowerUp", Random.Range(6,12));
    }
    void OnAvatarWinLap(int playerID, int count)
    {
        int totalLaps = Data.Instance.levels.GetCurrentLevelData().totalLaps;
        if (totalLaps != 0 && totalLaps == count)
            OnLapsOver();
    }
    void StartGame()
    {
        state = states.PLAYING;
    }
    int checkPoitionsNum = 0;
    void Update()
    {
        if (state == states.READY) return;
        if (state == states.IDLE) return;

        if (speed < targetSpeed) speed += acceleration;
        float realSpeed = speed*Time.deltaTime;

        foreach (GameObject container in containers)
            container.GetComponent<GameCamera>().Move(realSpeed);

        foreach (Lane lane in lanes)
            lane.UpdatePosition();

        int position = totalPlayers;
        foreach (Player player in players)
        {
            player.UpdatePosition(position);
            position--;
        }
        checkPoitionsNum++;
        if (checkPoitionsNum > 6)
        {
            ArrangePlayersByMeters();
            checkPoitionsNum = 0;
        }
    }
    void ArrangePlayersByMeters()
    {
        players = players.OrderBy(player => player.meters).ToList();
    }
    void OnLapsOver()
    {
        LevelComplete();
    }
    void GameOver(bool byTime)
    {
        state = states.READY;
    }
    void OnTimeOver()
    {
        float totalTime = Data.Instance.levels.GetCurrentLevelData().totalTime;
        float gameOverTime = Data.Instance.levels.GetCurrentLevelData().gameOver;
        bool somebodyWon = false;
        if (totalTime > 0)
        {
            foreach (Player player in players)
            {
                if (float.Parse(player.meters) > gameOverTime * 1000)
                    somebodyWon = true;
            }
            if (!somebodyWon)
            {
                Events.GameOver(false);
                return;
            }
        }
        LevelComplete();
    }
    void LevelComplete()
    {
        print("LevelComplete");
        Events.OnLevelComplete();
        Player winner = players[0];
        float playerPosition = 0;
        foreach (Player player in players)
        {
            if (int.Parse(player.meters) > playerPosition)
            {
                winner = player;
                playerPosition = int.Parse(player.meters);
            }
        }
        OnPlayerWin(winner);
    }
    void OnPlayerWin(Player player)
    {
        state = states.READY;
         float time = chronometer.timer;
         Data.Instance.levelData.SetResultValues(player.id, int.Parse(player.meters), time);
    }
    void OnAvatarDie(Player _player)
    {
        int playersDead = 0;
        foreach (Player player in players)
        {
            if (player.state == Player.states.DEAD)
                playersDead++;
        }
        if (playersDead == players.Count)
        {
            Levels.LevelData levelData = Data.Instance.levels.GetCurrentLevelData();
            if (levelData.Sudden_Death && chronometer.timer < levelData.gameOver)
                Events.GameOver(false);
            else
                LevelComplete();
        }
    }
}
