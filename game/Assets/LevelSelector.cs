using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LevelSelector : MonoBehaviour {

    public Color backgroundSinglePlayer;
    public Color backgroundMultiplayer;

    public Text debugText;
    public GameObject loginButton;
    public GameObject container;

    [SerializeField]
    LevelButton levelButton;

    private int buttonsSeparation = 300;

	void Start () {
        
       // debugText.text = "Not Logged!";
        if (FB.IsLoggedIn)
        {
            debugText.text = "Facebook Logged";
            if (Data.Instance.userData.FacebookFriends != null && Data.Instance.userData.FacebookFriends.Count == 0)
            {
                debugText.text += " carga anigos...";
                Data.Instance.loginManager.GetFriends();
            }
        }

       // debugText.text += " amigos: " + Data.Instance.userData.FacebookFriends.Count;

        Data.Instance.levelData.ResetChallenge();

        for (int a = 1; a < Data.Instance.levels.levels.Length; a++ )
        {
            LevelButton newLevelButton = Instantiate(levelButton) as LevelButton;
            newLevelButton.transform.SetParent(container.transform);
            newLevelButton.transform.localPosition = new Vector3(buttonsSeparation * (a-2), 0, 0);
            newLevelButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            newLevelButton.Init(this, a);
        }
        OnChangePlayMode(Data.Instance.userData.mode);
        Events.OnChangePlayMode += OnChangePlayMode;
	}
    void OnDestroy()
    {
        Events.OnChangePlayMode -= OnChangePlayMode;
    }
    public void StartLevel(int id)
    {
        Events.OnLoadParseScore(id);
        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            Data.Instance.Load("SinglePlayer");
        else
            Data.Instance.Load("Players");

        Data.Instance.GetComponent<Levels>().currentLevel = id;
    }
    public void Refresh()
    {
        Data.Instance.levelsData.Refresh();
        Data.Instance.Load("LevelSelector");        
    }
    public void Challenges()
    {
        Data.Instance.Load("Challenges");
    }
    public void ResetApp()
    {
        PlayerPrefs.DeleteAll();
        Data.Instance.userData.Reset();
        Data.Instance.Load("MainMenu");
        Data.Instance.loginManager.ParseFBLogout();
    }
    void OnChangePlayMode(UserData.modes mode)
    {
        switch (mode)
        {
            case UserData.modes.MULTIPLAYER:
                GetComponent<Camera>().backgroundColor = backgroundMultiplayer;
                break;
            case UserData.modes.SINGLEPLAYER:
                GetComponent<Camera>().backgroundColor = backgroundSinglePlayer;
                break;
        }
    }
}
