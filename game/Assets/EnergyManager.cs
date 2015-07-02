﻿using UnityEngine;
using System.Collections;
using System;

public class EnergyManager : MonoBehaviour {

    public int ENERGY_TO_START = 10;
    public int MINUTES_TO_ENERGY = 20;
    public int MAX_ENERGY;
    public int energy;
    public string timerString;

    public int diffTimestamp;
    public int lasTimeStamp;
    public int nowTimeStamp;


    private string playerPref_TIME = "energyTimestamp";
    private string playerPref_ENERGY = "energy";

    private float SECONDS_TO_ENERGY;
    


    void Start () {

        Events.StartGame += StartGame;
        SECONDS_TO_ENERGY = MINUTES_TO_ENERGY * 60;
        lasTimeStamp = PlayerPrefs.GetInt(playerPref_TIME);

        if(PlayerPrefs.HasKey(playerPref_ENERGY))
            energy = PlayerPrefs.GetInt(playerPref_ENERGY);
        else 
        {
            energy = ENERGY_TO_START;
            SaveEnergy();
        }

        Loop();
	}

    void StartGame()
    {
        if (energy == 0) return;
        energy--;
        SaveEnergy();
        SaveNewTime();
    }
    void Loop()
    {
        if (energy < MAX_ENERGY)
        {
            var epochStart = new System.DateTime(2010, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            nowTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            diffTimestamp = nowTimeStamp - lasTimeStamp;

            System.TimeSpan t = System.TimeSpan.FromSeconds(SECONDS_TO_ENERGY - diffTimestamp);

            timerString = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);

            Invoke("Loop", 1);

            if (diffTimestamp >= SECONDS_TO_ENERGY)
            {
                float energyToWin = Mathf.Floor(diffTimestamp / SECONDS_TO_ENERGY);

                energy += (int)energyToWin;

                if (energy >= MAX_ENERGY)
                {
                    energy = MAX_ENERGY;
                    diffTimestamp = 0;
                }

                SaveEnergy();
                SaveNewTime();
            }
        }
    }
    void SaveEnergy()
    {
        PlayerPrefs.SetInt(playerPref_ENERGY, energy);
        Events.OnEnergyWon();
    }
    void SaveNewTime()
    {
        var epochStart = new System.DateTime(2010, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        lasTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        PlayerPrefs.SetInt(playerPref_TIME, lasTimeStamp);
    }
}
