using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class LevelsData : MonoBehaviour {

    [Serializable]
    public class ScoreData
    {
        //public string objectID;
        public string facebookID;
        public string playerName;
        public float score;
    }
    [Serializable]
    public class LevelsScore
    {
        public ScoreData scoreData1;
        public ScoreData scoreData2;
        public ScoreData scoreData3;
        public float myScore;
        public float myScoreInParse;
    }

    public LevelsScore[] levelsScore;
    private int i;
    private int totalLevels;

    void Start()
    {
        Events.OnFacebookFriends += OnFacebookFriends;
        Events.OnSaveScore += OnSaveScore;
        Events.OnRefreshHiscores += OnRefreshHiscores;
        Events.OnLoadParseScore += OnLoadParseScore;
    }
    public void OnFacebookFriends()
    {
        i = 0;
      // levelsScore = new LevelsScore[Data.Instance.levels.levels.Length];
       totalLevels = Data.Instance.levels.levels.Length;
       
       Invoke("LoadNextData", 0.5f);
    }
    public void Refresh()
    {
        OnFacebookFriends();
    }
    void OnLoadParseScore(int levelID)
    {
        levelsScore[levelID].myScoreInParse = 0;
        var query = new ParseQuery<ParseObject>("Level_" + levelID)
              .WhereEqualTo("facebookID", Data.Instance.userData.facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                levelsScore[levelID].myScoreInParse =  float.Parse(result["score"].ToString());
            }
        });
    }
    void OnRefreshHiscores()
    {
        print("OnRefreshHiscores");
        if (levelsScore != null && levelsScore[0] != null && levelsScore[0].scoreData1 != null && levelsScore[0].scoreData1.score < 0)
        {
            i = 0;
            Invoke("LoadNextData", 0.5f);
        }
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
    void OnSaveScore(int level, float score)
    {
        print("_________OnSaveScore" + level + " " + score);
        // si es por laps entonces el tiempo tiene que ser menor para grabar el score
        if (Data.Instance.levels.levels[level].totalLaps > 0 && score > levelsScore[level].myScore && levelsScore[level].myScore != 0) { Debug.Log("Ya tenias menos tiempo"); return; }
        if (Data.Instance.levels.levels[level].totalTime > 0 && score < levelsScore[level].myScore) { Debug.Log("Ya hbaias recorrido mas distancia");   return; }

        PlayerPrefs.SetFloat("Run_Level_" + level, score);

        levelsScore[level].myScore = score;

        if (Data.Instance.userData.facebookID == "") return;

        if (levelsScore[level].myScoreInParse == 0)
            SaveNewScore(level, score);
        else
            UpdateScore(level, score);

        RefreshLevelHiscore(level, score);
        
        levelsScore[level].myScoreInParse = score;
        levelsScore[level].myScore = score;
    }
    

    void UpdateScore(int level, float score)
    {
        print("UpdateScore");
        var query = new ParseQuery<ParseObject>("Level_" + level)
            .WhereEqualTo("facebookID", Data.Instance.userData.facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["score"] = score;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Score updated!");
        });        

    }
    void SaveNewScore(int level, float score)
    {
        print("SaveNewScore");
        ParseObject gameScore = new ParseObject("Level_" + level.ToString());
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
        Data.Instance.levelsData.levelsScore[_level].myScore = PlayerPrefs.GetFloat("Run_Level_" + _level);
        Debug.Log("LoadData" + _level);

        ParseQuery<ParseObject> query;


        IList<string> myList = new List<string>();
        foreach (UserData.FacebookUserData facebookUserData in Data.Instance.userData.FacebookFriends)
        {
            myList.Add(facebookUserData.facebookID);
        }
        myList.Add(Data.Instance.userData.facebookID);

        if (Data.Instance.levels.levels[_level].totalLaps > 0)
        {
            query = ParseObject.GetQuery("Level_" + _level.ToString())
            .OrderBy("score")
            .WhereContainedIn("facebookID", myList)
            .Limit(3);
        }
        else
        {
            query = ParseObject.GetQuery("Level_" + _level.ToString())
                .OrderByDescending("score")
                .WhereContainedIn("facebookID", myList)
                .Limit(3);
        }
        
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
                    sd.score = float.Parse(result["score"].ToString());
                    a++;
                }
            });        
    }
    public string GetScoreString(int levelID, float score)
    {
        if (Data.Instance.levels.levels[levelID].totalLaps > 0)
            return GetTimer(score);
        else
            return score.ToString();
    }
    private string GetTimer(float timer)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        return string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
    }
    void RefreshLevelHiscore(int level, float score)
    {
        print("_____RefreshLevelHiscore ");
        LevelsScore thisLevelScore = levelsScore[level];

        int newRecord = 0;

        if (Data.Instance.levels.levels[level].totalLaps > 0)
        {
            if (thisLevelScore.scoreData1.score > score)
                newRecord = 1;
            else if (thisLevelScore.scoreData2.score > score)
                newRecord = 2;
            else if (thisLevelScore.scoreData3.score > score)
                newRecord = 3;
        }
        else if (Data.Instance.levels.levels[level].totalTime > 0)
        {
            if (thisLevelScore.scoreData1.score < score)
                newRecord = 1;
            else if (thisLevelScore.scoreData2.score < score)
                newRecord = 2;
            else if (thisLevelScore.scoreData3.score < score)
                newRecord = 3;
        }

        if (newRecord == 1)
        {
            if (thisLevelScore.scoreData2.playerName != Data.Instance.userData.username)
            {
                thisLevelScore.scoreData2.facebookID = thisLevelScore.scoreData1.facebookID;
                thisLevelScore.scoreData2.score = thisLevelScore.scoreData1.score;
                thisLevelScore.scoreData2.playerName = thisLevelScore.scoreData1.playerName;
            }
            AddNewRefreshedData(thisLevelScore.scoreData1, score);
        }
        if (newRecord == 2)
        {
            if (thisLevelScore.scoreData3.playerName != Data.Instance.userData.username)
            {
                thisLevelScore.scoreData3.facebookID = thisLevelScore.scoreData2.facebookID;
                thisLevelScore.scoreData3.score = thisLevelScore.scoreData2.score;
                thisLevelScore.scoreData3.playerName = thisLevelScore.scoreData2.playerName;
            }
            AddNewRefreshedData(thisLevelScore.scoreData2, score);
        }
        if (newRecord == 3)
        {
            AddNewRefreshedData(thisLevelScore.scoreData3, score);
        }
    }
    void AddNewRefreshedData(ScoreData scoreData, float score)
    {
        print("AddNewRefreshedData score: " + score);
        scoreData.score = score;
        scoreData.playerName = Data.Instance.userData.username;
        scoreData.facebookID = Data.Instance.userData.facebookID;
    }
}
