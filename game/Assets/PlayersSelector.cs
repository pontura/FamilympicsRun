using UnityEngine;
using System.Collections;

public class PlayersSelector : MonoBehaviour {

    public void Selected(int num)
    {
        Data.Instance.levelData.numPlayers = num;
        Application.LoadLevel("Game");
    }
    public void Challenge()
    {
        Application.LoadLevel("ChallengeCreator");
    }
}
