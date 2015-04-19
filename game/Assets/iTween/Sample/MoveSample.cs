using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
        iTween.FadeTo(gameObject, iTween.Hash(
            "time", 2,
            "alpha", 0,
            "loopType", iTween.LoopType.pingPong,
            "onstart", "M_OnStart"));
	}

    void M_OnStart()
    {
        
    }
}

