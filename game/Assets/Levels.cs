using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour {

    public int currentLevel;

    [Serializable]
    public class enemiesType
    {
        public bool HURDLES;
        public bool WIND;
        public int distance = 24;
    }

    [Serializable]
    public class LevelData
    {
        [SerializeField]
        public float speed = 0f;
        [SerializeField]
        public float acceleration = 0.001f;
        [SerializeField]
        public float lapAcceleration = 0;
        [SerializeField]
        public int totalLaps;
        [SerializeField]
        public int totalTime;
        [SerializeField]
        public bool Sudden_Death;
        [SerializeField]
        public enemiesType enemies;
        public float star1;
        public float star2;
        public float star3;
        public float gameOver;
    }

    public LevelData[] levels;

    public LevelData GetCurrentLevelData()
    {
        return levels[currentLevel];
    }
    public LevelData GetData(int id)
    {
        return levels[id];
    }
    public int GetCurrentLevelStarsByScore(int levelID, float score)
    {
        int stars = 0;
        LevelData level = levels[levelID];
       // print(" _______levelID: " + levelID + " GetCurrentLevelStarsByScore totalLaps: " + level.totalLaps + " star3: " + level.star3 + " score: " + score);

        if (level.totalLaps > 0)
        {
            if (score < level.star3)
                stars = 3;
            else if (score < level.star2)
                stars = 2;
            else if (score < level.star1)
                stars = 1;
        }
        else if (level.Sudden_Death ||  level.totalTime > 0)
        {
            if (score > level.star3)
                stars = 3;
            else if (score > level.star2)
                stars = 2;
            else if (score > level.star1)
                stars = 1;
        }
        return stars;
    }
    public int GetCurrentLevelStars(float time, int meters)
    {
        int stars = 0;
        LevelData level = GetCurrentLevelData();
        if (level.Sudden_Death)
        {
            if (time > level.star3)
                stars = 3;
            else if (time > level.star2)
                stars = 2;
            else if (time > level.star1)
                stars = 1;
        }
        if (level.totalLaps > 0)
        {
            if (time < level.star3)
                stars = 3;
            else if (time < level.star2)
                stars = 2;
            else if (time < level.star1)
                stars = 1;
        }
        else if ( level.totalTime > 0)
        {
           // print("METERS::::::::::::   " + meters + "    level.star3 * 1000: " + level.star3 * 1000);
            if (meters > level.star3 * 1000)
                stars = 3;
            else if (meters > level.star2 * 1000)
                stars = 2;
            else if (meters > level.star1 * 1000)
                stars = 1;
        }
        return stars;
    }

}
