using UnityEngine;
using System.Collections;

public class PushBehaviour : MonoBehaviour {

    void Awake()
    {
#if UNITY_IOS
  NotificationServices.RegisterForRemoteNotificationTypes(RemoteNotificationType.Alert |
                                                          RemoteNotificationType.Badge |
                                                          RemoteNotificationType.Sound);
#endif
    }
}
