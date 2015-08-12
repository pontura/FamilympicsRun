﻿using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    public int id;
    private states state;
    public float distance = 0;
    private float powerUpMultiplierSpeed = 1;
    private float lapSpeedIncreased = 0;
    public float _y;
    

    private enum states
    {
        PLAYING,
        PAUSED,
        PAUSED_BY_AVATAR
    }

    void Start()
    {
        Events.OnPowerUpActive += OnPowerUpActive;
        Events.OnAddEnemy += OnAddEnemy;
    }
    void OnDestroy()
    {
        Events.OnPowerUpActive -= OnPowerUpActive;
        Events.OnAddEnemy -= OnAddEnemy;
    }
    void OnPowerUpActive(int _id, Powerups.types type)
    {
        if (_id == id) return;
        switch (type)
        {
            case Powerups.types.PAUSE:
                state = states.PAUSED;
                Invoke("Reset", 2);
                break;
            case Powerups.types.REWIND:
                powerUpMultiplierSpeed = 3;
                Invoke("Reset", 2);
                break;
        }
    }
    void Reset()
    {
        powerUpMultiplierSpeed = 1;
        state = states.PLAYING;
    }
    
    public void OnAvatarMoved()
    {
        if (state == states.PAUSED_BY_AVATAR)
            Invoke("Reset", 0.1f);
    }
    public void OnAvatarGotBorder()
    {
        if (state == states.PAUSED_BY_AVATAR) return;
        state = states.PAUSED_BY_AVATAR;
    }
    public void Move(float speed)
    {
        if (state == states.PAUSED || state == states.PAUSED_BY_AVATAR) return;

        distance += (speed * powerUpMultiplierSpeed) + lapSpeedIncreased;

        Vector3 pos = transform.localPosition;
        pos.x -= (speed * powerUpMultiplierSpeed) + lapSpeedIncreased;
        transform.localPosition = pos;

        if (!enemy) return;

        if (distance > lastEnemyAddedDistance)
        {
            lastEnemyAddedDistance += distanceToAddEnemy;
            AddEnemy();
        }
    }

    private int lastEnemyAddedDistance;
    private int distanceToAddEnemy;
    private Enemy enemy;
    void OnAddEnemy(Enemy enemy, int _distance)
    {
        lastEnemyAddedDistance = 0;
        distanceToAddEnemy = _distance;
        this.enemy = enemy;
    }
    void AddEnemy()
    {
        Enemy newEnemy = Instantiate(enemy) as Enemy;
        newEnemy.transform.SetParent (transform);        
        newEnemy.transform.localScale = Vector3.one;

        Vector3 pos = transform.localPosition;
        pos.x = distance + 30;
        pos.y = _y;
        
        newEnemy.Init(this, pos);
    }
    public void NewLap()
    {
        float lapAcceleration = Data.Instance.levels.GetCurrentLevelData().lapAcceleration;
        if (lapAcceleration > 0)
            lapSpeedIncreased += lapAcceleration;
    }
}
