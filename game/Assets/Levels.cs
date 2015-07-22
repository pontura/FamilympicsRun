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
        public int totalLaps;
        [SerializeField]
        public int totalTime;
        [SerializeField]
        public enemiesType enemies;
        public float star1;
        public float star2;
        public float star3;
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
    public int GetCurrentLevelStars(float time, int meters)
    {
        int stars = 0;
        LevelData level = GetCurrentLevelData();
        if (level.totalLaps > 0)
        {
            if (time < level.star3)
                stars = 3;
            else if (time < level.star2)
                stars = 2;
            else if (time < level.star1)
                stars = 1;
        }
        else if (level.totalTime > 0)
        {
            if (meters > level.star3)
                stars = 3;
            else if (meters > level.star2)
                stars = 2;
            else if (meters > level.star1)
                stars = 1;
        }
        print("_____GetCurrentLevelStars - time: " + time + "- meters: " + meters + " stars: "  + stars);
        return stars;
    }

}
