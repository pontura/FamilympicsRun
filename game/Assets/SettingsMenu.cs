using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

    public Button challengesButton;
    public Button loginButton;

	void Start () {
        if (!FB.IsLoggedIn)
        {
            challengesButton.interactable = false;
            SetLoginButton(false);
        }
        else{
            SetLoginButton(true);
        }
	}

    void SetLoginButton(bool logged)
    {
        if(logged)
            loginButton.GetComponentInChildren<Text>().text = "Log-out";
        else
            loginButton.GetComponentInChildren<Text>().text = "Log-in";
    }
    public void LoginOrOut()
    {
        if (!FB.IsLoggedIn)
            Data.Instance.Load("Login");
        else
        {
            Data.Instance.userData.Reset();
            Data.Instance.Load("MainMenu");
            Data.Instance.loginManager.ParseFBLogout();
        }
    }
    public void Open()
    {
        GetComponent<Animation>().Play("SettingsOpen");
    }
    public void Close()
    {
        GetComponent<Animation>().Play("SettingsClose");
    }
}
