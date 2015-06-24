using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System;
using Facebook;
using Facebook.MiniJSON;
using System.Linq;



public class LoginManager : MonoBehaviour
{
    private bool triedToLogin;

    void Start()
    {
        if (FB.IsLoggedIn)
        {
            if (ParseUser.CurrentUser == null)
            {
                StartCoroutine("ParseLogin");
            }
            else
            {
                UpdateProfile();
            }
            GetFriends();
        }

        if (FB.IsLoggedIn) return;
        if(triedToLogin) return;

        triedToLogin = true;
        print("LoginManager Awake");

        FB.Init(SetInit, OnHideUnity);
        
    }

    private void SetInit()
    {
        if (this)
            enabled = true;
    }
    public void Back()
    {
        Data.Instance.Back();
    }

    private void OnHideUnity(bool isGameShown)
    {
        print("FB LOOGED" + isGameShown);
        if (!isGameShown)
        {
            // pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            
            Time.timeScale = 1;
        }
    }

    private IEnumerator ParseLogin()
    {
        if (FB.IsLoggedIn)
        {
            // Logging in with Parse
            var loginTask = ParseFacebookUtils.LogInAsync(FB.UserId,
                                                          FB.AccessToken,
                                                          DateTime.Now);
            while (!loginTask.IsCompleted) yield return null;
            // Login completed, check results
            if (loginTask.IsFaulted || loginTask.IsCanceled)
            {
                // There was an error logging in to Parse
                foreach (var e in loginTask.Exception.InnerExceptions)
                {
                    ParseException parseException = (ParseException)e;
                    Debug.Log("ParseLogin: error message " + parseException.Message);
                    Debug.Log("ParseLogin: error code: " + parseException.Code);
                }
            }
            else
            {
                // Log in to Parse successful
                // Get user info
                FB.API("/me", HttpMethod.GET, FBAPICallback);
                // Display current profile info
                UpdateProfile();
                Debug.Log("UpdateProfile");
            }
        }
    }

    public void FBLogin()
    {
        // Logging in with Facebook
        //  FB.Login("user_about_me, user_relationships, user_birthday, user_location", FBLoginCallback);
        FB.Login("email, user_about_me, user_friends", FBLoginCallback);
    }

    private void FBLoginCallback(FBResult result)
    {
        Events.OnFacebookLogin();

        if (FB.IsLoggedIn)
        {
            StartCoroutine("ParseLogin");
            GetFriends();
        }
        else
        {
            Debug.Log("FBLoginCallback: User canceled login");
        }
    }

    public void ParseFBLogout()
    {
        FB.Logout();
        ParseUser.LogOut();
    }

    private void FBAPICallback(FBResult result)
    {
        if (!String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("FBAPICallback: Error getting user info: + " + result.Error);
            // Log the user out, the error could be due to an OAuth exception
            ParseFBLogout();
        }
        else
        {
            // Got user profile info
            var resultObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
            var userProfile = new Dictionary<string, string>();


            Data.Instance.userData.RegisterUser(getDataValueForKey(resultObject, "name"), getDataValueForKey(resultObject, "id"), getDataValueForKey(resultObject, "email"));

            userProfile["name"] = getDataValueForKey(resultObject, "name");

            StartCoroutine("saveUserProfile", userProfile);
        }
    }

    private IEnumerator saveUserProfile(Dictionary<string, string> profile)
    {
        var user = ParseUser.CurrentUser;
        // user["profile"] = profile;
        user["email"] = Data.Instance.userData.email;
        user["facebookID"] = Data.Instance.userData.facebookID;
        user["playerName"] = Data.Instance.userData.username;
        // Save if there have been any updates
        //if (user.IsKeyDirty("profile"))
        //{
        var saveTask = user.SaveAsync();
        while (!saveTask.IsCompleted) yield return null;
        UpdateProfile();
        //}
    }

    private string getDataValueForKey(Dictionary<string, object> dict, string key)
    {
        object objectForKey;
        if (dict.TryGetValue(key, out objectForKey))
        {
            return (string)objectForKey;
        }
        else
        {
            return "";
        }
    }

    private void UpdateProfile()
    {
        var user = ParseUser.CurrentUser;
    }

    // Wrap text by line height
    private string ResolveTextSize(string input, int lineLength)
    {

        // Split string by char " "    
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words     
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result   
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }
    public void GetFriends()
    {
        FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
    }
    void FBFriendsCallback(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            // Let's just try again
            FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, FBFriendsCallback);
            return;
        }
        Data.Instance.userData.ResetFacebookFriends();

        print("FBFriendsCallback");
        List<object> friends = Util.DeserializeJSONFriends(result.Text);

        foreach (object friend in friends)
        {
            Dictionary<string, object> friendData = friend as Dictionary<string, object>;
            Data.Instance.userData.AddFacebookFriend(friendData["id"].ToString(), friendData["first_name"].ToString());
        }

        Events.OnFacebookFriends();
    }
}
