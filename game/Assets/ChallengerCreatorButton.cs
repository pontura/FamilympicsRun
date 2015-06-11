using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengerCreatorButton : MonoBehaviour {

    public string facebookID;
    public Text usernameLabel;
    public ProfilePicture profilePicture;
    public int id = 0;
    private ChallengerCreator creator;
    public bool infoLoaded;

    public void Init(ChallengerCreator _creator,  int _id)
    {
        this.creator = _creator;
        this.id = _id;
        Vector3 pos = transform.localPosition;
        pos.x = 1000;
        transform.localPosition = pos;
	}
    void Update()
    {
        if (infoLoaded) return;
        if (creator.userData[id].facebookID != "")
        {
            usernameLabel.text = creator.userData[id].playerName;
            profilePicture.setPicture(creator.userData[id].facebookID);
            infoLoaded = true;

            Vector3 pos = transform.localPosition;
            pos.x = 0;
            transform.localPosition = pos;

            GetComponent<Button>().onClick.AddListener(() =>
            {
                creator.Challenge(creator.userData[id].playerName, creator.userData[id].facebookID);
            });

        }
    }
}
