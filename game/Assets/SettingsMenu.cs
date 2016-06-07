using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.Unity;
using Soomla.Store;

public class SettingsMenu : MonoBehaviour {

    public Button friendsButton;
    public Button loginButton;
    public GameObject masker;
    public GameObject SettingsButton;

	void Start () {
        masker.SetActive(false);
        if (!FB.IsLoggedIn)
        {
            friendsButton.gameObject.SetActive(false);
            SetLoginButton(false);
        }
        else{
            SetLoginButton(true);
        }
        Events.OnFacebookLogin += OnFacebookLogin;
        StoreEvents.OnRestoreTransactionsFinished += OnRestoreTransactionsFinished;
    }
    void OnDestroy()
    {
        Events.OnFacebookLogin -= OnFacebookLogin;
        StoreEvents.OnRestoreTransactionsFinished -= OnRestoreTransactionsFinished;
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
        SettingsButton.SetActive(false);
        masker.SetActive(true);
        GetComponent<Animation>().Play("SettingsOpen");
    }
    public void InviteFriends()
    {
        Events.OnFacebookInviteFriends();
    }
    public void Close()
    {
        SettingsButton.SetActive(true);
        masker.SetActive(false);
        GetComponent<Animation>().Play("SettingsClose");
    }
    void OnFacebookLogin()
    {
        Data.Instance.userData.mode = UserData.modes.SINGLEPLAYER;
        Data.Instance.Load("MainMenu");
    }
    void FBLogin()
    {
        Data.Instance.loginManager.FBLogin();
    }
    public void RestoreApp()
    {
        Events.OnLoading(true);
        SoomlaStore.RestoreTransactions();
    }
    void OnRestoreTransactionsFinished(bool events)
    {
        Events.OnLoading(false);
        if (events)
            Data.Instance.Load("MainMenu");
    }
}
