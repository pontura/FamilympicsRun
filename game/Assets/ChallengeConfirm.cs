using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeConfirm : MonoBehaviour {

    public Text usernameLabel;

	void Start () {
        usernameLabel.text = Data.Instance.levelData.challenge_username;
	}
	
	public void StartGame () {
        Application.LoadLevel("Game");
	}
    public  void Back()
    {
        Application.LoadLevel("ChallengeCreator");
    }
}
