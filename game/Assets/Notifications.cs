using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class Notifications : MonoBehaviour {

    public List<string> FriendsThatGaveYouEnergy;
    public List<string> FriendsThatRequestedYou;

    public int totalRequestedNotifications;
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
                .Limit(90)
            );
    }
    void LoadFromParse(ParseQuery<ParseObject> query)
    {
        FriendsThatRequestedYou.Clear();
        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
            foreach (var result in results)
            {
                string facebookID = result.Get<string>("facebookID");
                FriendsThatRequestedYou.Add(facebookID);
            }
            lastNotificationsQty = FriendsThatRequestedYou.Count;
        }
       );
    }
    void Update()
    {
        if (lastNotificationsQty != totalRequestedNotifications)
        {
            totalRequestedNotifications = lastNotificationsQty;
            Events.OnRefreshNotifications(totalRequestedNotifications);
        }
    }
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
        Events.ReFillEnergy(Data.Instance.notifications.FriendsThatGaveYouEnergy.Count);

        print("AcceptEnergyFrom" + facebookID);

        Data.Instance.notifications.UpdateNotification(facebookID, "3");

        int id = 0;
        for (int a = 0; a<Data.Instance.notifications.FriendsThatGaveYouEnergy.Count; a++)
        {
            if (Data.Instance.notifications.FriendsThatGaveYouEnergy[a] == facebookID)
                id = a;
        }
        Data.Instance.notifications.FriendsThatGaveYouEnergy.RemoveAt(id);
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
