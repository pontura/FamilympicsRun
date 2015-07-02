using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int id;
    public states state;
    public float speed;
    private float initialAacceleration = 0.2f;
    private float initialDeceleration = 1.2f;

    private float maxSpeed = 2.2f;    
    private float speedJump = 1;

    private float acceleration;
    private float deceleration;
    public int laps = 0;

    private GameCamera gameCamera;

    public enum states
    {
        PLAYING,
        RUNNING,
        JUMPING,
        HURT,
        DEAD,
        READY
    }
    public void Init(GameCamera _gameCamera)
    {
        gameCamera = _gameCamera;
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;
        Events.OnPowerUpActive += OnPowerUpActive;
        Events.OnAvatarJump += OnAvatarJump;
        Events.OnAvatarRun += OnAvatarRun;
        Events.OnLevelComplete += OnLevelComplete;

    }
    void OnDestroy()
    {
        Events.OnPowerUpActive -= OnPowerUpActive;
        Events.OnAvatarJump -= OnAvatarJump;
        Events.OnAvatarRun -= OnAvatarRun;
        Events.OnLevelComplete -= OnLevelComplete;
    }
    void OnLevelComplete()
    {
        state = states.READY;
    }
    public void UpdatePosition()
    {
        if (state == states.READY) return;
        if (state != Player.states.DEAD)
        {
            float playerDistance = transform.localPosition.x;

            if (gameCamera.distance - playerDistance > 20)
                gameCamera.OnAvatarGotBorder();
            //Dead();
            else if (playerDistance - gameCamera.distance > 20)
                Win();
        }
    }
    void OnPowerUpActive(int _id, Powerups.types type)
    {
        if (state == states.READY) return;
        if (_id != id) return;
        switch (type)
        {
            case Powerups.types.FORWARD:
                animation.Play("playerOnForward");
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
        animation.Play("playerIdle");
    }
    public void Run()
    {
        if (state == states.READY) return;
        if (state == states.HURT) return;
        if (state == states.JUMPING) return;
        state = states.RUNNING;
        gameCamera.OnAvatarMoved();
        speed += acceleration;
        animation.Play("playerRun");
    }
    public void Jump()
    {
        if (state == states.READY) return;
        if (state == states.HURT) return;
        state = states.JUMPING;
        gameCamera.OnAvatarMoved();
        if (speed < speedJump) speed = speedJump;
        animation.Play("playerJump");
    }
    public void Hurt()
    {
        state = states.HURT;
        speed = 0;
        animation.Play("playerHurt");
    }

    //from animation
    public void EndJump()
    {
        Idle();
    }
    public void EndHurt()
    {
        Idle();
    }
    void Update()
    {
        if (state == states.READY) return;
        if (speed > maxSpeed) speed = maxSpeed;
        if (state == states.DEAD) return;
        if (state == states.PLAYING) return;
        if (state == states.HURT) return;

        if (speed == 0)
            state = states.PLAYING;

        if (state == states.RUNNING)
            speed -= deceleration*Time.deltaTime;
        else if (state == states.JUMPING)
            speed -= deceleration * Time.deltaTime;

        
        if (speed < 0) speed = 0;
        else if (speed > 0) 
        {
            Vector3 pos = transform.localPosition;
            pos.x += (speed*10) * Time.deltaTime;
            transform.localPosition = pos;
        }
    }
    public void Win()
    {
        laps++;
        Events.OnAvatarWinLap(id, laps);
        Vector3 pos = transform.localPosition;
        pos.x -= 40;
        transform.localPosition = pos;
    }
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy" && state != states.JUMPING)
        {
           // other.GetComponent<Enemy>().Die();
            Hurt();
        }
    }
    void SetOff()
    {
        Events.OnAvatarDie(this);
    }
}
