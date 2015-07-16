using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyPopup : MonoBehaviour {

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
