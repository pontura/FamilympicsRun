using UnityEngine;
using System.Collections;

public class ConfirmReset : MonoBehaviour {

    public SettingsMenu menu;
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }
    public void Open()
    {
        menu.Close();
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
    }
    public void ResetApp()
    {
        Events.ResetApp();
        Data.Instance.Load("MainMenu");
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
