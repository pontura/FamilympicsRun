using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeConfirm : MonoBehaviour {

    public Text usernameLabel;

	void Start () {
        usernameLabel.text = Data.Instance.levelData.challenge_username;
	}
	
	public void StartGame () {
        Data.Instance.Load("Game");
	}
    public  void Back()
    {
        Data.Instance.Back();
    }
}
