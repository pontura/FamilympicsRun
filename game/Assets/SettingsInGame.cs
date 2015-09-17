using UnityEngine;
using System.Collections;

public class SettingsInGame : MonoBehaviour {

	void Start () {
        Events.OpenIngameMenu += OpenIngameMenu;
	}
    void OnDestroy()
    {
        Events.OpenIngameMenu -= OpenIngameMenu;
    }
    public void OpenIngameMenu()
    {
        Time.timeScale = 0;
        GetComponent<AnimationExtensions>().Play( GetComponent<Animation>().animation, "SettingsOpen", false, () => Debug.Log("onComplete") );
    }
    public void Close()
    {
        Time.timeScale = 1;
        GetComponent<Animation>().Play("SettingsClose");
    }
    public void Challenge()
    {
        Time.timeScale = 1;
        Data.Instance.Load("ChallengeCreator");
    }
    public void NextRace()
    {
        // si no hay suficiente energia
        if ( !Data.Instance.energyManager.ReplayCheck()) return;

        if ( !Data.Instance.levels.CanPlayNext() )
        {
            ExitToMap();
            return;
        }
        Data.Instance.levels.currentLevel++;
        Time.timeScale = 1;
        int totalPlayers = Data.Instance.multiplayerData.players.Count;
        if (totalPlayers > 1)
            Data.Instance.Load("Game");
        else Data.Instance.Load("GameSingle");
    }
    public void Restart()
    {
        // si no hay suficiente energia
        if ( !Data.Instance.energyManager.ReplayCheck()) return;

        Time.timeScale = 1;
        int totalPlayers = Data.Instance.multiplayerData.players.Count;
        if (totalPlayers > 1)
            Data.Instance.Load("Game");
        else Data.Instance.Load("GameSingle");
    }
    public void ExitToMap()
    {
        Time.timeScale = 1;
        Data.Instance.Load("LevelSelector");
    }
}
