using System;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;


public class FacebookShare : MonoBehaviour {

    string linkName = "Running";
    private string feedLink = "https://itunes.apple.com/us/app/running-tap-challenges/id987539773?ls=1&mt=8";
    private string feedTitle = "Running!";
    private string feedCaption = "Running Caption";
    private string feedDescription = "Running is an amazing — fast-tapping — game. Run as fast as you can by tapping your fingers on the screen (left, right, left, right!!!) Short races, marathons, treadmill lanes, sudden death, tournaments, obstacles and powerups! Challenge your friends! In multiplayer mode, up to 4 players compete TOGETHER, SIMULTANEOUSLY on the SAME device!";
    private string feedImage = "http://tipitap.com/running-icon.jpg";
    private string feedMediaSource = string.Empty;


    public void ShareToFriend(string friend_facebookID, string linkCaption)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("ShareToFriend: " + friend_facebookID + " linkCaption: " + linkCaption + " linkName: " + linkName);
            
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string feedLink = "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest");

            FB.FeedShare(
                    friend_facebookID,
                    string.IsNullOrEmpty(this.feedLink) ? null : new Uri(feedLink),
                    linkName,
                    linkCaption,
                    this.feedDescription,
                    string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                    this.feedMediaSource,
                    this.HandleResult);
        }
    }

    public void MultiplayerHiscore(string score)
    {
        if (FB.IsLoggedIn)
        {
            string linkCaption = "Hiscore: " + score;

            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string feedLink = "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest");


            FB.ShareLink(
                   new Uri(feedLink),
                    "New Multiplayer Hiscore",
                   linkCaption,
                   string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                   this.HandleResult);
        }
    }

    public void WinChallengeTo(string challenge_username)
    {
        if (FB.IsLoggedIn)
        {
            string linkCaption = "You Won the challenge to " + challenge_username;

            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string feedLink = "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest");


            FB.ShareLink(
                   new Uri(feedLink),
                   "Challenges",
                   linkCaption,
                   string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                   this.HandleResult);
        }
    }
    public void LostChallengeTo(string challenge_username)
    {

        if (FB.IsLoggedIn)
        {
            string linkCaption = "You lost the challenge with " + challenge_username;

            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string feedLink = "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest");


            FB.ShareLink(
                   new Uri(feedLink),
                   "Challenges",
                   linkCaption,
                   string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                   this.HandleResult);
        }

    }
    public void NewHiscore(string linkCaption)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string feedLink = "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? aToken.UserId : "guest");


            FB.ShareLink(
                   new Uri(feedLink),
                   "New Hiscore",
                   linkCaption,
                   string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                   this.HandleResult);
        }

    }

    protected void HandleResult(IResult result)
    {
        Debug.Log(result);
        if (result == null)
        {

        }
    }
}
