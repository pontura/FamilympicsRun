using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationButton : MonoBehaviour {


    public string facebookID;
    public Text usernameLabel;
    public Text statusLabel;
    public Text result;
    public ProfilePicture profilePicture;

    private NotificationsScene creator;
    public GameObject Send;
    public GameObject Cancel;
    private bool selected;

    public void Init(NotificationsScene _creator, string playerName, string facebookID, string status)
    {
        this.creator = _creator;
        this.facebookID = facebookID;

        usernameLabel.text = playerName.ToUpper();
        profilePicture.setPicture(facebookID);

        result.text = "";

        if (status == "0")
        {
            Send.SetActive(true);

            Send.GetComponent<Button>().onClick.AddListener(() =>
            {
                SendAction();
            });
            Cancel.GetComponent<Button>().onClick.AddListener(() =>
            {
                CancelAction();
            });
        } 
        else
        {
            Send.SetActive(false);
            Cancel.SetActive(false);
        }
        SetResult(status);
    }
    void SetResult(string status)
    {
        if(status == "1")
            result.text = "SENDED";
        if (status == "2")
            result.text = "REJECTED";
    }
    public void SendAction()
    {
        result.text = "SENDED";
        ResetButtons();
        Events.SendEnergyTo(facebookID);
    }
    public void CancelAction()
    {
        result.text = "REJECTED";
        ResetButtons();
        Events.RejectEnergyTo(facebookID);
    }
    void ResetButtons()
    {
        Send.SetActive(false);
        Cancel.SetActive(false);
    }
}
