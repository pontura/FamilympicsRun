using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class LevelsData : MonoBehaviour {

    [Serializable]
    public class ScoreData
    {
        //public string objectID;
        public string facebookID;
        public string playerName;
        public int score;
    }
    [Serializable]
    public class LevelsScore
    {
        public ScoreData scoreData1;
        public ScoreData scoreData2;
        public ScoreData scoreData3;
        public int myScore;
        public int mysCoreInParse;
    }

    public LevelsScore[] levelsScore;
    private int i = 0;
    private int totalLevels;

    public void Init()
    {

       levelsScore = new LevelsScore[Data.Instance.levels.levels.Length];
       totalLevels = Data.Instance.levels.levels.Length;
       Invoke("LoadNextData", 0.5f);
       Events.OnSaveScore += OnSaveScore;

       print("LEVELS! 1__: levels.Length: " + Data.Instance.levels.levels.Length + " levelsScore: " + levelsScore);
    }
    
    private void LoadNextData()
    {
        i++;
        if (i < totalLevels)
        {
            LoadData(i);
            Invoke("LoadNextData", 0.5f);
        }
    }
    public void OnSaveScore(int level, float score)
    {
        ParseObject gameScore = new ParseObject("Level_" + level.ToString() );

        //gameScore.Increment("score", hiscore);
        gameScore["playerName"] = Data.Instance.userData.username;
        gameScore["facebookID"] = Data.Instance.userData.facebookID;
        gameScore["score"] = score;
        //gameScore["objectId"] = ParseUser.CurrentUser.Username;

        gameScore.SaveAsync();

        print("SaveHighScore");


    }


    


    public LevelsScore GetLevelScores(int level)
    {
        try
        {
            return levelsScore[level];
        }
        catch
        {
            return null;
        }
    }

    private void LoadData(int _level)
    {
        print("LEVELS! 2");
        Debug.Log("LoadData" + _level);
        var query = ParseObject.GetQuery("Level_" + _level.ToString() )
          .OrderByDescending("score")
          .Limit(3);
        LevelsScore lscr = levelsScore[_level];

        if (lscr == null) print("OKKKKKKKKKKK");

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            int a = 1;
            foreach (var result in results)
            {
                ScoreData sd = levelsScore[_level].scoreData3;
                switch (a)
                {
                    case 1: sd = levelsScore[_level].scoreData1; break;
                    case 2: sd = levelsScore[_level].scoreData2; break;
                    default: sd = levelsScore[_level].scoreData3; break;
                }
                sd.playerName = result["playerName"].ToString();
                sd.facebookID = result["facebookID"].ToString();
                sd.score = int.Parse(result["score"].ToString());
                a++;
            }
        });
    }

}
