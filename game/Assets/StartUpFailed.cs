using UnityEngine;
using System.Collections;

public class StartUpFailed : MonoBehaviour {

    public int id;
    public CharacterFace characterFace;

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
    }
    void OnDestroy()
    {
        Events.OnFalseStart -= OnFalseStart;
    }	
    void OnFalseStart(int _id)
    {
	    if(id != _id) return;
        GetComponent<Animation>().Play("FalseStart");
	}
}
