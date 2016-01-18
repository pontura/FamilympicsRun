using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Parse;

public class PushBehaviorScript : MonoBehaviour
{

    bool tokenSent = false;
    public ParseObject currentInstallation = null;

    void Start()
    {
        Events.OnChallengeCreate += OnChallengeCreate;
        Events.OnChallengeRemind += OnChallengeRemind;
        Events.OnChallengeClose += OnChallengeClose;
        Events.SendNotificationTo += SendNotificationTo;
        Events.SendEnergyTo += SendEnergyTo;

        if (PlayerPrefs.HasKey("currentInstallation"))
        {
            string objId = PlayerPrefs.GetString("currentInstallation");
            currentInstallation = ParseObject.CreateWithoutData("_Installation", objId);
        }

        if (currentInstallation == null)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
				NotificationServices.RegisterForRemoteNotificationTypes (RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound);
#endif
        }
    }

    void FixedUpdate()
    {
        if (!tokenSent && currentInstallation == null && Data.Instance.userData.facebookID != "")
        {

            
#if UNITY_IPHONE && !UNITY_EDITOR
				byte[] token   = NotificationServices.deviceToken;
				if(token != null) {
                    print("___________PARSE SOS: " + Data.Instance.userData.facebookID);
					tokenSent = true;
					string tokenString =  System.BitConverter.ToString(token).Replace("-", "").ToLower();
					Debug.Log ("OnTokenReived");
					Debug.Log (tokenString);
					ParseObject obj = new ParseObject("_Installation");
					obj["deviceToken"] = tokenString;
					obj["appIdentifier"] = "com.tipitap.taprun";
					obj["deviceType"] = "ios";
					obj["timeZone"] = "UTC";
                    obj["facebookID"] = "id_" + Data.Instance.userData.facebookID;
					obj["appName"] = "Running";
					obj["appVersion"] = "1.0.0";
					obj["parseVersion"] = "1.3.0";
					obj.SaveAsync().ContinueWith(t =>
		            {
						if (obj.ObjectId != null) {
							PlayerPrefs.SetString ("currentInstallation", obj.ObjectId);
						}
					});
				}
#endif
        }
    }


    //Events.OnChallengeCreate(facebookFriendName, facebookFriendId, levelId, score);
    void OnChallengeCreate(string facebookFriendName, string facebookFriendId, int levelId, float score)
    {
        print("______SEND: " + facebookFriendName + " " + facebookFriendId + " " + levelId + " " + score );
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("facebookFriendName", facebookFriendName);
        dic.Add("facebookFriendId", facebookFriendId);
        dic.Add("levelId", levelId);
        dic.Add("score", score);
        dic.Add("username", Data.Instance.userData.username);

         ParseCloud.CallFunctionAsync<string>("challengeUpdate", dic).ContinueWith(t =>
            Debug.Log("received: " + t.Result));
    }
    void OnChallengeClose(string objectID, string op_facebookID, string winner, float newScore)
    {
        if (winner == Data.Instance.userData.facebookID)
            OnChallengeWin(Data.Instance.userData.username, op_facebookID);
        else
            OnChallengeLost(Data.Instance.userData.username, op_facebookID);
    }
    void OnChallengeWin(string username, string facebookFriendId)
    {
        print("______SEND challengeBeat: " + username + " " + facebookFriendId);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("username", username);
        dic.Add("facebookFriendId", facebookFriendId);

        ParseCloud.CallFunctionAsync<string>("challengeBeatYou", dic).ContinueWith(t =>
           Debug.Log("received: " + t.Result));
    }
    void OnChallengeLost(string username, string facebookFriendId)
    {
        print("______SEND OnChallengeLost: " + username + " " + facebookFriendId);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("username", username);
        dic.Add("facebookFriendId", facebookFriendId);

        ParseCloud.CallFunctionAsync<string>("challengeLost", dic).ContinueWith(t =>
           Debug.Log("received: " + t.Result));
    }
    void OnChallengeRemind(string objectID, string facebookFriendId)
    {
        print("_________OnChallengeRemind: " + facebookFriendId);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("facebookFriendId", facebookFriendId);
        dic.Add("username", Data.Instance.userData.username);

        ParseCloud.CallFunctionAsync<string>("challengeRemind", dic).ContinueWith(t =>
           Debug.Log("received: " + t.Result));
    }

    void SendNotificationTo(string facebookFriendId, string username)
    {
        print("_________OnChallengeRemind: " + facebookFriendId);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("facebookFriendId", facebookFriendId);
        dic.Add("username", Data.Instance.userData.username);

        ParseCloud.CallFunctionAsync<string>("sendNotificationTo", dic).ContinueWith(t =>
           Debug.Log("received: " + t.Result));
    }
    void SendEnergyTo(string facebookFriendId)
    {
        string username = Data.Instance.userData.GetUsernameByFacebookID(facebookFriendId);
        print("_________OnChallengeRemind: " + facebookFriendId + " username: " + username);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("facebookFriendId", facebookFriendId);
        dic.Add("username", Data.Instance.userData.username);

        ParseCloud.CallFunctionAsync<string>("sentYouEnergy", dic).ContinueWith(t =>
           Debug.Log("received: " + t.Result));
    }
}