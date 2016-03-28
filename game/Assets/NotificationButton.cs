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
    public GameObject accept;
    private bool selected;

    public void Init(NotificationsScene _creator, string playerName, string facebookID, string status)
    {
        this.creator = _creator;
        this.facebookID = facebookID;

        usernameLabel.text = Data.Instance.gameSettings.GetUsername(playerName);
        profilePicture.setPicture(facebookID);

        result.text = "";

        if (status == "0")
        {
            Send.SetActive(true);
            accept.SetActive(false);

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
            if (status == "1")
            {
                accept.SetActive(true);
            }
        }
        SetResult(status);
    }
    void SetResult(string status)
    {
        if (status == "1")
        {
            statusLabel.text = "SENT YOU ENERGY";
        } else if (status == "2")
        {
            statusLabel.text = "REJECTED";
            Cancel.SetActive(true);
            Send.SetActive(false);
            accept.SetActive(false);
            Cancel.GetComponent<Button>().onClick.AddListener(() =>
            {
                DeleteRejectedEnergyRequest();
            });


        } else if (status == "3")
        {
            statusLabel.text = "ready....";
            result.text = "";
        }
    }
    public void SendAction()
    {
        result.text = "SENT";
        ResetButtons();
        Events.SendEnergyTo(facebookID);
    }
    public void DeleteRejectedEnergyRequest()
    {
        Data.Instance.notifications.DeleteNotification(Data.Instance.userData.facebookID, facebookID);
        Destroy(gameObject);
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
    public void Accept()
    {
        if (Data.Instance.energyManager.energy < 10)
        {
            accept.SetActive(false);
            result.text = "ACCEPTED";
            Events.OnAcceptEnergyFrom(facebookID);
        }
        else
        {
            creator.AcceptFail();
        }
    }
}
