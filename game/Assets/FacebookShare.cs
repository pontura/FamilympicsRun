using System;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;


public class FacebookShare : MonoBehaviour {

    string linkName = "Running";
    private string picture_URL = "http://tipitap.com/running-icon.jpg";


    public void ShareToFriend(string friend_facebookID, string linkCaption)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("ShareToFriend: " + friend_facebookID + " linkCaption: " + linkCaption + " linkName: " + linkName);

            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            //FB.FeedShare(
            //    toId: friend_facebookID,
            //    linkCaption: linkCaption,
            //    linkName: linkName,
            //    link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest"),
            //    picture: picture_URL
            //    );
        }
    }

    public void MultiplayerHiscore(string score)
    {
        if (FB.IsLoggedIn)
        {
            string linkCaption = "New Multiplayer hiscore: " + score;

            //FB.FeedShare(
            //    linkCaption: linkCaption,
            //    //  picture: "<INSERT A LINK TO A PICTURE HERE>",
            //    linkName: linkName,
            //    link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest"),
            //    picture: picture_URL
            //    );
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
            //FB.FeedShare(
            //    linkCaption: linkCaption,
            //    //  picture: "<INSERT A LINK TO A PICTURE HERE>",
            //    linkName: linkName,
            //    link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest"),
            //    picture: picture_URL
            //    );
        }
    }
}
