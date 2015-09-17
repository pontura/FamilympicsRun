using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerButton : MonoBehaviour {

    public GameObject editButton;
    public Image image;
    public Text field;
    public int id;
    public bool selected;
    private MultiplayerData multiplayerData;
    private string username;

	public void Init () {
        multiplayerData = Data.Instance.multiplayerData;
        image.color = Data.Instance.colors[id-1];
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Toogle();
        });
        foreach(MultiplayerData.PlayerData playerData in Data.Instance.multiplayerData.players)
        {
            if (playerData.playerID == id)
            {
                username = playerData.username;
                if(username != "")
                    field.text = username;
                IsOn();
                return;
            }
        }
        switch (id)
        {
            case 1: username = Data.Instance.multiplayerData.playerName1; break;
            case 2: username = Data.Instance.multiplayerData.playerName2; break;
            case 3: username = Data.Instance.multiplayerData.playerName3; break;
            case 4: username = Data.Instance.multiplayerData.playerName4; break;
        }

        if (username != "")
            field.text = username;
        else
            field.text = "RUN" + id;    

        IsOff();
	}
    void Toogle()
    {
        Events.OnSoundFX("buttonPress");
        if (!selected)
        {
            IsOn();
            multiplayerData.AddPlayer(id);
        }
        else
        {
            IsOff();
            multiplayerData.DeletePlayer(id);
        }
    }
    void IsOn()
    {
        selected = true;
        Color newColor = image.color;
        newColor.a = 1;
        image.color = newColor;
        Color newTextColor = Color.white;
        field.color = newTextColor;
        editButton.SetActive(true);
    }
    void IsOff()
    {
        selected = false;
        Color newColor = image.color;
        newColor.a = 0.25f;
        image.color = newColor;
        Color newTextColor = Color.white;
        newTextColor.a = 0.2f;
        field.color = newTextColor;
        editButton.SetActive(false);
    }

}
