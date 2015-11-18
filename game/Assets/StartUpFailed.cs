using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartUpFailed : MonoBehaviour {

    public int id;
    public CharacterFace characterFace;
    public Text label;

	void Start () {
        Events.OnFalseStart += OnFalseStart;
	}
   public void Init(int _id)
    {
        this.id = _id;
        characterFace.Init(id);
        int totalPlayers = Data.Instance.multiplayerData.players.Count;

        if (totalPlayers == 1) return;

        if (id > 2)
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
            Events.OnPowerUpActive += OnPowerUpActive;
    }
    void OnDestroy()
    {
        Events.OnFalseStart -= OnFalseStart;
        Events.OnPowerUpActive -= OnPowerUpActive;
    }
    void OnPowerUpActive(int _id, Powerups.types type)
    {
        switch (type)
        {
            case Powerups.types.PAUSE_ME:
                if (_id != id) return;
                OnPaused();
                break;
            case Powerups.types.PAUSE_OTHERS:
                if (_id == id) return;
                OnPaused();
                break;
        }
    }
    void OnPaused()
    {
        GetComponent<Animation>().Play("FalseStart");
        label.text = "Oh No!";
    }
    void OnFalseStart(int _id)
    {
	    if(id != _id) return;
        label.text = "FALSE START";
        GetComponent<Animation>().Play("FalseStart");
	}
}
