using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerButtons : MonoBehaviour {

    public int id;

    [SerializeField]
    Text lapsLabel;
    [SerializeField]
    Button button1;
    [SerializeField]
    Button button2;
    private Button activeButton;
    float scaleSmall = 0.38f;
    float scaleBig = 0.42f;


	void Start () {
        Events.OnAvatarDie += OnAvatarDie;
        Events.OnAvatarWinLap += OnAvatarWinLap;
        if (Data.Instance.numPlayers < 3 && id > 2) Destroy(gameObject);
        else if (Data.Instance.numPlayers < 4 && id > 3) Destroy(gameObject);

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

    public void PressedButton(int num)
    {
        Button buttonPressed;
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
