using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class Notifications : MonoBehaviour {

    public List<string> FriendsThatRequestedYou;
    public int totalRequestedNotifications;
    private int lastNotificationsQty;

	void Start () {
        Events.CheckForNewNotifications += CheckForNewNotifications;
        Events.OnNotificationReceived += OnNotificationReceived;
        Events.SendNotificationTo += SendNotificationTo;
	}
    void OnDestroy()
    {
        Events.CheckForNewNotifications -= CheckForNewNotifications;
        Events.OnNotificationReceived -= OnNotificationReceived;
        Events.SendNotificationTo += SendNotificationTo;
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
}
