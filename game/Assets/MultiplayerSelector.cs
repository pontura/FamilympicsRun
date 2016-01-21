using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplayerSelector : MonoBehaviour {

    public Button playButton;

    public GameObject playMulti;
    public GameObject playTournament;

    public PlayerButton[] playerButtons;
    private int numberOfPlayers = 0;
    

    void Start()
    {
       // Data.Instance.multiplayerData.ResetPlayers();
        foreach(PlayerButton playerButton in playerButtons)
        {
            playerButton.Init();
        }
        playButton.interactable = false;
        if (Data.Instance.tournament.isOn)
        {
            playTournament.SetActive(true);
            playMulti.SetActive(false);
        }
        else
        {
            playTournament.SetActive(false);
            playMulti.SetActive(true);
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
            if (playButton.interactable == false)
            {
                playButton.GetComponent<Animation>().Play("PlayButtonOn");
                playButton.interactable = true;
                playButton.Select();
            }

        }
        else
        {
            playButton.GetComponent<Animation>().Stop();
           playButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            playButton.interactable = false;
        }
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
    }
    public void Back()
    {
        Events.OnSoundFX("buttonPress");
        Data.Instance.Load("LevelSelector");
    }
}
