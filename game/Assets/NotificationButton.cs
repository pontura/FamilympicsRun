using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationButton : MonoBehaviour {

    public string facebookID;
    public Text usernameLabel;
    public Text statusLabel;
    public ProfilePicture profilePicture;

    private NotificationsScene creator;
    public GameObject Send;
    public GameObject Cancel;
    private bool selected;

    public void Init(NotificationsScene _creator, string playerName, string facebookID, string status)
    {
        this.creator = _creator;

        usernameLabel.text = playerName.ToUpper();
        profilePicture.setPicture(facebookID);

        if (status == "0")
            Send.SetActive(true);
        else
        {
            Send.SetActive(false);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (selected) return;
                selected = true;
                Send.SetActive(true);
                creator.Select(facebookID);
            });
        }
    }
}
