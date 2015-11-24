using UnityEngine;
using System.Collections;
using Parse;

public class PushBehaviorScript : MonoBehaviour
{

    bool tokenSent = false;
    public ParseObject currentInstallation = null;

    void Start()
    {
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
        if (!tokenSent && currentInstallation == null)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
				byte[] token   = NotificationServices.deviceToken;
				if(token != null) {
					tokenSent = true;
					string tokenString =  System.BitConverter.ToString(token).Replace("-", "").ToLower();
					Debug.Log ("OnTokenReived");
					Debug.Log (tokenString);
					ParseObject obj = new ParseObject("_Installation");
					obj["deviceToken"] = tokenString;
					obj["appIdentifier"] = "com.tipitap.taprun";
					obj["deviceType"] = "ios";
					obj["timeZone"] = "UTC";
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
}