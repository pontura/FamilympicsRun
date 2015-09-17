using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplayerSelector : MonoBehaviour {

    public Button playButton;
    public PlayerButton[] playerButtons;
    private int numberOfPlayers = 0;

    void Start()
    {
       // Data.Instance.multiplayerData.ResetPlayers();
        foreach(PlayerButton playerButton in playerButtons)
        {
            playerButton.Init();
        }
    }
    void Update()
    {
        if( playerButtons.Length==0) return;

        numberOfPlayers = 0;
        foreach (PlayerButton playerButton in playerButtons)
        {
            if (playerButton.selected) numberOfPlayers++;
        }
        if (numberOfPlayers > 1)
        {
            playButton.interactable = true;
            playButton.Select();
        }
        else
            playButton.interactable = false;
    }
    public void Edit(int id)
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.multiplayerData.activePlayerId = id;
        Data.Instance.Load("NameEditor");
    }
    public void StartGame()
    {
        Events.OnSoundFX("raceStart");

        if (numberOfPlayers > 1)
            Data.Instance.Load("Game");
        else
        {
            Data.Instance.userData.mode = UserData.modes.SINGLEPLAYER;
            Data.Instance.Load("GameSingle");
        }
    }
    public void Back()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }
}
