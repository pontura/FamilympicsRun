using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

    public Button challengesButton;

	void Start () {
        if (Data.Instance.userData.facebookID == "")
            challengesButton.interactable = false;
	}

    public void Open()
    {
        animation.Play("SettingsOpen");
    }
    public void Close()
    {
        animation.Play("SettingsClose");
    }
}
