using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfileModule : MonoBehaviour {

    public Text usernameLabel;
    public ProfilePicture profilePicture;

    void Start()
    {
        if (Data.Instance.userData.logged)
            SetOn();
        else
            SetOff();
    }
    public void SetOn()
    {
        usernameLabel.text = Data.Instance.userData.username;
        profilePicture.setPicture(Data.Instance.userData.facebookID);
        usernameLabel.gameObject.SetActive(true);
        profilePicture.gameObject.SetActive(true);
	}
    public void SetOff()
    {
        usernameLabel.gameObject.SetActive(false);
        profilePicture.gameObject.SetActive(false);
    }
}
