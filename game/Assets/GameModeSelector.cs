using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeSelector : MonoBehaviour {

    public GameObject multiButton;
    public GameObject singleButton;

    void Start()
    {
        SetButton();
    }
    public void Toogle()
    {
        Data.Instance.userData.ToogleMode();
        SetButton();
    }
    void SetButton()
    {
        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER)
        {
            multiButton.SetActive(true);
            singleButton.SetActive(false);
        }
        else
        {
            singleButton.SetActive(true);
            multiButton.SetActive(false);
        }
    }
   
}
