using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStart : MonoBehaviour {

    public Text title1;
    public Text title2;

    public GameObject panel;

	void Start () {

     //   title1.text = "HOLA0:";
     //   title2.text = "HOLA0:";

        panel.SetActive(true);
        Invoke("Ready", 3);
	}
    void Ready()
    {
        panel.SetActive(false);
        Events.OnRaceStartReady();
    }
}
