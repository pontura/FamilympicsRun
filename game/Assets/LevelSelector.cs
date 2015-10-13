using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LevelSelector : MonoBehaviour {

    public ScrollLimit scrollLimit;
    public Color backgroundSinglePlayer;
    public Color backgroundMultiplayer;

    public GameObject loginButton;
    public GameObject container;
    public GameObject buttonsContainer;

    [SerializeField]
    LevelButton levelButton;

    private int buttonsSeparation = 160;
    private int offsetX = 90;
    private int seasonWhiteSpace = 112;

	void Start () {

        if(FB.IsLoggedIn)
            Events.OnChallengesLoad();

        //resetea el flag de challenge por si es un level que no jugaste aun
        Data.Instance.levelData.dontSaveScore = false;

        Data.Instance.userData.starsCount = 0;
        Events.CheckForNewNotifications();

        Events.OnMusicChange("menus");        
       
        if (FB.IsLoggedIn)
        {
            if (Data.Instance.userData.FacebookFriends != null && Data.Instance.userData.FacebookFriends.Count == 0)
            {
                // Data.Instance.loginManager.GetFriends();
            }
        }
        Data.Instance.levelData.ResetChallenge();
        OnChangePlayMode(Data.Instance.userData.mode);
        Events.OnChangePlayMode += OnChangePlayMode;           

        LoadButtons();
        Positionate();
        Events.OnLoadLocalData();

        if (Data.Instance.energyManager.energy <= 0)
            Invoke("OnOpenEnergyPopup", 0.5f);
	}
    void OnOpenEnergyPopup()
    {
        Events.OnOpenEnergyPopup();
    }
    void LoadButtons()
    {
        int b = 0;
        int _seasonWhiteSpace = 0;
        Data.Instance.userData.starsCount = 0;

        int total = Data.Instance.levels.levels.Length;
        float lastLevelScore = 0;
        for (int a = 1; a < total; a++)
        {
            b++;
            Data.Instance.levelsData.ArrengeListByScore(a);
            LevelButton newLevelButton = Instantiate(levelButton) as LevelButton;
            newLevelButton.transform.SetParent(buttonsContainer.transform);
            int _x = (buttonsSeparation) * (a - 2) - offsetX + _seasonWhiteSpace;

            if (b == 8)
            {
                _seasonWhiteSpace += seasonWhiteSpace;
                b = 0;
            }

            newLevelButton.transform.localPosition = new Vector3(_x, 0, 0);
            newLevelButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
          //  float lastLevelScore = PlayerPrefs.GetFloat("Run_Level_" + (a - 1).ToString());

            float score = Data.Instance.levelsData.GetMyScoreIfExists(a);

            int _stars = Data.Instance.levels.GetCurrentLevelStarsByScore(a, score);

            //print("Run_Level_" + a + "   -   stars: " + _stars + "     score: " + score);

            if(_stars>0)
                Data.Instance.userData.starsCount += _stars;

            if (Data.Instance.FreeLevels) lastLevelScore = 3;
            newLevelButton.Init(this, a, lastLevelScore);

            if (lastLevelScore > 0)
                Data.Instance.userData.levelProgressionId = a;

            if (Data.Instance.FreeLevels)
                Data.Instance.userData.levelProgressionId = 100;

            lastLevelScore = score;

        }

        GetComponent<Tournaments>().Init();

        if (Data.Instance.FreeLevels == true)
            Data.Instance.userData.starsCount = 1000;
    }
    void Positionate()
    {
        int positionX = 0;
        if (Data.Instance.levels.currentLevel > 0)
            positionX = Data.Instance.levels.currentLevel - 2;
        else
            positionX = Data.Instance.userData.levelProgressionId - 2;

        if (positionX > 0)
            positionX *= -buttonsSeparation;

        positionX += 100;

        Vector3 pos = container.transform.localPosition;
        pos.x = positionX;
        container.transform.localPosition = pos;
    }
    void OnDestroy()
    {
        Events.OnChangePlayMode -= OnChangePlayMode;
    }
    public void StartLevel(int id)
    {
        print("StartLevel" + id);
        if (Data.Instance.energyManager.energy <= 0)
        {
            Events.OnOpenEnergyPopup();
            return;
        }
        GetComponent<LevelDetailsPopup>().Open(id);
    }
    public void GotoLevel(int id)
    {
       // Events.OnSoundFX("buttonPress");
      //  Events.OnLoadParseScore(id);
        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            Data.Instance.Load("GameSingle");
        else
            Data.Instance.Load("Players");

        Data.Instance.GetComponent<Levels>().currentLevel = id;
        Events.OnRefreshHiscores();
    }
    public void Refresh()
    {
        Data.Instance.FreeLevels = true;
       // Data.Instance.levelsData.Refresh();
        Data.Instance.Load("LevelSelector");        
    }
    public void Challenges()
    {
        if (FB.IsLoggedIn)
            Data.Instance.Load("Challenges");
        else
            Events.OnFacebookNotConnected();
    }
    public void Notifications()
    {
        if (FB.IsLoggedIn)
            Data.Instance.Load("Notifications");
        else
            Events.OnFacebookNotConnected();
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
