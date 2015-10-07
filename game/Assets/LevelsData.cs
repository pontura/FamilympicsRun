using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class LevelsData : MonoBehaviour {

    public bool loadingScores;
    public int totalLevels;
    public int scoresLoaded;

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
        public List<ScoreData> scoreData;
        public float myScore;
        public float myScoreInParse;
    }

    public List<LevelsScore> levelsScore;
    private int i;
    
    public IList<string> myList;

    void Start()
    {
        myList = new List<string>();

        Events.OnFacebookLogin += OnFacebookLogin;
        Events.OnFacebookFriends += OnFacebookFriends;
        Events.OnParseLogin += OnParseLogin;
        Events.OnSaveScore += OnSaveScore;
        Events.OnRefreshHiscores += OnRefreshHiscores;
     //   Events.OnLoadParseScore += OnLoadParseScore;
        Events.OnLoadLocalData += OnLoadLocalData;
    }
    public void ResetLoadings()
    {
        loadingScores = false;
        scoresLoaded = 0;
    }
    void OnFacebookLogin()
    {
        //resetea tu score local para poder grabar nuevos scores:
        loadingScores = true;
        Reset();
    }
    public float GetMyScoreIfExists(int levelID)
    {
        if (Data.Instance.userData.facebookID == "")
            return PlayerPrefs.GetFloat("Run_Level_" + levelID);

        float score = 0;
        string facebookID = Data.Instance.userData.facebookID;
        Levels levels = Data.Instance.levels;
        levelsScore[levelID].myScore = PlayerPrefs.GetFloat("Run_Level_" + levelID);

        //chequea de los amigos que llegan si estas vos y tenias un myScore mejor te lo graba:
        foreach (ScoreData scoreData in levelsScore[levelID].scoreData)
        {
            if (scoreData.facebookID == facebookID)
            {
                levelsScore[levelID].myScoreInParse = scoreData.score;

                if (levelsScore[levelID].myScore == 0 || levelsScore[levelID].myScore == levelsScore[levelID].myScoreInParse)
                    levelsScore[levelID].myScore = levelsScore[levelID].myScoreInParse;
                else
                {
                    if (
                        levelsScore[levelID].myScore < levelsScore[levelID].myScoreInParse && levels.levels[levelID].Sudden_Death
                        ||
                        levelsScore[levelID].myScore < levelsScore[levelID].myScoreInParse && levels.levels[levelID].totalLaps > 0
                        ||
                        levelsScore[levelID].myScore > levelsScore[levelID].myScoreInParse && levels.levels[levelID].totalTime > 0
                        )
                    {
                        UpdateScore(levelID, levelsScore[levelID].myScore);
                        scoreData.score = levelsScore[levelID].myScore;                        
                        levelsScore[levelID].myScoreInParse = levelsScore[levelID].myScore;
                    }
                }
                return levelsScore[levelID].myScore;
            }
        }
        if (Data.Instance.userData.facebookID != "" && PlayerPrefs.GetFloat("Run_Level_" + levelID) > 0 && levelsScore[levelID].myScore != levelsScore[levelID].myScoreInParse)
        {
            levelsScore[levelID].myScore = PlayerPrefs.GetFloat("Run_Level_" + levelID);
            levelsScore[levelID].myScoreInParse = PlayerPrefs.GetFloat("Run_Level_" + levelID);
            SaveNewScore(levelID, levelsScore[levelID].myScore);
        }
        return score;
    }
    void OnLoadLocalData()
    {
        int a = 0;
        foreach(Levels.LevelData data in Data.Instance.levels.levels)
        {
            Data.Instance.levelsData.levelsScore[a].myScore = PlayerPrefs.GetFloat("Run_Level_" + a);
            a++;
        }
    }
    public void Reset()
    {
        print("__________________RESET HISCORES_____________________");
        foreach (LevelsData.LevelsScore levelScore in levelsScore)
        {
            levelScore.myScore = 0;
            levelScore.myScoreInParse = 0;
        }
    }
    int loadFriendsAndParseLogged = 0;
    public void OnParseLogin()
    {
        StartCoroutine("WaitUntilUserLoaded");
    }
    IEnumerator WaitUntilUserLoaded()
    {
        while (Data.Instance.userData.facebookID == "")
            yield return null; 

        print("________________________OnParseLogin::::::::::::::::::::::::");
        myList.Add(Data.Instance.userData.facebookID);
        loadFriendsAndParseLogged++;
        CheckIfBothAreReady();
    }
    public void OnFacebookFriends()
    {
        print("________________________OnFacebookFriends::::::::::::::::::::::::");
        
        foreach (UserData.FacebookUserData facebookUserData in Data.Instance.userData.FacebookFriends)
            myList.Add(facebookUserData.facebookID);

        loadFriendsAndParseLogged++;
        CheckIfBothAreReady();
    }
    void CheckIfBothAreReady()
    {
       // return;
        if (loadFriendsAndParseLogged < 2) return;

       // print("_________________________CheckIfBothAreReady");
        i = 0;
      // levelsScore = new LevelsScore[Data.Instance.levels.levels.Length];
        totalLevels = Data.Instance.levels.GetTotalLevelsInUnblockedSeasons();
       
       Invoke("LoadNextData", 1f);
    }
    public void Refresh()
    {
        OnFacebookFriends();
    }
    void OnLoadParseScore(int levelID)
    {
        //print("_______________________________OnLoadParseScore " + levelID);

        //levelsScore[levelID].myScoreInParse = 0;
        //var query = new ParseQuery<ParseObject>("Level_" + levelID)
        //      .WhereEqualTo("facebookID", Data.Instance.userData.facebookID);       

        //query.FindAsync().ContinueWith(t =>
        //{
        //    IEnumerable<ParseObject> results = t.Result;
        //    foreach (var result in results)
        //    {
        //        levelsScore[levelID].myScore = float.Parse(result["score"].ToString());
        //        levelsScore[levelID].myScoreInParse = float.Parse(result["score"].ToString());
        //    }
        //});
    }
    void OnRefreshHiscores()
    {
        if (levelsScore != null)
        {
            print("_________OnRefreshHiscores totalLevels: " + totalLevels);
            i = 0;
            Invoke("LoadNextData", 1);
        }
    }
    private void LoadNextData()
    {
        i++;
        if (i < totalLevels)
        {
            LoadData(i);
            Invoke("LoadNextData", 1);
        }
    }
    void OnSaveScore(int level, float score)
    {
        print("_________OnSaveScore" + level + " " + score + " myScoreInParse  " + levelsScore[level].myScoreInParse);
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

        Events.OnNewHiscore(level, score);
    }
    

    void UpdateScore(int level, float score)
    {
        print("UpdateScore" + level + " SCORE: " + score);
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
        ScoreData sd = new ScoreData();
        sd.facebookID = Data.Instance.userData.facebookID;
        sd.playerName = Data.Instance.userData.username;
        sd.score = score;
        levelsScore[level].scoreData.Add(sd);

        print("SaveNewScore" + level);
        ParseObject gameScore = new ParseObject("Level_" + level.ToString());
        //gameScore.Increment("score", hiscore);
        gameScore["playerName"] = Data.Instance.userData.username;
        gameScore["facebookID"] = Data.Instance.userData.facebookID;
        gameScore["score"] = score;
        //gameScore["objectId"] = ParseUser.CurrentUser.Username;
        gameScore.SaveAsync();
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
        
        print("level: " + _level + " SCORE: " + Data.Instance.levelsData.levelsScore[_level].myScore);

        levelsScore[_level].scoreData.Clear();

        ParseQuery<ParseObject> query;

        //print("LoadDATA: " + myList.Count);

       // print("_________________");
        foreach(string n in myList)
        {
            //print("n: " + n);
        }

        if (Data.Instance.levels.levels[_level].totalLaps > 0)
        {
            query = ParseObject.GetQuery("Level_" + _level.ToString())
            .OrderBy("score")
            .WhereContainedIn("facebookID", myList)
            .Limit(20);
        }
        else
        {
            query = ParseObject.GetQuery("Level_" + _level.ToString())
                .OrderByDescending("score")
                .WhereContainedIn("facebookID", myList)
                .Limit(20);
        }
        
            query.FindAsync().ContinueWith(t =>
            {
                IEnumerable<ParseObject> results = t.Result;
                int a = 1;
                foreach (var result in results)
                {
                    ScoreData sd = new ScoreData();
                   
                    sd.playerName = result["playerName"].ToString();
                    sd.facebookID = result["facebookID"].ToString();
                    sd.score = float.Parse(result["score"].ToString());
                    levelsScore[_level].scoreData.Add(sd);
                    //Events.OnParseLoadedScore(sd.facebookID, sd.score, _level);
                    scoresLoaded++;
                    a++;
                }
                
            });        
    }


    public string GetScoreString(int levelID, float score)
    {
        if (Data.Instance.levels.levels[levelID].Sudden_Death || Data.Instance.levels.levels[levelID].totalLaps > 0)
        {
            return GetTimer(score);
        }
        else
            return score.ToString() + "m";
    }
    public string GetTimer(float timer)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        return string.Format("{0:00}:{1:00}.{2:00}", t.Minutes, t.Seconds, t.Milliseconds/10);
    }
    int lastLevelHiscore;
    void RefreshLevelHiscore(int level, float score)
    {
        print("RefreshLevelHiscore + " + level + " score: " + score);
        lastLevelHiscore = level;
        Invoke("ReloadHiscores", 2);
    }
    void ReloadHiscores()
    {
        LoadData(lastLevelHiscore);
    }
    void AddNewRefreshedData(ScoreData scoreData, float score)
    {
        print("AddNewRefreshedData score: " + score);
        scoreData.score = score;
        scoreData.playerName = Data.Instance.userData.username;
        scoreData.facebookID = Data.Instance.userData.facebookID;
    }
    public void ArrengeListByScore(int levelId)
    {
        levelsScore[levelId].scoreData = levelsScore[levelId].scoreData.OrderBy(x => x.score).ToList();
        if (Data.Instance.levels.levels[levelId].totalTime > 0) levelsScore[levelId].scoreData.Reverse();
    }
}
