using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int id;
    public states state;
    private float speed;
    private float acceleration = 9;
    private float deceleration = 50;
    public bool onPreviousLap;
    public int laps = 0;

    public enum states
    {
        PLAYING,
        DEAD
    }
    public void SetColor(Color color)
    {
        GetComponentInChildren<Image>().color = color;
    }
    public void Run()
    {
        speed = acceleration;
    }
    void Update()
    {
        if (state == states.DEAD) return;
        speed -= deceleration*Time.deltaTime;
        if (speed < 0) speed = 0;
        if (speed > 0)
        {
            Vector3 pos = transform.localPosition;
            pos.x += speed;
            transform.localPosition = pos;
        }
    }
    public void Win()
    {
        if (!onPreviousLap)
        {
            laps++;
            Events.OnAvatarWinLap(id, laps);
        }
        onPreviousLap = false;
        Vector3 pos = transform.localPosition;
        pos.x -= 750;
        transform.localPosition = pos;
    }
    public void Dead()
    {
        if (onPreviousLap)
        {
            state = states.DEAD;
            print("Die");
            Invoke("SetOff", 0.1f);
        }
        else
        {
            onPreviousLap = true;
            Vector3 pos = transform.localPosition;
            pos.x += 800;
            transform.localPosition = pos;
        }

    }
    void SetOff()
    {
        Events.OnAvatarDie(this);
    }
}
