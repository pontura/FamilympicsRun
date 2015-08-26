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

    public void Init(ChallengerCreator _creator, int _id, string playerName, string facebookID, bool done)
    {
        this.creator = _creator;
        this.id = _id;

        usernameLabel.text = playerName;
        profilePicture.setPicture(facebookID);

        if (done)
            SendButton.SetActive(false);
        else
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                creator.Challenge(playerName, facebookID);
            });
        }
    }
}
