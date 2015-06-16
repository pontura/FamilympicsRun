using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Powerups : MonoBehaviour {

    private int timeToReset = 2;
    public int playerId;
    public int powerUpActive;
    public types type;

    [SerializeField]
    public Image bar;
    public GameObject ForwardAsset;
    public GameObject PauseAsset;
    public GameObject RewindAsset;

    private bool isOn;

    public enum types
    {
        FORWARD,
        PAUSE,
        REWIND
    }

	void Start () {
        if(Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
             Events.OnPowerUpOn += OnPowerUpOn;
	}
    void OnDestroy()
    {
        Events.OnPowerUpOn -= OnPowerUpOn;
    }
    void Reset()
    {
        bar.gameObject.SetActive(false);
        ForwardAsset.gameObject.SetActive(false);
        PauseAsset.gameObject.SetActive(false);
        RewindAsset.gameObject.SetActive(false);
    }
    void OnPowerUpOn()
    {
        powerUpActive = Random.Range(1, 4);
        bar.gameObject.SetActive(true);
        switch (powerUpActive)
        {
            case 1: type = types.FORWARD; ForwardAsset.gameObject.SetActive(true); break;
            case 2: type = types.PAUSE; PauseAsset.gameObject.SetActive(true); break;
            case 3: type = types.REWIND; RewindAsset.gameObject.SetActive(true); break;
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
            case 1: Events.OnPowerUpActive(playerId, types.FORWARD); break;
            case 2: Events.OnPowerUpActive(playerId, types.PAUSE); break;
            default: Events.OnPowerUpActive(playerId, types.REWIND); break;
        }
    }
}
