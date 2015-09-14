using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class NotificationsScene : MonoBehaviour {

    public List<NotificationData> notifications;
    public List<string> friendsIdsThatGaveYouEnergy;

    [Serializable]
    public class NotificationData
    {
        public string asked_facebookID;
        public string facebookID;
        public string status;
    }    

    bool filterReady;

    public GameObject container;

    [SerializeField]
    NotificationButton button;
    
    private int buttonsSeparation = 80;


    void Start()
    {
      //  CreateList();
        FilterChellengers();
    }
    
    void FilterChellengers()
    {
        LoadFriends(
                 ParseObject.GetQuery("Notifications")
                .WhereEqualTo("asked_facebookID", Data.Instance.userData.facebookID)
                .Limit(90)
            );
    }
    void LoadFriends(ParseQuery<ParseObject> query)
    {
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                string asked_facebookID = result.Get<string>("asked_facebookID");
                string facebookID = result.Get<string>("facebookID");
                string status = result.Get<string>("status");

                NotificationData notification = new NotificationData();

                notification.asked_facebookID = asked_facebookID;
                notification.facebookID = facebookID;
                notification.status = status;

                notifications.Add(notification);
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
            CreateList();
        }
    }
    public void CreateList()
    {
        //ya te dieron energia:
        foreach (string asked_facebookId in Data.Instance.notifications.FriendsThatGaveYouEnergy)
        {
            NotificationButton newButton = Instantiate(button) as NotificationButton;

            newButton.transform.SetParent(container.transform);
            newButton.transform.localScale = Vector3.one;

            string username = "";

            foreach (UserData.FacebookUserData data in Data.Instance.userData.FacebookFriends)
            {
                if (data.facebookID == asked_facebookId)
                    username = data.username;
            }
            newButton.Init(this, username, asked_facebookId, "3");
        }
        for (int a = 0; a < notifications.Count; a++)
        {
            if (notifications[a].facebookID == Data.Instance.userData.facebookID && notifications[a].status == "1")
                friendsIdsThatGaveYouEnergy.Add(notifications[a].asked_facebookID);

            NotificationButton newButton = Instantiate(button) as NotificationButton;

            newButton.transform.SetParent(container.transform);
            newButton.transform.localPosition = new Vector3(0, buttonsSeparation * a * -1, 0);
            newButton.transform.localScale = Vector3.one;

            bool done = false;

            string username = "";

            foreach (UserData.FacebookUserData data in Data.Instance.userData.FacebookFriends)
            {
                print(data.facebookID + " --  " + notifications[a].facebookID);

                if (data.facebookID == notifications[a].facebookID)
                    username = data.username;
            }            
            newButton.Init(this, username, notifications[a].facebookID, notifications[a].status);
        }
        if (friendsIdsThatGaveYouEnergy.Count > 0)
            SetNewEnergyAccepted();

        float _h = buttonsSeparation * (Data.Instance.userData.FacebookFriends.Count + 2);
        int container_width = 948;
        Events.OnScrollSizeRefresh(new Vector2(container_width, _h));
    }
    private void SetNewEnergyAccepted()
    {
        Events.ReFillEnergy(friendsIdsThatGaveYouEnergy.Count);
       // print("new Energy qty: " + friendsIdsThatGaveYouEnergy.Count);
        foreach (string asked_facebookID in friendsIdsThatGaveYouEnergy)
        {
            Data.Instance.notifications.DeleteNotification(asked_facebookID);
        }
    }

    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void Select(string _facebookID)
    {
        Events.SendNotificationTo(_facebookID);
        Debug.Log("Select " + _facebookID);
    }
    public void ShowFacebookFriends()
    {
        Data.Instance.GetComponent<ChallengersManager>().showFacebookFriends = true;
        Data.Instance.Load("ChallengeCreator");
    }
    public void Accept()
    {
       // Events.OnChallengeCreate(facebookFriendName, facebookFriendId, levelId, score);
        //CloseOff();
    }  
}
