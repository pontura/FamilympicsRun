using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TournamentResults : MonoBehaviour {

    public GameObject panel;

    public MultiplayerResultLine puesto1;
    public MultiplayerResultLine puesto2;
    public MultiplayerResultLine puesto3;
    public MultiplayerResultLine puesto4;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(false);
	}
    void OnDestroy()
    {
    }
    public void Init()
    {
        panel.SetActive(true);     

        puesto2.gameObject.SetActive(false);
        puesto3.gameObject.SetActive(false);
        puesto4.gameObject.SetActive(false);
         
    }
    public void Close()
    {
        Data.Instance.Load("LevelSelector");
    }

}
