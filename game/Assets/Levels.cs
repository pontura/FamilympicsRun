using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour {

    public int currentLevel;

    [Serializable]
    public class LevelData
    {
        [SerializeField]
        public float scrollSpeed;
        [SerializeField]
        public float speed = 0;
        [SerializeField]
        public float targetSpeed = 0f;
        [SerializeField]
        public float acceleration = 0.001f;
        [SerializeField]
        public int totalLaps;
    }

    public LevelData[] levels;

    public LevelData GetCurrentLevelData()
    {
        return levels[currentLevel - 1];
    }

}
