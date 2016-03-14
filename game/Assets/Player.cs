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
    public int laps = 0;
    
    
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;

    public SpriteRenderer numSprite;

    private float speedJump;

    private float acceleration;
    private float deceleration;
    private float decelerationJump;
    private float initialAacceleration;
    private float initialDeceleration;
    private bool isMultiplayer;
    private float wind_stops;

    private float TrilRendererDefaultTime;
    private GameCamera gameCamera;
    private GameManager gameManager;

    private float PU_BOOST_Acceleration;
    private float PU_BOOST_duration;
    private float PU_PAUSED_duration;
    private Color color;
    private bool inTRampolinZone;

    private float timeTeResetAction;

    public enum states
    {
        PLAYING,
        RUNNING,
        JUMPING,
        HURT,
        DEAD,
        IN_WIND_ZONE,
        PAUSED,
        READY        
    }
    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        TrilRendererDefaultTime = GetComponent<TrailRenderer>().time;
        numSprite.sprite = null;
    }
    public void Init(GameCamera _gameCamera)
    {
        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            isMultiplayer = true;

        wind_stops = Data.Instance.gameSettings.wind_stops;
        PU_BOOST_Acceleration = Data.Instance.gameSettings.PU_BOOST_Acceleration;
        PU_BOOST_duration = Data.Instance.gameSettings.PU_BOOST_duration;
        PU_PAUSED_duration = Data.Instance.gameSettings.PU_PAUSED_duration;

        GameSettings gameSettigns = Data.Instance.gameSettings;
        initialAacceleration = gameSettigns.player.initialAacceleration;
        initialDeceleration = gameSettigns.player.initialDeceleration;
        decelerationJump = gameSettigns.player.jumpDeceleration;
        speedJump = gameSettigns.player.speedJump;

        gameCamera = _gameCamera;
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;

        if (isMultiplayer)
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
    
    void OnPowerUpActive(int _id, Powerups.types type)
    {
        if (state == states.PAUSED) return;
        if (state == states.READY) return;
        switch (type)
        {
            case Powerups.types.BOOST:
                if (_id != id) return;
                acceleration *= PU_BOOST_Acceleration;
                speed = acceleration;
                Invoke("ResetPowerups", PU_BOOST_duration);
                Run();
                break;
            case Powerups.types.PAUSE_ME:
                if (_id != id) return;
                OnPaused();                
                break;
            case Powerups.types.PAUSE_OTHERS:
                if (_id == id) return;
                OnPaused(); 
                break;
        }
    }
    void OnPaused()
    {
        print("OnPaused");

        if (acceleration > initialAacceleration)
            acceleration /= 2;
        if (acceleration < initialAacceleration) acceleration = initialAacceleration;

        state = states.PAUSED;
        Invoke("ResetPaused", PU_PAUSED_duration);
    }
    void ResetPaused()
    {
        print("OnPaused RESET");
        Idle();
    }
    void ResetPowerups()
    {
        print("ResetPowerups");
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;
        GetComponent<Animation>().Play("playerIdle");
    }
    
    public void SetColor(Color color)
    {
        this.color = color;
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
       // if (state == states.STARTING_NEXT_LAP) return;
        speed = speed/2;
        state = states.RUNNING;
       // state = states.PLAYING;
        GetComponent<Animation>().Play("playerIdle");
    }
    public void Run()
    {
        if (state == states.PAUSED) return;
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
        if (state == states.PAUSED) return;
        if (state == states.READY) return;
        if (state == states.HURT) return;
        if (state == states.JUMPING) return;

        Events.OnSoundFX("jump");

       // if (state != states.IN_WIND_ZONE)
        state = states.JUMPING;

        gameCamera.OnAvatarMoved();
        if (speed < speedJump) speed = speedJump;

        if (inTRampolinZone) speed *= 2;

        GetComponent<Animation>().Play("playerJump");
        //Invoke("EndJump", 0.7f);
        timeTeResetAction = 0.7f;
    }
    public void Hurt()
    {
        //Vector3 pos = transform.localPosition;
        //pos.x += 1;
        //transform.localPosition = pos;

        Events.OnSoundFX("hurdleFall");
        state = states.HURT;
        speed = 0;
        GetComponent<Animation>().Play("playerHurt");
       // Invoke("EndHurt", 0.4f);
        timeTeResetAction = 0.4f;
    }
    public void OnEnterWindZone()
    {
        state = states.IN_WIND_ZONE;
    }
    public void OnExitWindZone()
    {
        state = states.RUNNING;
    }

    public void OnEnterTrampolinZone()
    {
        inTRampolinZone = true;
    }
    public void OnExitTrampolinZone()
    {
        inTRampolinZone = false;
        // speed /= 2;
    }

    //from animation
    //public void EndJump()
    //{
    //   // if (state == states.STARTING_NEXT_LAP) return;
    //    if (gameManager.state == GameManager.states.READY) return;
    //    Idle();
    //}
    //public void EndHurt()
    //{
    //    //if (state == states.STARTING_NEXT_LAP) return;
    //    if (gameManager.state == GameManager.states.READY) return;
    //    Idle();
    //}
    void Update()
    {
        if (timeTeResetAction > 0)
        {
            timeTeResetAction -= Time.deltaTime;
        }
        else
        {
            if (state == states.JUMPING || state == states.HURT)
            {
                if (gameManager.state == GameManager.states.READY) return;
                state = states.RUNNING;
                GetComponent<Animation>().Play("playerIdle");
            }
        }

        if (state == states.READY) return;
        if (state == states.DEAD) return;
        if (state == states.PLAYING) return;
        if (state == states.HURT) return;

        if (speed == 0)
            state = states.PLAYING;

      
        if (state == states.RUNNING)
            speed -= (speed/1.7f) * deceleration * Time.deltaTime;
        else if (state == states.PAUSED)
            speed -= speed * deceleration * Time.deltaTime;
        else if (state == states.JUMPING)
            speed -= (speed/4) * decelerationJump * Time.deltaTime;

        //desacelera el powerup BOOST:
        if (isMultiplayer)
        {
            if (acceleration > initialAacceleration)
                acceleration -= 0.015f;
            if (acceleration < initialAacceleration) acceleration = initialAacceleration;
        }
        //if (id == 1)
        //{
        //    print(  "acceleration: " + acceleration + "initialAacceleration: " + initialAacceleration + "  deceleration: " + deceleration + "SPEED: " + speed + "   state: " + state );
        //}
        
        if (speed < 0) speed = 0;
        else if (speed > 0) 
        {
            Vector3 pos = transform.localPosition;

            if (state == states.IN_WIND_ZONE)
            {
                if (speed > 1) speed /= wind_stops;
            }
            pos.x += (speed * 10) * Time.deltaTime;

            transform.localPosition = pos;
        }
    }
    public void UpdatePosition(int position)
    {
        this.position = position;
        if (state == states.READY) return;
        else if (state != Player.states.DEAD)
        {
            float playerDistance = transform.localPosition.x;
            float realDistance = gameCamera.distance - playerDistance;
            if (gameCamera.distance - playerDistance > 20)
            {
                Die();
            }
            if (playerDistance - gameCamera.distance > 20)
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

                if (!isMultiplayer) return;

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
            //state = states.STARTING_NEXT_LAP;
            meters = laps + "000";
            Events.OnAvatarWinLap(id, laps);
           // Invoke("PrevLap", 0.05f);
            PrevLap();
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
    private float lastXPosition;
    public void Win()
    {
        laps++;
        gameCamera.NewLap();
        Events.OnFlashWinLap(color);

        OnExitTrampolinZone();

        if (state == states.JUMPING)
            lastState = state;
        else
            lastState = states.RUNNING;

        meters = laps + "000";
        Events.OnAvatarWinLap(id, laps);        
        Invoke("NextLap", 0.02f);
        Vector3 pos = transform.localPosition;
        pos.x += 1;
        transform.localPosition = pos;
        lastXPosition = pos.x;
        GetComponent<TrailRenderer>().time = -1;
    }
    void NextLap()
    {
        if (gameManager.state == GameManager.states.READY || state == states.READY)
        {            
            Idle();
            state = states.READY;
            return;
        }

        GetComponent<TrailRenderer>().time = TrilRendererDefaultTime;
        if (lastState != states.JUMPING)
            state = states.RUNNING;

        Vector3 pos = transform.localPosition;
        pos.x = lastXPosition - 40;
        transform.localPosition = pos;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<VerticalEnemy>())
                Hurt();

        if (state == states.JUMPING) return;
        if (other.tag == "enemy")
        {
            if (other.GetComponent<Trampolin>())
                OnEnterTrampolinZone();
            else if(other.GetComponent<Hurdle>())
                Hurt();
            else if (other.GetComponent<Wind>())
                OnEnterWindZone();

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (state == states.JUMPING) return;

        if (other.GetComponent<Wind>())
            OnExitWindZone();
        else if (other.GetComponent<Trampolin>())
            OnExitTrampolinZone();        
    }
    
    void SetOff()
    {
        state = states.DEAD;
        Events.OnAvatarDie(this);
    }
}
