using UnityEngine;
using System.Collections;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;

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
