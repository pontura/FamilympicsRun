using UnityEngine;
using System.Collections;

public class TournamentEndingPopup : MonoBehaviour {

    public GameObject panel;

    void Start()
    {
        Events.OnTournamentFinishAskForConfirmation += OnTournamentFinishAskForConfirmation;
        panel.SetActive(false);
    }
    void OnDestroy()
    {
        Events.OnTournamentFinishAskForConfirmation -= OnTournamentFinishAskForConfirmation;
    }
    void OnTournamentFinishAskForConfirmation()
    {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
    }
    public void Quit()
    {
        Close();
        Events.OnTournamentFinish();
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
