using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersSelector : MonoBehaviour {

    public Button challengeButton;

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
