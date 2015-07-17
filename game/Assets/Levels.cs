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

}
