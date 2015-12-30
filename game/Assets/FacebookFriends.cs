using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FacebookFriends : MonoBehaviour {

    [Serializable]
    public class Friend
    {
        public string id;
        public Texture2D picture;
    }
    public IList<string> ids;
    public List<Friend> all;

	void Start () {
        ids = new List<string>();
        Events.AddFacebookFriend += AddFacebookFriend;
        Events.OnFacebookInviteFriends += OnFacebookInviteFriends;
	}
    void OnFacebookInviteFriends()
    {
        FB.AppRequest(
            "Running!", null, null, null, null, "Come and play Running!", null
        );
    }
    void AddFacebookFriend(string id, string username)
    {
        ids.Add(id);
        Friend friend = new Friend();
        friend.id = id;
        all.Add(friend);
        StartCoroutine(GetPicture(id));
    }
    IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "")
            yield break;

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
            SetProfilePicture(facebookID, receivedData.texture);
    }
    public void SetProfilePicture(string facebookID, Texture2D picture)
    {
        foreach (Friend friend in all)
        {
            if (friend.id == facebookID)
                friend.picture = picture;
        }
    }
    public Texture2D GetProfilePicture(string facebookID)
    {
        foreach (Friend friend in all)
        {
            if (friend.id == facebookID)
                return friend.picture;
        }
        return null;
    }
}
