using UnityEngine;
using System.Collections;

public class TournamentNotEnoughEnergy : MonoBehaviour {

    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }
    public void Open()
    {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
    }
    public void Buy()
    {
        Data.Instance.Load("Buy");
    }
    public void Close()
    {
        panel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        panel.SetActive(false);
    }
}
