using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class UserData : MonoBehaviour {

    public bool logged;
    public string facebookID;
    public string username;
    public string email;
    public int energy;
    public List<FacebookUserData> FacebookFriends;
    public modes mode;
    public enum modes
    {
        SINGLEPLAYER,
        MULTIPLAYER
    }

    [Serializable]
    public class FacebookUserData
    {
        public string facebookID;
        public string username;
    }

	public void Init () {
        //RegisterUser("", "", "");
        if (PlayerPrefs.GetString("username") != "" && PlayerPrefs.GetString("facebookID") != "")
            SetUser(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("facebookID"), PlayerPrefs.GetString("email"));

        string modeStr = PlayerPrefs.GetString("mode");
        if (modeStr == "MULTIPLAYER") mode = modes.MULTIPLAYER;
        print("mode es : " + modeStr);
	}
    void SetUser(string username, string facebookID, string email)
    {
        this.facebookID = facebookID;
        this.username = username;
        this.email = email;
        if(username != "")
            logged = true;
    }
    public void RegisterUser(string username, string facebookID, string email)
    {
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("facebookID", facebookID);
        PlayerPrefs.SetString("email", email);
        SetUser(username, facebookID, email);
    }
    public void Reset()
    {
        logged = false;
        facebookID = "";
        username = "";
        email = "";
    }
    public void ResetFacebookFriends()
    {
        print("ResetFacebookFriends");
        FacebookFriends.Clear();
    }
    public void AddFacebookFriend(string id, string username)
    {
        FacebookUserData data = new FacebookUserData();
        data.facebookID = id;
        data.username = username;
        FacebookFriends.Add(data);
    }
    public void ToogleMode()
    {
        if(mode == modes.SINGLEPLAYER) mode = modes.MULTIPLAYER;
        else mode = modes.SINGLEPLAYER;
        PlayerPrefs.SetString("mode", mode.ToString() );
        Events.OnChangePlayMode(mode);
    }
}
