using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserData : MonoBehaviour {
    
    private Data data;

	public void Init () {
        data = GetComponent<Data>();
	}
}
