using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameEditor : MonoBehaviour {

    private MultiplayerData multiPlayerData;
    public Image background;
    public Text label;
    public InputField input;


	void Start () {
        
        multiPlayerData = Data.Instance.multiplayerData;
        string username =  multiPlayerData.GetPlayer(multiPlayerData.activePlayerId).username;

        if(username != "")
            input.text = username;
        else
            label.text = "RUN" + multiPlayerData.activePlayerId;

        Color color = multiPlayerData.GetPlayer(multiPlayerData.activePlayerId).color;
        background.color = color;

        input.characterLimit = 4;
	}
    public void Back()
    {
        Data.Instance.Load("Players");
    }
	public void Send () {

        Events.OnSoundFX("buttonPress");
        if(label.text != "")
            multiPlayerData.OnSaveName(label.text, multiPlayerData.activePlayerId);

        Data.Instance.Back();
	}
}
