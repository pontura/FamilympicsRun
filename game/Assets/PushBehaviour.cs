using UnityEngine;
using System.Collections;
#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

public class PushBehaviour : MonoBehaviour {

    void Awake()
    {
#if UNITY_IOS
NotificationServices.RegisterForNotifications(
             NotificationType.Alert |
             NotificationType.Badge |
             NotificationType.Sound);
#endif
    }
}
