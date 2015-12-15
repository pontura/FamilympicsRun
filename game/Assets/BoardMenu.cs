using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardMenu : MonoBehaviour {

    public Text energyField;
    public Text notificationsField;
    public Text challengesField;

	void Start () {
        SetEnergy();
        Events.OnEnergyWon += OnEnergyWon;

        notificationsField.text = "";
        challengesField.text = "";
        Events.OnResetApp += OnResetApp;
        //OnRefreshNotifications( Data.Instance.notifications.FriendsThatGaveYouEnergy.Count);
	}
    void OnDestroy()
    {
        Events.OnEnergyWon -= OnEnergyWon;
         Events.OnResetApp -= OnResetApp;
    }

    private int totalRequestedNotifications = 0;
    void Update()
    {
        if ((Data.Instance.notifications.notifications.Count + Data.Instance.notifications.notificationsReceived.Count) != totalRequestedNotifications)
        {
            totalRequestedNotifications = Data.Instance.notifications.notifications.Count + Data.Instance.notifications.notificationsReceived.Count;
            if (totalRequestedNotifications > 0)
                notificationsField.text = totalRequestedNotifications.ToString();
            else notificationsField.text = "";
        }
        if (Data.Instance.challengesManager.received.Count>0)
        {
            int num = Data.Instance.challengesManager.received.Count;
            if (num > 0)
                challengesField.text = num.ToString();
            else challengesField.text = "";
        }
    }
    //void OnRefreshNotifications(int totalRequestedNotifications)
    //{
    //    notificationsField.text = totalRequestedNotifications.ToString();
    //}
    void OnEnergyWon()
    {
        SetEnergy();
    }
    void SetEnergy()
    {
        int totalEnergy = Data.Instance.energyManager.ENERGY_TO_START;
        int energy = Data.Instance.energyManager.energy;
        energyField.text = energy + "/" + totalEnergy;
    }
    public void OpenEnergyPopup()
    {
        Events.OnOpenEnergyPopup();
    }
    void OnResetApp()
    {
        challengesField.text = "";
        notificationsField.text = "";
    }
}
