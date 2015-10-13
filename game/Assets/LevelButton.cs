using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelButton : MonoBehaviour {

    public Stars stars;
    public Color lockColor;
    public Image LockImage;
    public Button button;
    public Text labelNum;
    public int id;
    public RankingLine user1;
    public RankingLine user2;
    public RankingLine user3;
    public Text myScore;

    public bool infoLoaded;
    public LevelsData.LevelsScore levelScore;
    private Image image;
    private MultiplayerData multiplayerData;
    private float myLastScore;
    private int levelID;
    private LevelSelector levelSelector;
    private bool isLocked;

    void Start()
    {
        this.multiplayerData = Data.Instance.GetComponent<MultiplayerData>();
        Events.OnChangePlayMode += OnChangePlayMode;
    }
    void OnDestroy()
    {
        Events.OnChangePlayMode -= OnChangePlayMode;
    }
    public void Init(LevelSelector levelSelector, int levelID, float myLastScore)
    {
        this.levelSelector = levelSelector;
        this.levelID = levelID;
        this.myLastScore = myLastScore;
        levelScore = Data.Instance.levelsData.GetLevelScores(levelID);
        image = GetComponent<Image>();
        this.id = levelID;
       
        
        OnChangePlayMode(Data.Instance.userData.mode);

        int tournamentActive = Data.Instance.userData.GetTournamentAvailable();
        bool showFriends = true;
        if (levelID > 8 && tournamentActive < 2) showFriends = false;
        else if (levelID > 16 && tournamentActive < 3) showFriends = false;
        else if (levelID > 24 && tournamentActive < 4) showFriends = false;
        if (showFriends) StartCoroutine("LoopToCheckChanges");


        if (myLastScore == 0 && levelID > 1)
            SetLock(true);
        else 
            SetLock(false);

        button.onClick.AddListener(() =>
        {
            Clicked();
        });
        
    }
    void Clicked()
    {
        if (isLocked) return;
        if (Data.Instance.levels.CanPlay(id))
        {
            levelSelector.StartLevel(id);
        }
    }
    void SetLock(bool isLocked)
    {
        this.isLocked = isLocked;
        if (isLocked)
        {
            button.GetComponent<Image>().color = lockColor;
            LockImage.enabled = true;
            labelNum.enabled = false;
        }
        else
        {
            button.GetComponent<Image>().color = Color.white;
            LockImage.enabled = false;
            labelNum.enabled = true;

            labelNum.text = levelID.ToString();
           

        }
    }
    IEnumerator LoopToCheckChanges()
    {
        // se fija si cambio en el medio...
        yield return new WaitForSeconds(1);

        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
        {

            LevelsData.LevelsScore ls = Data.Instance.levelsData.GetLevelScores(levelID);
            int a = 0;
            bool refresh = false;
            //foreach (LevelsData.ScoreData sd in ls.scoreData)
            //{
            if (ls.scoreData.Count>0 && user1.score_float != ls.scoreData[0].score)
                    refresh = true;
           // }
            if (refresh) Refresh();
            if (!infoLoaded) Refresh();
        }
        StartCoroutine("LoopToCheckChanges");
    }
    void Refresh()
    {
        infoLoaded = true;
        Invoke("WaitToLoadAllScoresInLevel", 0.5f);
    }
    void WaitToLoadAllScoresInLevel()
    {
        OnChangePlayMode(Data.Instance.userData.mode);
    }
    void LoadSinglePlayerWinners()
    {
        float _myScore = Data.Instance.levelsData.GetMyScoreIfExists(id);

        if (_myScore > 0)
        {
            
            string _score = Data.Instance.levelsData.GetScoreString(id, _myScore);
            int _stars = Data.Instance.levels.GetCurrentLevelStarsByScore(id, _myScore);
            stars.Init(_stars);
            myScore.text = _score;
            SetLock(false);

            if (_myScore != PlayerPrefs.GetFloat("Run_Level_" + id))
            {
                Debug.Log("___GRABA LOCAL level: " + id + " score: " + _myScore);
                PlayerPrefs.SetFloat("Run_Level_" + id, _myScore);
            }
        }        

        if (Data.Instance.userData.facebookID.Length < 2)
        {
            user1.gameObject.SetActive(false);
            user2.gameObject.SetActive(false);
            user3.gameObject.SetActive(false);
            return;
        }

        if (levelScore.scoreData.Count>0)
        {
            user1.gameObject.SetActive(true);
            LevelsData.ScoreData scoreData = levelScore.scoreData[0];
            user1.Init(id, scoreData.playerName, scoreData.score.ToString(), scoreData.facebookID);
            user1.SetSinglePlayer();
        }
        else user1.gameObject.SetActive(false);
        if (levelScore.scoreData.Count > 1)
        {
            user2.gameObject.SetActive(true);
            LevelsData.ScoreData scoreData = levelScore.scoreData[1];
            user2.Init(id, scoreData.playerName, scoreData.score.ToString(), scoreData.facebookID);
            user2.SetSinglePlayer();
        }
        else user2.gameObject.SetActive(false);
        if (levelScore.scoreData.Count > 2)
        {
            user3.gameObject.SetActive(true);
            LevelsData.ScoreData scoreData = levelScore.scoreData[2];
            user3.Init(id, scoreData.playerName, scoreData.score.ToString(), scoreData.facebookID);
            user3.SetSinglePlayer();
        }
        else user3.gameObject.SetActive(false);
    }

    //TODO:::::::::::
    void LoadMultiplayerWinners()
    {
        user1.gameObject.SetActive(false);
        user2.gameObject.SetActive(false);
        user3.gameObject.SetActive(false);

        myScore.text = "";
        multiplayerData = Data.Instance.GetComponent<MultiplayerData>();

        List<MultiplayerData.HiscoresData> hiscoreData =  multiplayerData.hiscoreLevels[id].hiscores;
        if (hiscoreData == null) return;
        if (hiscoreData.Count == 0) return;
        if (hiscoreData.Count > 0)
        {
           // print("______LoadMultiplayerWinners: levelID: " + id + "hiscoreData[0].score: " + hiscoreData[0].score);
            int numStars = Data.Instance.levels.GetCurrentLevelStarsByScore(id, hiscoreData[0].score);
            stars.Init(numStars);

            user1.gameObject.SetActive(true);
            user1.Init(id, hiscoreData[0].username, hiscoreData[0].score.ToString(), "");
            user1.SetMultiplayerColor(hiscoreData[0].playerID);
        }
        if (hiscoreData.Count > 1)
        {
            user2.gameObject.SetActive(true);
            user2.Init(id, hiscoreData[1].username, hiscoreData[1].score.ToString(), "");
            user2.SetMultiplayerColor(hiscoreData[1].playerID);
        }
        if (hiscoreData.Count > 2)
        {
            user3.gameObject.SetActive(true);
            user3.Init(id, hiscoreData[2].username, hiscoreData[2].score.ToString(), "");
            user3.SetMultiplayerColor(hiscoreData[2].playerID);
        }
    }

    void OnChangePlayMode(UserData.modes mode)
    {
        
        switch (mode)
        {
            case UserData.modes.MULTIPLAYER:
                CheckForHiscoreColor();
                LoadMultiplayerWinners();
                break;
            case UserData.modes.SINGLEPLAYER:
                if (myLastScore == 0 && levelID > 1)
                    button.GetComponent<Image>().color = lockColor;
                else
                    button.image.color = Color.white;
                LoadSinglePlayerWinners();
                break;
        }
    }
    void CheckForHiscoreColor()
    {
        if (Data.Instance.multiplayerData.hiscoreLevels[id].lastWinner != 0)
        {
            int playerID = Data.Instance.multiplayerData.hiscoreLevels[id].lastWinner;
            Color color = Data.Instance.colors[playerID - 1];
            print(color);
            if (button)
                button.image.color = color;
            else
                print("No hay boton");
        }
    }
    public void Challenge(RankingLine rankingLine)
    {

        //Data.Instance.Load("ChallengeCreator");
        return;


        if (Data.Instance.userData.mode == UserData.modes.MULTIPLAYER) return;

        if (rankingLine.facebookID == Data.Instance.userData.facebookID) return;

        Data.Instance.levels.currentLevel = id;
        Data.Instance.levelData.challenge_facebookID = rankingLine.facebookID;
        Data.Instance.levelData.challenge_username = rankingLine.playerName;
        Data.Instance.Load("ChallengeConfirm");
    }
    
    
}
