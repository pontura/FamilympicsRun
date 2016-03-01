using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationsChallenge : MonoBehaviour {

    public int num;
    public GameObject panel;
    public Text field;
    public ProfilePicture picture;
    public int levelId;
    public Animation anim;
    public Animation animFlag;
    public float score;

    public string facebookID;
    public string username;

    private bool clicked;

	void Start () {
        panel.SetActive(false);        
        Events.OnCheckIfAutomaticChallenge += OnCheckIfAutomaticChallenge;
        Events.OnChallengeCreate += OnChallengeCreate;
	}
    void OnChallengeCreate(string username, string facebookID, int levelId, float score)
    {
        num = 0;
    }
    void OnCheckIfAutomaticChallenge(float score)
    {
        clicked = false;
        levelId = Data.Instance.levels.currentLevel;
        animFlag.gameObject.SetActive(false);
        this.score = score;
        num++;
        if (num > 9)
            GetFreeFriend();
	}

    void GetFreeFriend()
    {
        UserData.FacebookUserData dataShuffle = Data.Instance.userData.FacebookFriends[0];
        Data.Instance.userData.FacebookFriends.Remove(dataShuffle);
        Data.Instance.userData.FacebookFriends.Add(dataShuffle);

        foreach ( UserData.FacebookUserData data in Data.Instance.userData.FacebookFriends)
        {
            if ( CheckIfUserAvailable(data.facebookID) )
            {
                Activate(data.facebookID, data.username);
                return;
            }
        }
    }
    bool CheckIfUserAvailable(string facebookID)
    {

        foreach (ChallengersManager.PlayerData data in Data.Instance.challengesManager.made)
        {
            if (data.facebookID == facebookID)
                if (data.level == levelId)
                    return false;
        }
        return true;
    }
    void Activate(string _facebookID, string _username)
    {
        animFlag.gameObject.SetActive(false);
        panel.SetActive(true);
        anim.Play("NotificationSignalOn");
        num = 0;
        picture.setPicture(_facebookID);
        string str = "challenge " + _username + "?";
        field.text = str.ToUpper();

        this.facebookID = _facebookID;
        this.username = _username;  
        
    }
    public void SendChallenge()
    {
        if (clicked) return;
        clicked = true;
        Debug.Log("SendChallenge");
        animFlag.gameObject.SetActive(true);
        animFlag.Play("FinishFlagOpen");
        Events.OnChallengeCreate(username, facebookID, levelId, score);
        Invoke("FlagReady", 3.7f);
    }
    void FlagReady()
    {
        panel.gameObject.SetActive(false);
    }
}
