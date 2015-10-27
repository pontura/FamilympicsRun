using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengerCreatorButton : MonoBehaviour {

    public string facebookID;
    public Text usernameLabel;
    public ProfilePicture profilePicture;
    public int id = 0;
    private ChallengerCreator creator;
    public GameObject SendButton;
    public GameObject RemindButton;

    private string playerName;

    public void Init(ChallengerCreator _creator, int _id, string playerName, string facebookID, bool done)
    {
        this.playerName = playerName;
        this.facebookID = facebookID;

        RemindButton.SetActive(false);
        this.creator = _creator;
        this.id = _id;

        usernameLabel.text = playerName.ToUpper();
        profilePicture.setPicture(facebookID);

        if (done)
        {
            SendButton.SetActive(false);
            RemindButton.SetActive(true);

            GetComponent<Button>().onClick.AddListener(() =>
            {
                Remind();
            });

        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                creator.Challenge(playerName, facebookID);
            });
        }
    }
    void Remind()
    {
        RemindButton.SetActive(false);
        creator.Remind(playerName, facebookID);
    }
}
