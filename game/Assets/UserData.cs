using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserData : MonoBehaviour {

    public bool logged;
    public string facebookID;
    public string username;
    public string parseUsername;

	public void Init () {
        RegisterUser("", "");
        if (PlayerPrefs.GetString("username") != "" && PlayerPrefs.GetString("facebookID") != "")
            SetUser(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("facebookID"));
	}
    void SetUser(string username, string facebookID)
    {
        this.facebookID = facebookID;
        this.username = username;
        logged = true;
    }
    public void RegisterUser(string username, string facebookID)
    {
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("facebookID", facebookID);

        SetUser(username, facebookID);
    }
}
