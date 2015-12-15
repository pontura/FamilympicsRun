using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class ChallengersManager : MonoBehaviour {

    public bool showFacebookFriends;

    public List<PlayerData> received;
    public List<PlayerData> made;

    [Serializable]
    public class PlayerData
    {
        public string objectID;
        public string facebookID;
        public string playerName;
        public float score;
        public int level;

        public float score2;
        public string winner;
    }
    void Start()
    {
        Events.OnChallengesLoad += OnChallengesLoad;
        Events.OnChallengeCreate += OnChallengeCreate;
        Events.OnChallengeRemind += OnChallengeRemind;
        Events.OnChallengeClose += OnChallengeClose;
        Events.OnChallengeDelete += OnChallengeDelete;
        Events.OnResetApp += OnResetApp;
    }
    void OnResetApp()
    {
        received.Clear();
        made.Clear();
    }
    void OnChallengesLoad()
    {
        received.Clear();
        made.Clear();
        Invoke("LoadReceived", 2);
        Invoke("LoadMade", 4);
    }
    public void LoadReceived()
    {
        LoadChallenge(true, 
              ParseObject.GetQuery("Challenges")
             .WhereEqualTo("op_facebookID", Data.Instance.userData.facebookID)
             .OrderByDescending("updatedAt")
             .Limit(90)
         );
    }
    public void LoadMade()
    {
        LoadChallenge(false,
              ParseObject.GetQuery("Challenges")
             .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
             .OrderByDescending("updatedAt")
             .Limit(90)
         );
    }
    void LoadChallenge(bool _received, ParseQuery<ParseObject> query)
    {
        if (_received)
            received.Clear();
        else
            made.Clear();

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                string objectID = result.ObjectId;
                string facebookID = result.Get<string>("facebookID");
                string op_facebookID = result.Get<string>("op_facebookID");
                string op_playerName = result.Get<string>("op_playerName");
                string playerName = result.Get<string>("playerName");
                
                int level = result.Get<int>("level");
                float score = result.Get<float>("score");

                float score2 = 0;
                string winner = "";
                try
                {
                    score2 = result.Get<float>("score2");
                    winner = result.Get<string>("winner");
                }
                catch
                {
                }
                PlayerData playerData = new PlayerData();
                playerData.objectID = objectID;
                playerData.facebookID = facebookID;
                playerData.playerName = playerName;

                if (!_received)
                {
                    playerData.facebookID = op_facebookID;
                    playerData.playerName = op_playerName;
                }

                playerData.score = score;
                playerData.level = level;

                playerData.winner = winner;
                playerData.score2 = score2;

                if (_received)
                    received.Add(playerData);
                else
                    made.Add(playerData);
            }
        }
        );
    }
    public void OnChallengeCreate(string oponent_username, string oponent_facebookID, int level, float score)
    {
        ParseObject data = new ParseObject("Challenges");
        data["playerName"] = Data.Instance.userData.username;
        data["facebookID"] = Data.Instance.userData.facebookID;
        
        data["op_playerName"] = oponent_username;
        data["op_facebookID"] = oponent_facebookID;

        data["level"] = level;
        data["score"] = score;

        data.SaveAsync();
        print("Challenge Saved");
    }
    void OnChallengeRemind(string objectID)
    {
        print("OnChallengeRemind(string objectID)  " + objectID);

        var query = new ParseQuery<ParseObject>("Challenges")
            .WhereEqualTo("objectId", objectID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["score2"] = 0;
           // data["winner"] = winner;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("OnChallengeRemind updated!");
        });   

    }
    public void OnChallengeClose(string objectID, string op_facebookID, string winner, float newScore)
    {
        var query = new ParseQuery<ParseObject>("Challenges")
            .WhereEqualTo("objectId", objectID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["score2"] = newScore;
            data["winner"] = winner;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Score updated!");
        });   
    }
    public void OnChallengeDelete(string objectId)
    {
        print("Deletechallengen");
        var query = new ParseQuery<ParseObject>("Challenges")
            .WhereEqualTo("objectId", objectId);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            return data.DeleteAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Challenge deleted!");
        });
    }
}
