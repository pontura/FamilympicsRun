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
        public string facebookID;
        public string playerName;
        public float score;
        public int level;
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
        ParseUser.Query
         .Limit(userData.Length)
         .FindAsync().ContinueWith(t =>
         {
             IEnumerable<ParseUser> result = t.Result;
             int a = 0;
             foreach (var item in result)
             {
                 string facebookID = item.Get<string>("facebookID");
                 string playerName = item.Get<string>("playerName");
                 float score = item.Get<float>("score");
                 int level = item.Get<int>("level");

                 userData[a].facebookID = facebookID;
                 userData[a].playerName = playerName;
                 userData[a].score = score;
                 userData[a].level = level;
                 a++;
             }
         });      
    }
    public void CreateList() {
        for (int a = 0; a < Data.Instance.levels.levels.Length; a++)
        {
            ChallengesLine newButton = Instantiate(challengesLine) as ChallengesLine;
            newButton.transform.SetParent(container.transform);
            newButton.transform.localPosition = new Vector3(0, buttonsSeparation * a *-1, 0);
            newButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            newButton.Init(this, a+1);
        }
    }
    public void Back()
    {
        Application.LoadLevel("Players");
    }
    public void Confirm(string username, string facebookID)
    {
        Data.Instance.levelData.CreateChallenge(username, facebookID);
        Application.LoadLevel("ChallengeConfirm");
    }
}
