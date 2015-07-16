using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerButtons : MonoBehaviour {

    public int id;
    private int laps;

    [SerializeField]
    Text lapsLabelTitle;
    [SerializeField]
    Text lapsLabel;
    [SerializeField]
    Image button1;
    [SerializeField]
    Image button2;

    private Image activeButton;
    float scaleSmall;
    float scaleBig;

    public Player player;
    private GameManager gameManager;


	public void Init (int id) {
        
        this.id = id;
        scaleBig = button1.transform.localScale.x;
        scaleSmall = scaleBig - 0.04f;
        

        button1.GetComponent<Image>().color = Data.Instance.colors[id - 1];
        button2.GetComponent<Image>().color = Data.Instance.colors[id - 1];
        lapsLabel.GetComponent<Text>().color = Data.Instance.colors[id - 1];

        if (GameObject.Find("Game").GetComponent<GameManager>().ForceMultiplayer) return;

        if (id > 2)
        {
            lapsLabelTitle.transform.localScale = new Vector3(-1, 1, 1);
            lapsLabel.transform.localScale = new Vector3(-1, 1, 1);
            lapsLabelTitle.alignment = TextAnchor.MiddleLeft;
            lapsLabel.alignment = TextAnchor.MiddleLeft;
        }
	}
    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        LoopStart();
        Events.OnAvatarDie += OnAvatarDie;
        Events.OnAvatarWinLap += OnAvatarWinLap;
    }
    void LoopStart()
    {
        if (gameManager.ForceMultiplayer && gameManager.players.Count>0)
        {
            player = gameManager.players[0];
            id = player.id;
            Init(id);
        }
        else
        {
            foreach (Player thisPlayer in gameManager.players)
            {
                if (thisPlayer.id == id)
                {
                    player = thisPlayer;
                    this.id = player.id;
                }
            }
        }
    }
    void OnDestroy()
    {
        Events.OnAvatarDie -= OnAvatarDie;
        Events.OnAvatarWinLap -= OnAvatarWinLap;
    }
    void OnAvatarWinLap(int _id, int laps)
    {
        this.laps = laps;
        if (id == _id)
        {
            lapsLabel.text = laps.ToString();
        }
    }
    public void Update()
    {
        if (!player)
        {
            LoopStart();
            return;
        }
        if (player.meters.Length > 0)
            lapsLabel.text = player.meters;
        else lapsLabel.text = "0000";
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
