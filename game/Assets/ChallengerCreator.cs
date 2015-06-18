using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class ChallengerCreator : MonoBehaviour {

    [Serializable]
    public class PlayerData
    {
        public string facebookID;
        public string playerName;
    }

    public GameObject container;

    [SerializeField]
    ChallengerCreatorButton button;

    private int buttonsSeparation = 55;
    public PlayerData[] userData;

    void Start()
    {
        if(Data.Instance.GetComponent<ChallengersManager>().showFacebookFriends)
            LoadFacebookFriends();
        else
            LoadParseUsers();

        CreateList();
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
    public void LoadFacebookFriends()
    {
        FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
    }
    void FBFriendsCallback(FBResult result)
    {
        Debug.Log("APICallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            // Let's just try again
            FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
            return;
        }
        print("FBFriendsCallback");
        List<object> friends = Util.DeserializeJSONFriends(result.Text);
        int a = 0;
        foreach (object friend in friends)
        {
            Dictionary<string, object> friendData = friend as Dictionary<string, object>;
            foreach (KeyValuePair<string, object> keyval in friendData)
            {
                if (keyval.Key == "id")
                    userData[a].facebookID = keyval.Value.ToString();
                else if (keyval.Key == "first_name")
                    userData[a].playerName = keyval.Value.ToString();
            }
            a++;
        }
    }
    private void LoadParseUsers()
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
                 Debug.Log(facebookID + " " + playerName);
                // CreateList(facebookID, playerName);
                 userData[a].facebookID = facebookID;
                 userData[a].playerName = playerName;
                 a++;
             }
         });      
    }
    public void CreateList() {
        for (int a = 0; a < userData.Length-1; a++)
        {
            ChallengerCreatorButton newButton = Instantiate(button) as ChallengerCreatorButton;
            newButton.transform.SetParent(container.transform);
            newButton.transform.localPosition = new Vector3(0, buttonsSeparation * a *-1, 0);
            newButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            newButton.Init(this, a+1);
        }
    }
    public void Back()
    {
        Data.Instance.Back();
    }
    public void Challenge(string username, string facebookID)
    {
        Data.Instance.levelData.CreateChallenge(username, facebookID);
        Data.Instance.Load("ChallengeConfirm");
    }
}
