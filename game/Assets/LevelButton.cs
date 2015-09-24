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
        this.levelID = levelID;
        this.myLastScore = myLastScore;
        levelScore = Data.Instance.levelsData.GetLevelScores(levelID);
        image = GetComponent<Image>();
        this.id = levelID;
       
        
        OnChangePlayMode(Data.Instance.userData.mode);

        if (myLastScore == 0 && levelID > 1)
        {
            button.GetComponent<Image>().color = lockColor;
            LockImage.enabled = true;
            labelNum.enabled = false;
            return;
        }

       

        LockImage.enabled = false;
        
        labelNum.text = levelID.ToString();
        button.onClick.AddListener(() =>
        {
            if( Data.Instance.levels.CanPlay(id))
            {
                levelSelector.StartLevel(id);
            }
        });
        StartCoroutine("LoopToCheckChanges");
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
            foreach (LevelsData.ScoreData sd in ls.scoreData)
            {
                if (user1.score_float != sd.score)
                    refresh = true;
            }
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
        float _myScore = PlayerPrefs.GetFloat("Run_Level_" + id);

        if (_myScore > 0)
        {
            
            string _score = Data.Instance.levelsData.GetScoreString(id, _myScore);
            int _stars = Data.Instance.levels.GetCurrentLevelStarsByScore(id, _myScore);
            Events.AddStarsToCount(_stars);
            stars.Init(_stars);
            myScore.text = _score;
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
