using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Soomla.Store;


public class UserData : MonoBehaviour {

    public int tournamentAvailable;
    public int starsCount;
    public int levelProgressionId;
    public bool logged;
    public string facebookID;
    public string username;
    public string email;
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

#if UNITY_EDITOR
        SetUser("", "", "");
#endif

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
        data.username = Data.Instance.gameSettings.GetUsername(username);
        FacebookFriends.Add(data);
    }
    public void ToogleMode()
    {
        if(mode == modes.SINGLEPLAYER) mode = modes.MULTIPLAYER;
        else mode = modes.SINGLEPLAYER;
        PlayerPrefs.SetString("mode", mode.ToString() );
        Events.OnChangePlayMode(mode);
        print("ToogleMode mode: " + mode);
    }
    public int GetTournamentAvailable()
    {
        if (Data.Instance.FreeLevels) return 4;
        if (StoreData.Instance.season_2_unlocked) return 2;
        else if (StoreData.Instance.season_3_unlocked) return 3;


        int stars = Data.Instance.userData.starsCount;

       int tournamentAvailable = 1;
        if (Data.Instance.userData.levelProgressionId > 8 && stars >= Data.Instance.gameSettings.stars_for_tournament_2)
            tournamentAvailable = 2;
        if (Data.Instance.userData.levelProgressionId > 16 && stars >= Data.Instance.gameSettings.stars_for_tournament_3)
            tournamentAvailable = 3;
        else if (Data.Instance.userData.levelProgressionId > 32 && stars >= Data.Instance.gameSettings.stars_for_tournament_4)
            tournamentAvailable = 4;

        return tournamentAvailable;

    }
}
