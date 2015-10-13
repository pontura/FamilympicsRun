using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class Notifications : MonoBehaviour {

    [Serializable]
    public class NotificationData
    {
        public string asked_facebookID;
        public string facebookID;
        public string status;
    }

    public List<NotificationData> notifications;
    public List<NotificationData> notificationsReceived;

    private int lastNotificationsQty;

	void Start () {
        Events.CheckForNewNotifications += CheckForNewNotifications;
        Events.OnNotificationReceived += OnNotificationReceived;
        Events.SendNotificationTo += SendNotificationTo;
        Events.OnAcceptEnergyFrom += OnAcceptEnergyFrom;
	}
    void OnDestroy()
    {
        Events.CheckForNewNotifications -= CheckForNewNotifications;
        Events.OnNotificationReceived -= OnNotificationReceived;
        Events.SendNotificationTo += SendNotificationTo;
        Events.OnAcceptEnergyFrom -= OnAcceptEnergyFrom;
    }
    void OnNotificationReceived(string facebookId)
    {
        Debug.Log("OnNotificationReceived from :" + facebookId);
    }
    void SendNotificationTo(string friend_facebookId)
    {
        Debug.Log("OnNotificationSend to :" + friend_facebookId);

        ParseObject parseObj = new ParseObject("Notifications");

        parseObj["facebookID"] = Data.Instance.userData.facebookID;
        parseObj["asked_facebookID"] = friend_facebookId;
        parseObj["status"] = "0";

        parseObj.SaveAsync();
    }
    void CheckForNewNotifications()
    {
        LoadFromParse(
                 ParseObject.GetQuery("Notifications")
                .WhereEqualTo("asked_facebookID", Data.Instance.userData.facebookID)
                .WhereEqualTo("status", "0")
                .OrderByDescending("updatedAt")
                .Limit(90)
            );
    }
    void LoadFromParse(ParseQuery<ParseObject> query)
    {
        notifications.Clear();
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                NotificationData data = new NotificationData();
                data.asked_facebookID = result.Get<string>("asked_facebookID");
                data.facebookID = result.Get<string>("facebookID");
                data.status = result.Get<string>("status");
                notifications.Add(data);
            }
        }
       );
       CheckForNewNotificationsReceived();
    }
    void CheckForNewNotificationsReceived()
    {
        print("CheckForNewNotificationsReceived");

        LoadFromParseReceived(
                 ParseObject.GetQuery("Notifications")
                .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
                .WhereNotEqualTo("status", "0")
                .OrderByDescending("updatedAt")
                .Limit(90)
            );
    }
    void LoadFromParseReceived(ParseQuery<ParseObject> query)
    {
        notificationsReceived.Clear();
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                NotificationData data = new NotificationData();
                data.asked_facebookID = result.Get<string>("asked_facebookID");
                data.facebookID = result.Get<string>("facebookID");
                data.status = result.Get<string>("status");
                notificationsReceived.Add(data);
            }
        }
       );
    }
    //void Update()
    //{
    //    if (lastNotificationsQty != totalRequestedNotifications)
    //    {
    //        totalRequestedNotifications = lastNotificationsQty;
    //        //Events.OnRefreshNotifications(totalRequestedNotifications);
    //    }
    //}
    public void UpdateNotification(string asked_facebookID, string status)
    {
        var query = new ParseQuery<ParseObject>("Notifications")
            .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
            .WhereEqualTo("asked_facebookID", asked_facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["status"] = status;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Notification updated!");
        });
    }
    public void OnAcceptEnergyFrom(string facebookID)
    {
        Events.ReFillEnergy(1);
        print("OnAcceptEnergyFrom : " + facebookID);

        int id = 0;
        for (int a = 0; a < notificationsReceived.Count; a++)
        {
            if (notificationsReceived[a].asked_facebookID == facebookID)
                id = a;
        }
        notificationsReceived.RemoveAt(id);
        DeleteNotification(facebookID);
    }
    public void DeleteNotification(string asked_facebookID)
    {
        print("DeleteNotification");
        var query = new ParseQuery<ParseObject>("Notifications")
            .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
            .WhereEqualTo("asked_facebookID", asked_facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            //data["status"] = "3";
            return data.DeleteAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Notification deleted!");
        });
    }
}
