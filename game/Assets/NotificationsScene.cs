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
    public GameObject empty;
    public Text title;
    public GameObject scrollbars;
    public GameObject popup;

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
        if (Data.Instance.notifications.notifications.Count == 0 && Data.Instance.notifications.notificationsReceived.Count == 0)
            ShowEmpty();
        else
        {
            empty.SetActive(false);
            CreateList();
        }
    }
    void ShowEmpty()
    {
        empty.SetActive(true);
        title.text = "";
        scrollbars.SetActive(false);
    }
    //void FilterChellengers()
    //{
    //    LoadFriends(
    //             ParseObject.GetQuery("Notifications")
    //            .WhereEqualTo("asked_facebookID", Data.Instance.userData.facebookID)
    //            .Limit(90)
    //        );
    //}
    //void LoadFriends(ParseQuery<ParseObject> query)
    //{
    //    query.FindAsync().ContinueWith(t =>
    //    {
    //        IEnumerable<ParseObject> results = t.Result;
    //        foreach (var result in results)
    //        {
    //            string asked_facebookID = result.Get<string>("asked_facebookID");
    //            string facebookID = result.Get<string>("facebookID");
    //            string status = result.Get<string>("status");

    //            if (status != "3")
    //            {
    //                NotificationData notification = new NotificationData();

    //                notification.asked_facebookID = asked_facebookID;
    //                notification.facebookID = facebookID;
    //                notification.status = status;
                
    //                notifications.Add(notification);
    //            }
    //        }
    //        filterReady = true;
    //    }
    //    );
    //}
    //void Update()
    //{
    //    if (filterReady)
    //    {
    //        filterReady = false;
    //        CreateList();
    //    }
    //}
    public void CreateList()
    {
        //ya te dieron energia:
        foreach (Notifications.NotificationData data in Data.Instance.notifications.notifications)
        {
            NotificationButton newButton = Instantiate(button) as NotificationButton;

            newButton.transform.SetParent(container.transform);
            newButton.transform.localScale = Vector3.one;

            string username = "";

            foreach (UserData.FacebookUserData facebookData in Data.Instance.userData.FacebookFriends)
            {
                if (facebookData.facebookID == data.facebookID)
                    username = facebookData.username;
            }
            newButton.Init(this, username, data.facebookID, data.status);
        }

        //ya te dieron energia:
        foreach (Notifications.NotificationData data in Data.Instance.notifications.notificationsReceived)
        {
            NotificationButton newButton = Instantiate(button) as NotificationButton;

            newButton.transform.SetParent(container.transform);
            newButton.transform.localScale = Vector3.one;

            string username = "";

            foreach (UserData.FacebookUserData facebookData in Data.Instance.userData.FacebookFriends)
            {
                if (facebookData.facebookID == data.asked_facebookID)
                    username = facebookData.username;
            }
            newButton.Init(this, username, data.asked_facebookID, data.status);
        }
    }
    
    private void SetNewEnergyAccepted()
    {
        Events.ReFillEnergy(friendsIdsThatGaveYouEnergy.Count);
       // print("new Energy qty: " + friendsIdsThatGaveYouEnergy.Count);
        foreach (string asked_facebookID in friendsIdsThatGaveYouEnergy)
        {
            Data.Instance.notifications.DeleteNotification(Data.Instance.userData.facebookID, asked_facebookID);
        }
    }
    public void AcceptFail()
    {
        OpenPopup();
    } 
    public void OpenPopup()
    {
        popup.SetActive(true);
        popup.GetComponent<Animation>().Play("PopupOn");
    }
    public void ClosePopup()
    {
        popup.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        popup.SetActive(false);
    }
    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void Select(string _facebookID, string username)
    {
        Events.SendNotificationTo(_facebookID, username);
        Debug.Log("Select " + _facebookID);
    }
    public void ShowFacebookFriends()
    {
        Data.Instance.GetComponent<ChallengersManager>().showFacebookFriends = true;
        Data.Instance.Load("ChallengeCreator");
    }
     
}
