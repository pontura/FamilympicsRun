using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour {

    public int currentLevel;

    [Serializable]
    public class VerticalBar
    {
        public int _x;
        public int _y;
        public int speed;
        public int size;
    }


    [Serializable]
    public class enemiesType
    {
        public VerticalBar[] VERTICAL_BAR;
        public bool TRAMPOLIN;
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
    public int GetSeason(int levelID)
    {
        if (levelID < 9) return 1;
        else if (levelID < 17) return 2;
        else if (levelID < 25) return 3;
        else return 4;
    }
    public LevelData GetData(int id)
    {
        return levels[id];
    }
    public bool CanPlayNext()
    {
        bool result = true;
        int stars = Data.Instance.userData.starsCount;
        if (currentLevel == 8 && stars < Data.Instance.gameSettings.stars_for_tournament_2)
            result = false;
        else if (currentLevel == 16 && stars < Data.Instance.gameSettings.stars_for_tournament_3)
            result = false;
        else if (currentLevel == 32 && stars < Data.Instance.gameSettings.stars_for_tournament_4)
            result = false;
        return result;
    }
    public bool CanPlay(int levelID)
    {
        bool result = true;
        int stars = Data.Instance.userData.starsCount;
        if (levelID > 8 && stars < Data.Instance.gameSettings.stars_for_tournament_2)
            result = false;
        else if (levelID > 16 && stars < Data.Instance.gameSettings.stars_for_tournament_3)
            result = false;
        else if (levelID > 32 && stars < Data.Instance.gameSettings.stars_for_tournament_4)
            result = false;
        return result;
    }
    public int GetCurrentLevelStarsByScore(int levelID, float score)
    {
        if (score == 0) return 0;

        int stars = 0;
        LevelData level = levels[levelID];

        if (level.totalLaps > 0)
        {
            if (score < level.star3)
                stars = 3;
            else if (score < level.star2)
                stars = 2;
            else if (score < level.star1)
                stars = 1;
        }
        else if (level.Sudden_Death)
        {
            if (score > level.star3)
                stars = 3;
            else if (score > level.star2)
                stars = 2;
            else if (score > level.star1)
                stars = 1;
        }
        else if (level.totalTime > 0)
        {
            if (score > level.star3 * 1000)
                stars = 3;
            else if (score > level.star2 * 1000)
                stars = 2;
            else if (score > level.star1 * 1000)
                stars = 1;
        }

      //  print("stars: " + stars + "  score " + score + " level.totalLaps: " + level.totalLaps + " level.totalTime: " + level.totalTime + " level.suddenDeath: " + level.Sudden_Death);

        return stars;
    }
    public int GetCurrentLevelStars(float time, int meters)
    {
        LevelData level = GetCurrentLevelData();

        float score = time;
        if (level.Sudden_Death)
            score = time;
        if (level.totalLaps > 0)
            score = time;
        else if ( level.totalTime > 0)
            score = meters;
       
        return GetCurrentLevelStarsByScore(Data.Instance.levels.currentLevel, score);
    }
    public int GetTotalLevelsInUnblockedSeasons()
    {
        return 25;

        int levelProgressionId = Data.Instance.userData.levelProgressionId;
        int total = levels.Length;
        if (levelProgressionId < 8)
            return 10;
        else if (levelProgressionId < 16)
            return 18;
        else if (levelProgressionId < 32)
            return 34;
        return total;
    }

}
