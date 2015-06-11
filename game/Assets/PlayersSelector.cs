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
    public void Selected(int num)
    {
        Data.Instance.levelData.numPlayers = num;
        Application.LoadLevel("Game");
    }
    public void Challenge()
    {
        Application.LoadLevel("ChallengeCreator");
    }
    public void Back()
    {
        Application.LoadLevel("LevelSelector");
    }
}
