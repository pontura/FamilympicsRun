using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardMenu : MonoBehaviour {

    public Text energyField;
    public Text notificationsField;

	void Start () {
        SetEnergy();
        Events.OnEnergyWon += OnEnergyWon;
        Events.OnRefreshNotifications += OnRefreshNotifications;
        OnRefreshNotifications( Data.Instance.notifications.FriendsThatGaveYouEnergy.Count);
	}
    void OnDestroy()
    {
        Events.OnEnergyWon -= OnEnergyWon;
        Events.OnRefreshNotifications -= OnRefreshNotifications;
    }
    void OnRefreshNotifications(int totalRequestedNotifications)
    {
        notificationsField.text = totalRequestedNotifications.ToString();
    }
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
}
