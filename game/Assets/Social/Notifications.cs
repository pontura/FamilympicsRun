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
        public bool notificated;
    }

    public List<NotificationData> notifications;
    public List<NotificationData> notificationsReceived;

    private int lastNotificationsQty;

	void Start () {
        Events.CheckForNewNotifications += CheckForNewNotifications;
        Events.OnNotificationReceived += OnNotificationReceived;
        Events.SendNotificationTo += SendNotificationTo;
        Events.OnAcceptEnergyFrom += OnAcceptEnergyFrom;
        Events.OnResetApp += OnResetApp;
    }    
    void OnDestroy()
    {
        Events.CheckForNewNotifications -= CheckForNewNotifications;
        Events.OnNotificationReceived -= OnNotificationReceived;
        Events.SendNotificationTo -= SendNotificationTo;
        Events.OnAcceptEnergyFrom -= OnAcceptEnergyFrom;
        Events.OnResetApp -= OnResetApp;
    }
    void OnResetApp()
    {
        notifications.Clear();
        notificationsReceived.Clear();
    }
    void OnNotificationReceived(string facebookId)
    {
        Debug.Log("OnNotificationReceived from :" + facebookId);
    }
    void SendNotificationTo(string friend_facebookId, string username)
    {
        Debug.Log("OnNotificationSend to :" + friend_facebookId);

        ParseObject parseObj = new ParseObject("Notifications");

        parseObj["facebookID"] = Data.Instance.userData.facebookID;
        parseObj["asked_facebookID"] = friend_facebookId;
        parseObj["status"] = "0";
        parseObj["notificated"] = false;

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
                data.notificated = result.Get<bool>("notificated");
                notifications.Add(data);
            }
        }
       );
       CheckForNewNotificationsReceived();
    }
    void CheckForNewNotificationsReceived()
    {
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
                data.notificated = result.Get<bool>("notificated");
                notificationsReceived.Add(data);
            }
        }
       );
    }
    public void NotificationNotificated(string facebookID, string asked_facebookID)
    {
        print("____________NotificationNotificated");
        var query = new ParseQuery<ParseObject>("Notifications")
            .WhereEqualTo("facebookID", facebookID)
            .WhereEqualTo("asked_facebookID", asked_facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["notificated"] = true;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Notification updated!");
        });
    }
    public void UpdateNotification(string asked_facebookID, string status)
    {
        print("_________________UpdateNotification");
        var query = new ParseQuery<ParseObject>("Notifications")
            .WhereEqualTo("facebookID", Data.Instance.userData.facebookID)
            .WhereEqualTo("asked_facebookID", asked_facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["status"] = status;
           // if (status == "1")
            data["notificated"] = false;
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
