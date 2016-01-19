
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Facebook.Unity;

public class NotificationsEnergyPopup : MonoBehaviour {

    public List<string> FriendsThatGaveYouEnergy;
    public GameObject panel;

    [Serializable]
    public class NotificationData
    {
        public string asked_facebookID;
        public string facebookID;
        public string status;
    }    

    void Start()
    {
        if (FB.IsLoggedIn)
        {
            panel.SetActive(false);
            LoadFriends(
                     ParseObject.GetQuery("Notifications")
                    .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
                    .WhereEqualTo("status", "1")
                    .Limit(90)
                );
        }
    }
    void LoadFriends(ParseQuery<ParseObject> query)
    {
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            FriendsThatGaveYouEnergy.Clear();
            foreach (var result in results)
            {
                string asked_facebookID = result.Get<string>("asked_facebookID");
                string facebookID = result.Get<string>("facebookID");
                string status = result.Get<string>("status");

                NotificationData notification = new NotificationData();

                notification.asked_facebookID = asked_facebookID;
                notification.facebookID = facebookID;
                notification.status = status;

               FriendsThatGaveYouEnergy.Add(asked_facebookID);
            }
        }
        );
    }
    private bool ready;
    void Update()
    {
        if (ready) return;
        if (FriendsThatGaveYouEnergy.Count > 0)
        {
           // Data.Instance.notifications.FriendsThatGaveYouEnergy = FriendsThatGaveYouEnergy;
            ready = true;
            SetNewEnergyAccepted();
            Events.OnRefreshNotifications(FriendsThatGaveYouEnergy.Count);
        }
    }
    private void SetNewEnergyAccepted()
    {
        //panel.SetActive(true);
    }    
    public void Notifications()
    {
        Data.Instance.Load("Notifications");
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
