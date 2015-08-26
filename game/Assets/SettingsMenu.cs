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
        Events.OnFacebookLogin += OnFacebookLogin;
    }
    void OnDestroy()
    {
        Events.OnFacebookLogin -= OnFacebookLogin;
    }

    void SetLoginButton(bool logged)
    {
        if(logged)
            loginButton.GetComponentInChildren<Text>().text = "DISCONNECT";
        else
            loginButton.GetComponentInChildren<Text>().text = "CONNECT FACEBOOK";
    }
    public void LoginOrOut()
    {
        if (!FB.IsLoggedIn)
        {
            FBLogin();
            Close();
        }
        else
        {
            Data.Instance.userData.Reset();
            Data.Instance.Load("MainMenu");
            Data.Instance.loginManager.ParseFBLogout();
        }
    }
    public void Rules()
    {

    }
    public void Policy()
    {
        Data.Instance.Load("Policy");
    }
    public void Open()
    {
        GetComponent<Animation>().Play("SettingsOpen");
    }
    public void Close()
    {
        GetComponent<Animation>().Play("SettingsClose");
    }
    void OnFacebookLogin()
    {
        Data.Instance.userData.mode = UserData.modes.SINGLEPLAYER;
        Data.Instance.Load("LevelSelector");
    }
    void FBLogin()
    {
        Data.Instance.loginManager.FBLogin();
    }
}
