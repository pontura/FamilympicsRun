using UnityEngine;
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

    public GameObject container;

    [SerializeField]
    ChallengesLine challengesLine;

    private int buttonsSeparation = 55;
    public PlayerData[] userData;

    void Start()
    {
        LoadData();
        CreateList();
    }
    private void LoadData()
    {
        var  query = ParseObject.GetQuery("Challenges")
            .WhereEqualTo("op_facebookID", Data.Instance.userData.facebookID)
            .Limit(userData.Length);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            int a = 0;
            foreach (var result in results)
            {
                string objectID = result.ObjectId;
                string facebookID = result.Get<string>("facebookID");
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

                userData[a].objectID = objectID;
                userData[a].facebookID = facebookID;
                userData[a].playerName = playerName;                
                userData[a].score = score;                
                userData[a].level = level;

                userData[a].winner = winner;
                userData[a].score2 = score2;
                a++;
            }
        });        
    }
    public void CreateList() {
        for (int a = 0; a < userData.Length; a++)
        {
            ChallengesLine newButton = Instantiate(challengesLine) as ChallengesLine;
            newButton.transform.SetParent(container.transform);
            newButton.transform.localPosition = new Vector3(0, buttonsSeparation * a *-1, 0);
            newButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            newButton.Init(this, a);
        }
    }
    public void Back()
    {
        Application.LoadLevel("LevelSelector");
    }
    public void Confirm(string objectID, string facebookID, float op_score)
    {
        Data.Instance.levelData.challenge_facebookID = facebookID;
        Data.Instance.levelData.challenge_objectID = objectID;
        Data.Instance.levelData.challenge_op_score = op_score;
        Application.LoadLevel("Game");
    }
}
