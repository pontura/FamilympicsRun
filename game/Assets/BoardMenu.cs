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
        challengesReceived = false;
        notificationsReceived = false;
	}

    void OnDestroy()
    {
        Events.OnEnergyWon -= OnEnergyWon;
         Events.OnResetApp -= OnResetApp;
    }

    private int totalRequestedNotifications = 0;
    private bool challengesReceived;
    private bool notificationsReceived;

    void Update()
    {
        if (!notificationsReceived)
        {
            if ((Data.Instance.notifications.notifications.Count + Data.Instance.notifications.notificationsReceived.Count) != totalRequestedNotifications)
            {
                
                totalRequestedNotifications = Data.Instance.notifications.notifications.Count + Data.Instance.notifications.notificationsReceived.Count;
                if (totalRequestedNotifications > 0)
                {
                    notificationsField.text = totalRequestedNotifications.ToString();
                    notificationsReceived = true;
                }
                else notificationsField.text = "";
            }
        }
        if (!challengesReceived)
        {
            int newChallenges = Data.Instance.challengesManager.GetNewChallenges();
            if (newChallenges > 0)
            {
                if (newChallenges > 0)
                {
                    challengesField.text = newChallenges.ToString();
                    challengesReceived = true;
                }
                else challengesField.text = "";
            }
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
