using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class Challenges : MonoBehaviour {

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
    public types type;
    public enum types
    {
        RECEIVED,
        MADE        
    }
    public GameObject container;


    [SerializeField]
    ChallengesLine challengesLine;

    private int buttonsSeparation = 89;
    public List<PlayerData> userData;
    public bool infoLoaded;

    void Start()
    {
        ChallengesReceived();
    }
    public void Back()
    {
        Data.Instance.Back();
    }
    public void Switch()
    {
        if (type == types.MADE)
            ChallengesReceived();
        else
            ChallengesMade();
    }
    public void ChallengesMade()
    {
        type = types.MADE;
        LoadData();
    }
    public void ChallengesReceived()
    {
        type = types.RECEIVED;
        LoadData();
    }
    private void LoadData()
    {
        foreach (Transform childTransform in container.transform)
        {
            Destroy(childTransform.gameObject);
        }
        
        if (type == types.RECEIVED)
        {
             LoadChallenge(
                 ParseObject.GetQuery("Challenges")
                .WhereEqualTo("op_facebookID", Data.Instance.userData.facebookID)
                .Limit(90)
            );
        }
        else
        {
            LoadChallenge( 
                ParseObject.GetQuery("Challenges")
                .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
                .Limit(90)
            );
        }
    }
    void LoadChallenge(ParseQuery<ParseObject> query)
    {
        print("LoadData  " + type);

        userData.Clear();
        infoLoaded = false;

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                string objectID = result.ObjectId;
                string facebookID = result.Get<string>("facebookID");
                string op_playerName = result.Get<string>("op_playerName");
                string playerName = result.Get<string>("playerName");
                float score = result.Get<float>("score");
                int level = result.Get<int>("level");

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

                if (type == types.MADE)
                    playerData.playerName = op_playerName;
                else
                    playerData.playerName = playerName;

                playerData.score = score;
                playerData.level = level;

                playerData.winner = winner;
                playerData.score2 = score2;

                userData.Add(playerData);
                print("userData " + userData.Count);
            }
        }
        );
       
    }
    void Update() {
        if (infoLoaded) return;
        if (userData.Count > 0)
        {
            print("carga qty: " + userData.Count);
            infoLoaded = true;
            for (int a = 0; a < userData.Count; a++)
            {
                ChallengesLine newButton = Instantiate(challengesLine) as ChallengesLine;
                newButton.transform.SetParent(container.transform);
                newButton.transform.localScale = Vector3.one;
                newButton.Init(this, a);                
            }
        }
    }
    
    public void Confirm(string username, string objectID, string facebookID, float op_score)
    {
        Data.Instance.levelData.challenge_username = username;
        Data.Instance.levelData.challenge_facebookID = facebookID;
        Data.Instance.levelData.challenge_objectID = objectID;
        Data.Instance.levelData.challenge_op_score = op_score;
        Data.Instance.userData.mode = UserData.modes.SINGLEPLAYER;
        Data.Instance.Load("GameSingle");
    }
}
