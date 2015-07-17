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
    }
    void OnDestroy()
    {
        Events.OnFalseStart -= OnFalseStart;
    }	
    void OnFalseStart(int _id)
    {
	    if(id != _id) return;
        animation.Play("FalseStart");
	}
}
