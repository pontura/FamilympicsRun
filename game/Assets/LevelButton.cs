using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour {

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

    void Start()
    {
        user1.gameObject.SetActive(false);
        user2.gameObject.SetActive(false);
        user3.gameObject.SetActive(false);

        Events.OnChangePlayMode += OnChangePlayMode;
    }
    void OnDestroy()
    {
        Events.OnChangePlayMode -= OnChangePlayMode;
    }
    public void Init(LevelSelector levelSelector, int levelID)
    {
        image = GetComponent<Image>();
        this.id = levelID;
        levelScore = Data.Instance.levelsData.GetLevelScores(levelID);

        float myLastScore = PlayerPrefs.GetFloat("Run_Level_" + (levelID-1).ToString() );

        if (myLastScore == 0 && levelID > 1)
        {
            labelNum.text = "X";
            return;
        }
        
        labelNum.text = levelID.ToString();
        button.onClick.AddListener(() =>
        {
            levelSelector.StartLevel(id);
        });
        OnChangePlayMode(Data.Instance.userData.mode);
    }
    void Update()
    {
        if (infoLoaded) return;
        
        if (levelScore != null && levelScore.scoreData1.score > 0 )
        {
            infoLoaded = true;
            OnChangePlayMode(Data.Instance.userData.mode);
        }
    }
    void LoadSinglePlayerWinners()
    {

        float _myScore = PlayerPrefs.GetFloat("Run_Level_" + id);
        if (_myScore > 0)
            myScore.text = Data.Instance.levelsData.GetScoreString(id, _myScore);

        if (levelScore.scoreData1.playerName != "")
        {
            user1.gameObject.SetActive(true);
            user1.Init(id, levelScore.scoreData1.playerName, levelScore.scoreData1.score.ToString(), levelScore.scoreData1.facebookID);
        }
        else user1.gameObject.SetActive(false);
        if (levelScore.scoreData2.playerName != "")
        {
            user2.gameObject.SetActive(true);
            user2.Init(id, levelScore.scoreData2.playerName, levelScore.scoreData2.score.ToString(), levelScore.scoreData2.facebookID);
        }
        else user2.gameObject.SetActive(false);
        if (levelScore.scoreData3.playerName != "")
        {
            user3.gameObject.SetActive(true);
            user3.Init(id, levelScore.scoreData3.playerName, levelScore.scoreData3.score.ToString(), levelScore.scoreData3.facebookID);
        }
        else user3.gameObject.SetActive(false);
    }

    //TODO:::::::::::
    void LoadMultiplayerWinners()
    {
        myScore.text = "";
        user1.gameObject.SetActive(false);
        user2.gameObject.SetActive(false);
        user3.gameObject.SetActive(false);
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
                button.image.color = Color.white;
                LoadSinglePlayerWinners();
                break;
        }
    }
    void CheckForHiscoreColor()
    {
        if (Data.Instance.multiplayerData.hiscoreLevels[id].lastWinner != 0)
            button.image.color = Data.Instance.multiplayerData.GetPlayer(Data.Instance.multiplayerData.hiscoreLevels[id].lastWinner).color;
    }
    public void Challenge(RankingLine rankingLine)
    {
        if (rankingLine.facebookID == Data.Instance.userData.facebookID) return;

        Data.Instance.levels.currentLevel = id;
        Data.Instance.levelData.challenge_facebookID = rankingLine.facebookID;
        Data.Instance.levelData.challenge_username = rankingLine.playerName;
        Data.Instance.Load("ChallengeConfirm");
    }
    
    
}
