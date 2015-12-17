using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Powerups : MonoBehaviour {

    private int timeToReset = 2;
    public int playerId;
    public int powerUpActive;
    public types type;
    public Image[] toColorize;
    public GameObject panel;
    public GameObject tutorialPanel;
    public Text tutorialField;

    [SerializeField]
    public Image bar;
    public GameObject BoostAsset;
    public GameObject PauseMeAsset;
    public GameObject PauseOthersAsset;

    private bool isOn;

    public enum types
    {
        BOOST,
        PAUSE_ME,
        PAUSE_OTHERS
    }
    public void Init(int playerId)
    {
        panel.SetActive(false);
        this.playerId = playerId;
        Color colorID = Data.Instance.colors[playerId - 1];
        colorID.a = 0.5f;
        foreach (Image image in toColorize)
            image.color = colorID;
    }
	void Start () {
        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
        {
            Events.OnPowerUpOn += OnPowerUpOn;
            Events.OnPowerUpActive += OnPowerUpActive;
        }
	}
    void OnDestroy()
    {
        Events.OnPowerUpOn -= OnPowerUpOn;
        Events.OnPowerUpActive -= OnPowerUpActive;
    }
    void OnPowerUpActive(int playerID, types type)
    {
        Reset();
    }
    void Reset()
    {
        bar.gameObject.SetActive(false);
        BoostAsset.gameObject.SetActive(false);
        PauseMeAsset.gameObject.SetActive(false);
        PauseOthersAsset.gameObject.SetActive(false);
        panel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
    void OnPowerUpOn(int powerUpActive)
    {
       
        tutorialPanel.SetActive(true);
        if (playerId > 2)
            tutorialPanel.transform.localScale = new Vector3(-1, 1, 1);

        panel.SetActive(true);
        bar.gameObject.SetActive(true);
        switch (powerUpActive)
        {
            case 1: type = types.BOOST; BoostAsset.gameObject.SetActive(true);              tutorialField.text = "BOOST";       break;
            case 2: type = types.PAUSE_ME; PauseMeAsset.gameObject.SetActive(true);         tutorialField.text = "DON'T TAP";   break;
            case 3: type = types.PAUSE_OTHERS; PauseOthersAsset.gameObject.SetActive(true); tutorialField.text = "TROLL";       break;
        }
         iTween.ValueTo(gameObject, iTween.Hash(
             "from", 0,
             "to", 1,
             "time", timeToReset,
             "onupdate", "OnBarUpdate",
             "oncomplete", "OnBarComplete"
         ));
	}
    void OnBarUpdate(float value)
    {
        bar.fillAmount = value;
    }
    void OnBarComplete()
    {
        Reset();
    }
    public void Clicked(int type)
    {
        switch (type)
        {
            case 1: Events.OnPowerUpActive(playerId, types.BOOST); break;
            case 2: Events.OnPowerUpActive(playerId, types.PAUSE_ME); break;
            default: Events.OnPowerUpActive(playerId, types.PAUSE_OTHERS); break;
        }
    }
}
