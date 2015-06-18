using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersSelector : MonoBehaviour {

    public Button challengeButton;

    void Start()
    {
        if (Data.Instance.userData.facebookID == "")
            challengeButton.interactable = false;
    }
    public void Challenge()
    {
        Data.Instance.Load("ChallengeCreator");
    }
    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void SinglePlayer()
    {
        Data.Instance.Load("Game");
    }
}
