using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Lane lane;
    [SerializeField]
    Player player;

    public List<Lane> lanes;
    public List<Player> players;

    public GameObject lanesContainer;

    public float speed;
    public float distance;

    public GameCamera gameCamera;

    public int LaneSeparation = 50;

    public states state;
    public enum states
    {
        IDLE,
        PLAYING
    }

	void Start () {
        Events.StartGame += StartGame;
        Events.OnAvatarRun += OnAvatarRun;
        Events.OnAvatarDie += OnAvatarDie;

        lanesContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ((Data.Instance.numPlayers - 1) * LaneSeparation) /2);
        for (int a = 0; a < Data.Instance.numPlayers; a++)
        {
            Lane newLane = Instantiate(lane) as Lane;
            newLane.transform.SetParent(lanesContainer.transform);
            lanes.Add(newLane);
            newLane.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, a * -LaneSeparation);


            Player newPlayer = Instantiate(player) as Player;
            newPlayer.SetColor(Data.Instance.colors[a]);
            newPlayer.transform.SetParent(lanesContainer.transform);
            players.Add(newPlayer);
            newPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, a * -LaneSeparation);
            newPlayer.id = a + 1;
        }
        Invoke("OnPowerUp", Random.Range(3,6));
	}
    void OnPowerUp()
    {
        Events.OnPowerUpOn();
        Invoke("OnPowerUp", Random.Range(5, 12));
    }
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.OnAvatarRun -= OnAvatarRun;
        Events.OnAvatarDie -= OnAvatarDie;
    }
    void StartGame()
    {
        state = states.PLAYING;
    }
    void OnAvatarRun(int id)
    {
        if (state == states.IDLE) return;
        players[id-1].Run();
    }
    void Update()
    {
        if (state == states.IDLE) return;
        float realSpeed = (speed*100)*Time.deltaTime;
        distance += realSpeed;
        gameCamera.Move(realSpeed);

        foreach (Lane lane in lanes)
        {
            lane.UpdatePosition(distance);
        }
        foreach (Player player in players)
        {
            if (player.state != Player.states.DEAD)
            {
                float playerDistance = player.transform.localPosition.x;

                if(player.id == 1)
                 print(playerDistance - distance); 

                if (distance - playerDistance > 400)
                    player.Dead();
                else if (playerDistance - distance  > 400)
                    player.Win();
            }
        }
    }
    void OnPlayerWin(int id)
    {
        Data.Instance.winnerID = id;
        Application.LoadLevel("Results");
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
            Application.LoadLevel("MainMenu");
    }
}
