using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeSelector : MonoBehaviour {

    public Color Multi;
    public Color Single;
    public Color UnselectedText;

    public GameObject interactiveBG;

    public Text multiButton;
    public Text singleButton;

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
            multiButton.color = Color.white;
            singleButton.color = UnselectedText;
            MoveBg(false);
        }
        else
        {
            singleButton.color = Color.white;
            multiButton.color = UnselectedText;
            MoveBg(true);
        }
    }
    void MoveBg(bool left)
    {
        if (left)
            interactiveBG.GetComponent<Image>().color = Single;
        else 
            interactiveBG.GetComponent<Image>().color = Multi;

        float _x = 68;
        if (left) _x = -68;
        Vector3 pos = new Vector3(_x, 0, 0);
        iTween.MoveTo(interactiveBG, iTween.Hash("position", pos, "islocal",true, "easetype", iTween.EaseType.easeOutQuad, "time", 0.25f));
    }
   
}
