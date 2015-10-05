using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyPopup : MonoBehaviour {

    public GameObject outOfEnergyIcon;
    public Text title;
    public Text dataField;
    public Text field;
    public GameObject panel;
    private bool isActive;

	void Start () {
        outOfEnergyIcon.SetActive(false);
        panel.SetActive(false);
        Events.OnOpenEnergyPopup += OnOpenEnergyPopup;
	}
    void OnDestroy()
    {
        Events.OnOpenEnergyPopup -= OnOpenEnergyPopup;
    }
    public void OnOpenEnergyPopup()
    {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        if (Data.Instance.energyManager.energy == 0)
        {
            outOfEnergyIcon.SetActive(true);
            title.text = "OUT OF ENERGY!";
        }
        else
        {
            title.text = "MY ENERGY";
        }

        isActive = true;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
        dataField.text = Data.Instance.energyManager.energy + "/" + Data.Instance.energyManager.MAX_ENERGY;
    }
    public void AskForEnergy()
    {
        if (FB.IsLoggedIn)
            Data.Instance.Load("EnergyAskFor");
        else
        {
            CloseOff();
            Events.OnFacebookNotConnected();
        }
    }
    public void Close()
    {
        panel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        isActive = false;
        panel.SetActive(false);
    }
	public void Update () {
        if (!isActive) return;

        field.text = Data.Instance.energyManager.timerString;
	}
    public void Refill()
    {
        Events.ReFillEnergy(5);
        Close();
    }
}
