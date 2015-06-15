using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerButtons : MonoBehaviour {

    public int id;

    [SerializeField]
    Text lapsLabel;
    [SerializeField]
    Image button1;
    [SerializeField]
    Image button2;
    private Image activeButton;
    float scaleSmall;
    float scaleBig;


	public void Init (int id) {
        this.id = id;
        scaleBig = button1.transform.localScale.x;
        scaleSmall = scaleBig - 0.04f;
        Events.OnAvatarDie += OnAvatarDie;
        Events.OnAvatarWinLap += OnAvatarWinLap;
        if (Data.Instance.levelData.numPlayers < 2 && id > 1) Destroy(gameObject);
        if (Data.Instance.levelData.numPlayers < 3 && id > 2) Destroy(gameObject);
        else if (Data.Instance.levelData.numPlayers < 4 && id > 3) Destroy(gameObject);

        button1.GetComponent<Image>().color = Data.Instance.colors[id - 1];
        button2.GetComponent<Image>().color = Data.Instance.colors[id - 1];
        lapsLabel.GetComponent<Text>().color = Data.Instance.colors[id - 1];
	}
    void OnDestroy()
    {
        Events.OnAvatarDie -= OnAvatarDie;
        Events.OnAvatarWinLap -= OnAvatarWinLap;
    }
    void OnAvatarWinLap(int _id, int laps)
    {
        if (id == _id)
        {
            lapsLabel.text = laps.ToString();
        }
    }
    void OnAvatarDie(Player player)
    {
        if(player.id == id)
            Destroy(gameObject);
    }
    public void MidButtonPressed()
    {
        Events.OnAvatarJump(id);
    }
    public void PressedButton(int num)
    {
        Image buttonPressed;
        switch (num)
        {
            case 1: buttonPressed = button1; break;
            default: buttonPressed = button2; break;
        }
        if (buttonPressed == activeButton)
            return;

        button1.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
        button2.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
        activeButton = buttonPressed;
        buttonPressed.transform.localScale = new Vector3(scaleBig, scaleBig, scaleBig);
        Events.OnAvatarRun(id);
    }
}
