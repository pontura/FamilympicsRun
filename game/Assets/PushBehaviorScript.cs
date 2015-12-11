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
    public void OnChallengeCreate(string facebookFriendName, string facebookFriendId, int levelId, float score)
    {
        print("______SEND: " + facebookFriendName + " " + facebookFriendId + " " + levelId + " " + score );
        //ParseCloud.CallFunctionAsync<string>("hello", new Dictionary<string, object>()).ContinueWith(t =>
        // System.Console.WriteLine("received: " + t.Result);
        //  print("____" + t.Result);
        // );
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("facebookFriendName", facebookFriendName);
        dic.Add("facebookFriendId", facebookFriendId);
        dic.Add("levelId", levelId);
        dic.Add("score", score);

         ParseCloud.CallFunctionAsync<string>("challengeUpdate", dic).ContinueWith(t =>
            Debug.Log("received: " + t.Result));
    }
}