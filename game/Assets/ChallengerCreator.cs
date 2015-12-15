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
    public Animation anim;

    public string lastSelectedFacebookId;

    public List<string> challengesMade;

    bool filterReady;

    public GameObject container;

    [SerializeField]
    ChallengerCreatorButton button;

    private int buttonsSeparation = 80;
   // public List<FriendData> friendsData;

    void Start()
    {
        Init();
    }
    void Init()
    {
        challengesMade.Clear();

        foreach (Transform childTransform in container.transform)
            Destroy(childTransform.gameObject);

        //anim.gameObject.SetActive(false);
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
    public void InviteFriends()
    {
        Events.OnFacebookInviteFriends();
    }
    public void CreateList()
    {
        for (int a = 0; a < Data.Instance.userData.FacebookFriends.Count; a++)
        {
            UserData.FacebookUserData data = Data.Instance.userData.FacebookFriends[a];
            
            ChallengerCreatorButton newButton = Instantiate(button) as ChallengerCreatorButton;
            newButton.transform.SetParent(container.transform);            

            string facebookID = data.facebookID;
            bool done = false;

            //si es un recien elegido...
            if (facebookID == lastSelectedFacebookId)
                done = true;
            else
            {
                foreach (string challengesMadeFBId in challengesMade)
                {
                    if (challengesMadeFBId == facebookID)
                        done = true;
                }
            }
            

            newButton.Init(this, a + 1, data.username, facebookID, done);
            newButton.transform.localScale = new Vector3(0.74f, 0.74f, 0.74f);
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
    public void Remind(string _username, string _facebookID)
    {
        facebookFriendName = _username;
        facebookFriendId = _facebookID;

        string objectID = "";

        ChallengersManager cm = Data.Instance.challengesManager;
        foreach (ChallengersManager.PlayerData data in cm.made)
        {
            if (data.facebookID == _facebookID && data.level == levelId)
            {
                objectID = data.objectID;
            }
        }
        if(objectID != "")
            Events.OnChallengeRemind(objectID, _facebookID);
    }
    public void Accept()
    {
        anim.gameObject.SetActive(true);
        anim.Play("FinishFlagOpen");
        Events.OnChallengeCreate(facebookFriendName, facebookFriendId, levelId, score);
        lastSelectedFacebookId = facebookFriendId;
        Init();
        Invoke("FlagReady", 3.7f);
    }
    void FlagReady()
    {
        anim.gameObject.SetActive(false);
    }
    public void CloseConfirmation()
    {
        confirmationPanel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        confirmationPanel.SetActive(false);
    }
    
}
