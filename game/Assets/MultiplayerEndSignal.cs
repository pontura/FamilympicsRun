using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerEndSignal : MonoBehaviour {

    public CharacterFace characterFace;
    public GameObject panel;
    public Text field;
    private int playerID;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        Events.OnLevelComplete += OnLevelComplete;
	}
    void OnDestroy()
    {
        Events.OnLevelComplete -= OnLevelComplete;
    }
    void OnLevelComplete()
    {        
        Invoke("SetOn", 1f);
    }
    void SetOn()
    {
        playerID = Data.Instance.levelData.winnerID;
        characterFace.Init(playerID);

        string username = "";

        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            username = Data.Instance.multiplayerData.GetPlayer(playerID).username;
        else
            username = Data.Instance.userData.username;

        if(Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            field.text = "LEVEL COMPLETE!";
        else if (Data.Instance.multiplayerData.players.Count>1)
            field.text = "THE WINNER IS " + username + "!";

        Invoke("Reset", 3f);
    }
    void Reset()
    {
        Events.OpenSummary();
    }
}
