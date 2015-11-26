using System;
using System.Collections.Generic;
using UnityEngine;


public class FacebookShare : MonoBehaviour {

    string linkName = "Running";

    public void ShareToFriend(string friend_facebookID, string linkCaption)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("ShareToFriend: " + friend_facebookID + " linkCaption: " + linkCaption + " linkName: " + linkName);

            FB.Feed(
                toId: friend_facebookID,
                linkCaption: linkCaption,
                //  picture: "<INSERT A LINK TO A PICTURE HERE>",
                linkName: linkName,
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                );
        }
    }

    public void MultiplayerHiscore(string score)
    {
        if (FB.IsLoggedIn)
        {
            string linkCaption = "New Multiplayer hiscore: " + score;

            FB.Feed(
                linkCaption: linkCaption,
                //  picture: "<INSERT A LINK TO A PICTURE HERE>",
                linkName: linkName,
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                );
        }
    }

    //public void WinChallengeTo(string challenge_username)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        string  linkCaption = "You Won the challenge to " + challenge_username;

    //        FB.Feed(
    //            linkCaption: linkCaption,
    //            //  picture: "<INSERT A LINK TO A PICTURE HERE>",
    //            linkName: linkName,
    //            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
    //            );
    //    }
    //}
    //public void LostChallengeTo(string challenge_username)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        string linkCaption = "You lost the challenge with " + challenge_username;

    //        FB.Feed(
    //            linkCaption: linkCaption,
    //            //  picture: "<INSERT A LINK TO A PICTURE HERE>",
    //            linkName: linkName,
    //            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
    //            );
    //    }
    //}
    public void NewHiscore(string linkCaption)
    {
        if (FB.IsLoggedIn)
        {
            FB.Feed(
                linkCaption: linkCaption,
                //  picture: "<INSERT A LINK TO A PICTURE HERE>",
                linkName: linkName,
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                );
        }
    }
}
