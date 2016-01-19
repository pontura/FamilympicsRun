using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.Unity;

public class EnergyPopup : MonoBehaviour {

    public GameObject outOfEnergyIcon;
    public GameObject plusEnergyIcon;

    public Text plusEnergyLabel;
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
       plusEnergyIcon.SetActive(false);
        panel.transform.localScale = Data.Instance.screenManager.scale;
        if (Data.Instance.energyManager.energy == 0)
        {
            if (Data.Instance.energyManager.plusEnergy < 1)
            {
                outOfEnergyIcon.SetActive(true);
                title.text = "OUT OF ENERGY!";
            }
            else
            {
                Data.Instance.energyManager.ConsumePlusEnergy();
            }
        }
        else
        {
            title.text = "MY ENERGY";
        }
        if (Data.Instance.energyManager.plusEnergy > 1)
            plusEnergyIcon.SetActive(true);

        isActive = true;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
        dataField.text = Data.Instance.energyManager.energy + "/" + Data.Instance.energyManager.MAX_ENERGY;
        SetPlusLabel();
    }
    void SetPlusLabel()
    {
        plusEnergyLabel.text = Data.Instance.energyManager.plusEnergy + "x";
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
        Data.Instance.Load("Buy");
    }
}
