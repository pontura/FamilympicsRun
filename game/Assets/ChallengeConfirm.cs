using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeConfirm : MonoBehaviour {

    public Text usernameLabel;
    public ProfilePicture profilePicture;

    private string username;
    private string facebookId;

	public void Init(string username, string facebookId) {
        this.username = username;
        this.facebookId = facebookId;
        usernameLabel.text = username.ToUpper();
        profilePicture.setPicture(facebookId);
	}
}
