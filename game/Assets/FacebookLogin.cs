using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System;
using Facebook;
using Facebook.MiniJSON;
using System.Linq;

public class FacebookLogin : MonoBehaviour
{
    public GameObject loggedInUIElements;
    public GameObject loggedOutUIElements;

    public ProfileModule profileModule;

    public Text DebugText;

    void Start()
    {
        Events.OnFacebookLogin += OnFacebookLogin;
    }
    void OnDestroy()
    {
        Events.OnFacebookLogin -= OnFacebookLogin;
    }
    void OnFacebookLogin()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void FBLogin()
    {
        Data.Instance.loginManager.FBLogin();
    }
    public void Back()
    {
        Data.Instance.Back();
    }
}
