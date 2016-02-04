using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationSignal : MonoBehaviour {

    public GameObject panel;
    public ProfilePicture profilePicture;
    public Text title;
    public Text description;
    private bool isOn;
    public Animation anim;
    public types type;
    public enum types
    {
        NOTIFICATIONS,
        CHALLENGES
    }
    public GameObject defaultProfilePicture;
    public GameObject customProfilePicture;

	void Start () {
        SetOff();
        Invoke("EverySecond", 1);
        Events.OnLoading += OnLoading;
	}
    void OnLoading(bool result)
    {
        SetOff();
    }
    public void SetOn(string facebookID, string username, string _description)
    {
        
        isOn = true;
        panel.SetActive(true);

        anim.Play("NotificationSignalOn");

        if (facebookID == "")
        {
            customProfilePicture.SetActive(false);
            defaultProfilePicture.SetActive(true);
        }
        else
        {
            customProfilePicture.SetActive(true);
            defaultProfilePicture.SetActive(false);
            profilePicture.setPicture(facebookID);
        }

        title.text = username.ToUpper();
        description.text = _description.ToUpper();
    }
    public void SetOff()
    {
        isOn = false;
        panel.SetActive(false);
    }
    void EverySecond()
    {
        if (!isOn && Application.loadedLevelName == "LevelSelector")
        {
            CheckForNotifications();
        }
        Invoke("EverySecond", 1);
    }
    void CheckForNotifications()
    {

        //////////////////// chequea si agrupa los challenges:////////////////////////////////////////////////

        List<ChallengersManager.PlayerData> totalChallengesToShow = new List<ChallengersManager.PlayerData>();
        foreach (ChallengersManager.PlayerData playerData in Data.Instance.challengesManager.received)
        {
            if (!playerData.notificated && playerData.winner == "")
            {
                totalChallengesToShow.Add(playerData);
            }
        }
        foreach (ChallengersManager.PlayerData playerData in Data.Instance.challengesManager.made)
        {
            if (!playerData.notificated && playerData.winner != "")
            {
                totalChallengesToShow.Add(playerData);
            }
        }
        if (totalChallengesToShow.Count > 1)
        {
            foreach (ChallengersManager.PlayerData playerData in totalChallengesToShow)
            {
                type = types.CHALLENGES;
                string title = "You have received";
                string text = totalChallengesToShow.Count + " challenges from your friends.";

                SetOn("", title, text);
                playerData.notificated = true;
                Events.OnChallengeNotificated(playerData.objectID);
            }
            return;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        foreach( ChallengersManager.PlayerData playerData in Data.Instance.challengesManager.received)
        {
            if (!playerData.notificated && playerData.winner == "")
            {
                type = types.CHALLENGES;
                string text = "SENT YOU A NEW CHALLENGE!";
                SetOn(playerData.facebookID, playerData.playerName, text);                
                playerData.notificated = true;
                Events.OnChallengeNotificated(playerData.objectID);
                return;
            }
        }
        foreach (ChallengersManager.PlayerData playerData in Data.Instance.challengesManager.made)
        {
            if (!playerData.notificated && playerData.winner != "")
            {
                type = types.CHALLENGES;
                string text;
                if (playerData.winner != Data.Instance.userData.facebookID)
                    text = "accepted your challenge and beat you!";
                else
                    text = "accepted your challenge and lost!";

                SetOn(playerData.facebookID, playerData.playerName, text);
                playerData.notificated = true;
                Events.OnChallengeNotificated(playerData.objectID);
                return;
            }
        }





        //////////////////// chequea si agrupa los notifications:////////////////////////////////////////////////

        List<Notifications.NotificationData> totalNotificationsToShow = new List<Notifications.NotificationData>();
        foreach (Notifications.NotificationData playerData in Data.Instance.notifications.notifications)
        {
            if (!playerData.notificated && playerData.status == "0")
            {
                totalNotificationsToShow.Add(playerData);
            }
        }
        foreach (Notifications.NotificationData playerData in Data.Instance.notifications.notificationsReceived)
        {
            if (!playerData.notificated && playerData.status == "1")
            {
                totalNotificationsToShow.Add(playerData);
            }
        }
        if (totalNotificationsToShow.Count > 1)
        {
            foreach (Notifications.NotificationData playerData in totalNotificationsToShow)
            {
                type = types.NOTIFICATIONS;

                string title = "You have received";
                string text = totalNotificationsToShow.Count + " Energy Gifts.";

                SetOn("", title, text);
                playerData.notificated = true;
                Data.Instance.notifications.NotificationNotificated(playerData.facebookID, Data.Instance.userData.facebookID);
            }
            return;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////



        foreach (Notifications.NotificationData data in Data.Instance.notifications.notifications)
        {
            if (!data.notificated && data.status == "0")
            {
                type = types.NOTIFICATIONS;
                string text = "is requesting energy.";
                string username = Data.Instance.userData.GetUsernameByFacebookID(data.facebookID);
                SetOn(data.facebookID, username, text);
                data.notificated = true;
                Data.Instance.notifications.NotificationNotificated(data.facebookID, Data.Instance.userData.facebookID);
                return;
            }
        }
        foreach (Notifications.NotificationData data in Data.Instance.notifications.notificationsReceived)
        {
            if (!data.notificated && data.status == "1")
            {
                type = types.NOTIFICATIONS;
                string text = "sent you energy!";
                string username = Data.Instance.userData.GetUsernameByFacebookID(data.asked_facebookID);
                SetOn(data.asked_facebookID, username, text);
                data.notificated = true;
                Data.Instance.notifications.NotificationNotificated(Data.Instance.userData.facebookID, data.asked_facebookID);
                return;
            }
        }
    }
    public void Clicked()
    {
        if (type == types.NOTIFICATIONS)
            Data.Instance.Load("Notifications");
        else
            Data.Instance.Load("Challenges");

        SetOff();
    }
}
