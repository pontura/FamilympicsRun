using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

    public Button challengesButton;
    public Button loginButton;

	void Start () {
        if (Data.Instance.userData.facebookID == "")
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
        if (Data.Instance.userData.facebookID == "")
            Data.Instance.Load("Login");
        else
            Data.Instance.loginManager.ParseFBLogout();
    }
    public void Open()
    {
        animation.Play("SettingsOpen");
    }
    public void Close()
    {
        animation.Play("SettingsClose");
    }
}
