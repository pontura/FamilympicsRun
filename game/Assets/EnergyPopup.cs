using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyPopup : MonoBehaviour {

    public Text dataField;
    public Text field;
    public GameObject panel;
    private bool isActive;

	void Start () {
        panel.SetActive(false);
	}
    public void Open()
    {
        isActive = true;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
        dataField.text = Data.Instance.energyManager.energy + "/" + Data.Instance.energyManager.MAX_ENERGY;
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
}
