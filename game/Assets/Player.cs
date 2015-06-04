using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int id;
    public states state;
    public float speed;
    private float initialAacceleration = 3;
    private float initialDeceleration = 10;
    private float speedJump = 40;

    private float acceleration;
    private float deceleration;
    public int laps = 0;

    private GameCamera gameCamera;

    public enum states
    {
        PLAYING,
        RUNNING,
        JUMPING,
        DEAD
    }
    public void Init(GameCamera _gameCamera)
    {
        gameCamera = _gameCamera;
        acceleration = initialAacceleration;
        deceleration = initialDeceleration;
        Events.OnPowerUpActive += OnPowerUpActive;
    }
    void OnDestroy()
    {
        Events.OnPowerUpActive -= OnPowerUpActive;
    }
    public void UpdatePosition()
    {
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
    public void Idle()
    {
        speed = 0;
        state = states.PLAYING;
        animation.Play("playerIdle");
    }
    public void Run()
    {
        if (state == states.JUMPING) return;
        state = states.RUNNING;
        gameCamera.OnAvatarMoved();
        speed = acceleration;
        animation.Play("playerRun");
    }
    public void Jump()
    {
        state = states.JUMPING;
        gameCamera.OnAvatarMoved();
        speed = acceleration/3;
        animation.Play("playerJump");
    }
    //from animation
    public void EndJump()
    {
        Idle();
    }
    void Update()
    {
        if (state == states.DEAD) return;
        if (state == states.PLAYING) return;

        if (speed == 0)
            state = states.PLAYING;

        if (state == states.RUNNING)
            speed -= deceleration*Time.deltaTime;
        else if (state == states.JUMPING)
            speed = speedJump * Time.deltaTime;

        
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
    //public void Dead()
    //{
    //    if (laps == 0)
    //    {
    //        state = states.DEAD;
    //        print("Die");
    //        Invoke("SetOff", 0.1f);
    //    }
    //    else
    //    {
    //        laps--;
    //        Events.OnAvatarWinLap(id, laps);
    //        Vector3 pos = transform.localPosition;
    //        pos.x += 800;
    //        transform.localPosition = pos;
    //    }
    //}
    void SetOff()
    {
        Events.OnAvatarDie(this);
    }
}
