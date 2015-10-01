using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int position;
    public int id;
    public string meters;
    public states state;
    public float speed;
    public float realDistance;
    private float initialAacceleration;
    private float initialDeceleration;
    public int totalPlayers;

    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;

    public SpriteRenderer numSprite;

    private float speedJump;

    private float acceleration;
    private float deceleration;
    private float decelerationJump;
    public int laps = 0;

    private float TrilRendererDefaultTime;
    private GameCamera gameCamera;
    private GameManager gameManager;

    public enum states
    {
        PLAYING,
        RUNNING,
        JUMPING,
        HURT,
        DEAD,
        IN_WIND_ZONE,
        STARTING_NEXT_LAP,
        READY        
    }
    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        TrilRendererDefaultTime = GetComponent<TrailRenderer>().time;
        totalPlayers = Data.Instance.multiplayerData.players.Count;

        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            totalPlayers = 1;
        numSprite.sprite = null;
    }
    public void Init(GameCamera _gameCamera)
    {
        GameSettings gameSettigns = Data.Instance.gameSettings;
        initialAacceleration = gameSettigns.player.initialAacceleration;
        initialDeceleration = gameSettigns.player.initialDeceleration;
        decelerationJump = gameSettigns.player.jumpDeceleration;
        speedJump = gameSettigns.player.speedJump;

        gameCamera = _gameCamera;
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;

        Events.OnPowerUpActive += OnPowerUpActive;
        Events.OnAvatarJump += OnAvatarJump;
        Events.OnAvatarRun += OnAvatarRun;
        Events.OnLevelComplete += OnLevelComplete;
        Events.GameOver += GameOver;

        

    }
    void OnDestroy()
    {
        Events.OnPowerUpActive -= OnPowerUpActive;
        Events.OnAvatarJump -= OnAvatarJump;
        Events.OnAvatarRun -= OnAvatarRun;
        Events.OnLevelComplete -= OnLevelComplete;
        Events.GameOver -= GameOver;
    }
    void GameOver(bool byTime)
    {
        OnLevelComplete();
    }
    void OnLevelComplete()
    {
        state = states.READY;
    }
    void StartGame()
    {
        state = states.PLAYING;
    }
    public void UpdatePosition(int position)
    {
        this.position = position;
        if (state == states.STARTING_NEXT_LAP) return;
        else if (state == states.READY) return;
        else if (state != Player.states.DEAD)
        {
            float playerDistance = transform.localPosition.x;
            float realDistance = gameCamera.distance - playerDistance;
            if (gameCamera.distance - playerDistance > 20)
            {
                Die();
            }
            else if (playerDistance - gameCamera.distance > 20)
                Win();
            else
            {
                float posX = (20 - realDistance) * 100 / 40;
                if (posX < 0) posX = 0; if (posX > 100) posX = 99;
                meters = ((int)(posX * 10)).ToString();
                if (meters.Length < 2) meters = "00" + meters;
                else if (meters.Length < 3) meters = "0" + meters;
                meters = laps + meters;

                if (state == states.PLAYING) return;

                if (totalPlayers <2) return;

                switch (position)
                {
                    case 1: numSprite.sprite = num1; break;
                    case 2: numSprite.sprite = num2; break;
                    case 3: numSprite.sprite = num3; break;
                    case 4: numSprite.sprite = num4; break;
                }
            }
        }
    }
    void OnPowerUpActive(int _id, Powerups.types type)
    {
        if (state == states.READY) return;
        else if (_id != id) return;
        switch (type)
        {
            case Powerups.types.FORWARD:
                GetComponent<Animation>().Play("playerOnForward");
                deceleration /= 2;
                acceleration = 12;
                speed = acceleration;
                Invoke("ResetPowerups", 2);
                break;
        }
    }
    void ResetPowerups()
    {
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;
        Idle();
    }
    public void SetColor(Color color)
    {
        GetComponentInChildren<SpriteRenderer>().color = color;
        GetComponentInChildren<TrailRenderer>().material.color = color;
    }
    void OnAvatarRun(int _id)
    {
        if(id != _id) return;
        Run();
    }
    void OnAvatarJump(int _id)
    {
        if (id != _id) return;
        Jump();
    }
    public void Idle()
    {
        speed = speed/2;
        state = states.PLAYING;
        GetComponent<Animation>().Play("playerIdle");
    }
    public void Run()
    {
        if (state == states.STARTING_NEXT_LAP) return;
        if (state == states.READY) return;
        if (state == states.HURT) return;
        if (state == states.JUMPING) return;

        if (state != states.IN_WIND_ZONE)
             state = states.RUNNING;

        gameCamera.OnAvatarMoved();
        speed += acceleration;
        GetComponent<Animation>().Play("playerRun");
    }
    public void Jump()
    {
        if (state == states.STARTING_NEXT_LAP) return;
        if (state == states.READY) return;
        if (state == states.HURT) return;
        if (state == states.JUMPING) return;

        Events.OnSoundFX("jump");

       // if (state != states.IN_WIND_ZONE)
        state = states.JUMPING;

        gameCamera.OnAvatarMoved();
        if (speed < speedJump) speed = speedJump;
        GetComponent<Animation>().Play("playerJump");
    }
    public void Hurt()
    {
        if (state == states.STARTING_NEXT_LAP) return;

        Vector3 pos = transform.localPosition;
        pos.x += 1;
        transform.localPosition = pos;

        Events.OnSoundFX("hurdleFall");
        state = states.HURT;
        speed = 0;
        GetComponent<Animation>().Play("playerHurt");
    }
    public void OnEnterWindZone()
    {
        state = states.IN_WIND_ZONE;
    }
    public void OnExitWindZone()
    {
        state = states.RUNNING;
        speed /= 2;
    }

    //from animation
    public void EndJump()
    {
       // Idle();
        state = states.PLAYING;
        GetComponent<Animation>().Play("playerIdle");
    }
    public void EndHurt()
    {
        Idle();
    }
    void Update()
    {
        if (state == states.STARTING_NEXT_LAP) return;
        if (state == states.READY) return;
      //  if (speed > maxSpeed) speed = maxSpeed;
        if (state == states.DEAD) return;
        if (state == states.PLAYING) return;
        if (state == states.HURT) return;

        if (speed == 0)
            state = states.PLAYING;

        if (state == states.RUNNING)
            speed -= (speed/1.7f) * deceleration * Time.deltaTime;
        else if (state == states.JUMPING)
            speed -= (speed/4) * decelerationJump * Time.deltaTime;

        
        
        if (speed < 0) speed = 0;
        else if (speed > 0) 
        {
            Vector3 pos = transform.localPosition;

            if (state == states.IN_WIND_ZONE)
                pos.x += (speed * 4) * Time.deltaTime;
            else
                pos.x += (speed * 10) * Time.deltaTime;

            transform.localPosition = pos;
        }
    }
    void Die()
    {
        if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death)
        {
            if (laps == 0)
            {
                SetOff();
                return;
            }
            laps--;
            state = states.STARTING_NEXT_LAP;
            meters = laps + "000";
            Events.OnAvatarWinLap(id, laps);
            Invoke("PrevLap", 0.05f);
            Vector3 pos = transform.localPosition;
            pos.x -= 1;
            transform.localPosition = pos;
            GetComponent<TrailRenderer>().time = -1;
        }
        else
        {
            gameCamera.OnAvatarGotBorder();
        }
    }
    void PrevLap()
    {
        if (gameManager.state == GameManager.states.READY) return;
        if (state == states.READY) return;

        GetComponent<TrailRenderer>().time = TrilRendererDefaultTime;
        state = states.RUNNING;
        Vector3 pos = transform.localPosition;
        pos.x += 40;
        transform.localPosition = pos;
    }
    private states lastState;
    public void Win()
    {
        laps++;
        gameCamera.NewLap();
        lastState = state;
        state = states.STARTING_NEXT_LAP;
        meters = laps + "000";
        Events.OnAvatarWinLap(id, laps);        
        Invoke("NextLap", 0.02f);
        Vector3 pos = transform.localPosition;
        pos.x += 1;
        transform.localPosition = pos;
        GetComponent<TrailRenderer>().time = -1;
    }
    void NextLap()
    {
        if (gameManager.state == GameManager.states.READY) return;
        if (state == states.READY) return;

        GetComponent<TrailRenderer>().time = TrilRendererDefaultTime;
        state = lastState;
        Vector3 pos = transform.localPosition;
        pos.x -= 40;
        transform.localPosition = pos;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (state == states.STARTING_NEXT_LAP) return;
        if (state == states.JUMPING) return;
        if (other.tag == "enemy")
        {
            if(other.GetComponent<Hurdle>())
                Hurt();
            else if (other.GetComponent<Wind>())
                OnEnterWindZone();

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (state == states.STARTING_NEXT_LAP) return;
        if (state == states.JUMPING) return;

        if (other.GetComponent<Wind>())
            OnExitWindZone();
        
    }
    
    void SetOff()
    {
        state = states.DEAD;
        Events.OnAvatarDie(this);
    }
}
