using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

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

    public List<GameObject> containers;

    private float speed = 0;
    private float targetSpeed;
    private float acceleration;

    public GameCamera gameCamera;

    private float LaneSeparation = 2.5f;
    private float scaleFactor;

    public states state;

    public enum states
    {
        IDLE,
        PLAYING
    }

	void Start () {
        Levels.LevelData levelData = Data.Instance.GetComponent<Levels>().GetCurrentLevelData();
        targetSpeed = levelData.speed;
        acceleration = levelData.acceleration;

        int totalPlayers = 1;
        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            totalPlayers = Data.Instance.multiplayerData.players.Count;

        float offsetY = (totalPlayers-1)*(LaneSeparation/2);
        for (int a = 0; a < totalPlayers; a++)
        {
            int id = 1;
            if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
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

        LaneSeparation /= scaleFactor;

        Invoke("OnPowerUp", Random.Range(3,6));
	}
    
    
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.OnAvatarDie -= OnAvatarDie;
        Events.OnAvatarWinLap -= OnAvatarWinLap;
        Events.OnTimeOver -= OnTimeOver;
    }
    void OnPowerUp()
    {
        Events.OnPowerUpOn();
        Invoke("OnPowerUp", Random.Range(5, 12));
    }
    void OnAvatarWinLap(int playerID, int count)
    {
        if (Data.Instance.levels.GetCurrentLevelData().totalLaps == count)
            OnLapsOver();
    }
    void StartGame()
    {
        state = states.PLAYING;
    }
    void Update()
    {
        if (state == states.IDLE) return;

        if (speed < targetSpeed) speed += acceleration;
        float realSpeed = speed*Time.deltaTime;        

        foreach (GameObject container in containers)
            container.GetComponent<GameCamera>().Move(realSpeed);

        foreach (Lane lane in lanes)
            lane.UpdatePosition();

        foreach (Player player in players)
            player.UpdatePosition();
    }
    void OnLapsOver()
    {
        LevelComplete();
    }
    void OnTimeOver()
    {
        LevelComplete();
    }
    void LevelComplete()
    {
        Player winner = players[0];
        float playerPosition = 0;
        foreach (Player player in players)
        {
            if (player.transform.localPosition.x > playerPosition)
            {
                winner = player;
                playerPosition = player.transform.localPosition.x;
            }
        }
        OnPlayerWin(winner);
    }
    void OnPlayerWin(Player player)
    {
         float time = chronometer.timer;
         Data.Instance.levelData.SetResultValues(player.id, player.laps, time);
         if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            Data.Instance.Load("SummaryMultiplayer");
        else if (Data.Instance.levelData.challenge_facebookID == "")
            Data.Instance.Load("Summary");
        else
            Data.Instance.Load("SummaryChallenge");
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
            Data.Instance.Load("LevelSelector");
    }
}
