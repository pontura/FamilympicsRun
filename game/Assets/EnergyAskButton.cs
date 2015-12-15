using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyAskButton : MonoBehaviour {

    public string facebookID;
    public Text usernameLabel;
    public ProfilePicture profilePicture;
    public int id = 0;
    private EnergyAskFor creator;
    public GameObject SendButton;
    public GameObject SendedButton;
    private bool selected;

    public void Init(EnergyAskFor _creator, int _id, string playerName, string facebookID, bool done)
    {
        this.creator = _creator;
        this.id = _id;

        usernameLabel.text = playerName.ToUpper();
        profilePicture.setPicture(facebookID);

        if (done)
        {
            SendedButton.SetActive(true);
            SendButton.SetActive(false);
        }
        else
        {
            SendedButton.SetActive(false);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (selected) return;
                selected = true;
                SendButton.SetActive(false);
                SendedButton.SetActive(true);
                creator.Select(facebookID, playerName);
            });
        }
    }
}
