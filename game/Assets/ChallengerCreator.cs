using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class ChallengerCreator : MonoBehaviour {

    public GameObject confirmationPanel;
    public Text levelField;
    public Text scoreField;
    public string facebookFriendName;
    public string facebookFriendId;
    private int levelId;
    private float score;

    public List<string> challengesMade;

    bool filterReady;

    public GameObject container;

    [SerializeField]
    ChallengerCreatorButton button;

    private int buttonsSeparation = 80;
   // public List<FriendData> friendsData;

    void Start()
    {
        confirmationPanel.SetActive(false);
        levelId = Data.Instance.levels.currentLevel;
        levelField.text = "LEVEL " + levelId;
        score = Data.Instance.levelsData.levelsScore[levelId].myScore;
        scoreField.text = Data.Instance.levelsData.GetScoreString(levelId, score);
        FilterChellengers();
    }
    
    void FilterChellengers()
    {
         LoadChallenge(
                 ParseObject.GetQuery("Challenges")
                .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
                .WhereEqualTo("level", levelId)
                .Limit(90)
            );
    }
    void LoadChallenge(ParseQuery<ParseObject> query)
    {
        query.FindAsync().ContinueWith(t =>
        {
            print("LoadChallenge");
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                string facebookID = result.Get<string>("op_facebookID");
                challengesMade.Add(facebookID);
            }
            filterReady = true;
        }
        );
    }
    void Update()
    {
        if (filterReady)
        {
            filterReady = false;
            print("challengesMade count: " + challengesMade.Count);
            CreateList();
        }
    }
    public void CreateList()
    {
        for (int a = 0; a < Data.Instance.userData.FacebookFriends.Count; a++)
        {
            UserData.FacebookUserData data = Data.Instance.userData.FacebookFriends[a];
            
            ChallengerCreatorButton newButton = Instantiate(button) as ChallengerCreatorButton;
            newButton.transform.SetParent(container.transform);
            newButton.transform.localPosition = new Vector3(0, buttonsSeparation * a * -1, 0);
            newButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            string facebookID = data.facebookID;
            bool done = false;
            foreach (string challengesMadeFBId in challengesMade)
            {
                if (challengesMadeFBId == facebookID)
                    done = true;
            }

            newButton.Init(this, a + 1, data.username, facebookID, done);
        }
    }


    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void Challenge(string _username, string _facebookID)
    {
        facebookFriendName = _username;
        facebookFriendId = _facebookID;

        confirmationPanel.SetActive(true);
        GetComponent<ChallengeConfirm>().Init(facebookFriendName, facebookFriendId );
        confirmationPanel.GetComponent<Animation>().Play("PopupOn");
    }
    public void ShowParseFriends()
    {
        Data.Instance.GetComponent<ChallengersManager>().showFacebookFriends = false;
        Data.Instance.Load("ChallengeCreator");
    }
    public void ShowFacebookFriends()
    {
        Data.Instance.GetComponent<ChallengersManager>().showFacebookFriends = true;
        Data.Instance.Load("ChallengeCreator");
    }
    public void Accept()
    {
        Events.OnChallengeCreate(facebookFriendName, facebookFriendId, levelId, score);
        CloseOff();
    }
    public void CloseConfirmation()
    {
        confirmationPanel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        confirmationPanel.SetActive(false);
        Data.Instance.Load("ChallengeCreator");
    }
    //public void LoadFacebookFriends()
    //{
    //    FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
    //}
    //void FBFriendsCallback(FBResult result)
    //{
    //    Debug.Log("APICallback");
    //    if (result.Error != null)
    //    {
    //        Debug.LogError(result.Error);
    //        // Let's just try again
    //        FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
    //        return;
    //    }
    //    print("FBFriendsCallback");
    //    List<object> friends = Util.DeserializeJSONFriends(result.Text);
    //    int a = 0;
    //    foreach (object friend in friends)
    //    {
    //        Dictionary<string, object> friendData = friend as Dictionary<string, object>;
    //        FriendData newFriendData = new FriendData();
    //        foreach (KeyValuePair<string, object> keyval in friendData)
    //        {
    //            if (keyval.Key == "id")
    //                newFriendData.facebookID = keyval.Value.ToString();
    //            else if (keyval.Key == "first_name")
    //                newFriendData.playerName = keyval.Value.ToString();
    //        }
    //        friendsData.Add(newFriendData);
    //        a++;
    //    }
    //    CreateList();
    //}
    //private void LoadParseUsers()
    //{
    //    ParseUser.Query
    //     .Limit(userData.Length)
    //     .FindAsync().ContinueWith(t =>
    //     {
    //         IEnumerable<ParseUser> result = t.Result;
    //         int a = 0;
    //         foreach (var item in result)
    //         {
    //             string facebookID = item.Get<string>("facebookID");
    //             string playerName = item.Get<string>("playerName");
    //             Debug.Log(facebookID + " " + playerName);
    //            // CreateList(facebookID, playerName);
    //             userData[a].facebookID = facebookID;
    //             userData[a].playerName = playerName;
    //             a++;
    //         }
    //     });      
    //}
    
}
