﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.Unity;   

public class FacebookFriends : MonoBehaviour {

    [Serializable]
    public class Friend
    {
        public string id;
        public Texture2D picture;
    }
    public IList<string> ids;
    public List<Friend> all;
    private string icon_url = "http://tipitap.com/running-icon.jpg";

    //void Awake()
    //{
    //    if (FB.IsInitialized)
    //    {
    //        FB.ActivateApp();
    //    }
    //    else
    //    {
    //        //Handle FB.Init
    //        FB.Init(() =>
    //        {
    //            FB.ActivateApp();
    //        });
    //    }
    //}

    //// Unity will call OnApplicationPause(false) when an app is resumed
    //// from the background
    //void OnApplicationPause(bool pauseStatus)
    //{
    //    // Check the pauseStatus to see if we are in the foreground
    //    // or background
    //    if (!pauseStatus)
    //    {
    //        //app resume
    //        if (FB.IsInitialized)
    //        {
    //            FB.ActivateApp();
    //        }
    //        else
    //        {
    //            //Handle FB.Init
    //            FB.Init(() =>
    //            {
    //                FB.ActivateApp();
    //            });
    //        }
    //    }
    //}

	void Start () {
        ids = new List<string>();
        Events.AddFacebookFriend += AddFacebookFriend;
        Events.OnFacebookInviteFriends += OnFacebookInviteFriends;
	}
    void OnFacebookInviteFriends()
    {



        FB.Mobile.AppInvite(
           // new Uri("https://fb.me/1248005065213962"),
            new Uri("https://fb.me/1271323376215464")
            , callback: this.HandleResult
            //new Uri(icon_url),
            //AppInviteCallback
        );
        //FB.AppRequest(
        //    "Running!", null, null, null, null, "Come and play Running!", null
        //);
    }
    private string LastResponse;
    private string Status;
    protected void HandleResult(IResult result)
    {
        Debug.Log("________________HandleResult_____________");
        if (result == null)
        {
            this.LastResponse = "Null Response\n";
            Debug.Log(" Null " + this.LastResponse);
            return;
        }
        if (!string.IsNullOrEmpty(result.Error))
        {
            this.Status = "Error - Check log for details";
            this.LastResponse = "Error Response:\n" + result.Error;
            Debug.Log(Status + " - " + result.Error);
        }
        else if (result.Cancelled)
        {
            this.Status = "Cancelled - Check log for details";
            this.LastResponse = "Cancelled Response:\n" + result.RawResult;
             Debug.Log(Status + " - " + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = "Success Response:\n" + result.RawResult;
            Debug.Log(Status + " - " + result.RawResult);
        }
        else
        {
            this.LastResponse = "Empty Response\n";
            Debug.Log(this.LastResponse);
        }
    }
    void AppInviteCallback(IAppInviteResult result)
    {
        Debug.Log("_______IAppInviteResult: " + result.Cancelled);

        if (!result.Cancelled)
            Events.ReFillEnergy(1);

    }
    void AddFacebookFriend(string id, string username)
    {
        ids.Add(id);
        Friend friend = new Friend();
        friend.id = id;
        all.Add(friend);
        StartCoroutine(GetPicture(id));
    }
    IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "")
            yield break;

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
            SetProfilePicture(facebookID, receivedData.texture);
    }
    public void SetProfilePicture(string facebookID, Texture2D picture)
    {
        foreach (Friend friend in all)
        {
            if (friend.id == facebookID)
                friend.picture = picture;
        }
    }
    public Texture2D GetProfilePicture(string facebookID)
    {
        foreach (Friend friend in all)
        {
            if (friend.id == facebookID)
                return friend.picture;
        }
        return null;
    }
}
