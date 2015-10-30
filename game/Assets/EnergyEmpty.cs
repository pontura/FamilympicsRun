using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyEmpty : MonoBehaviour
{
    public GameObject panel;
    private bool isActive;

    void Start()
    {
        panel.SetActive(false);
        Events.OnOpenEnergyPopup += OnOpenEnergyPopup;
    }
    void OnDestroy()
    {
        Events.OnOpenEnergyPopup -= OnOpenEnergyPopup;
    }
    public void OnOpenEnergyPopup()
    {
        if (Data.Instance.energyManager.energy > 0) return;

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
    public void Buy()
    {
        Data.Instance.Load("Buy");
    }
}
