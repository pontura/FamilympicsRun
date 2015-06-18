using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplayerSelector : MonoBehaviour {

    public Button playButton;
    public PlayerButton[] playerButtons;

    void Start()
    {
        Data.Instance.multiplayerData.ResetPlayers();
        foreach(PlayerButton playerButton in playerButtons)
        {
            playerButton.Init();
        }
    }
    void Update()
    {
        if( playerButtons.Length==0) return;
        int numberOfPlayers = 0;

        foreach (PlayerButton playerButton in playerButtons)
        {
            if (playerButton.selected) numberOfPlayers++;
        }
        if (numberOfPlayers > 1)
            playButton.interactable = true;
        else  
            playButton.interactable = false;
    }
    public void StartGame()
    {
        Data.Instance.Load("Game");
    }
    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
}
