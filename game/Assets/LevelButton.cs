using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour {

    public Text labelNum;
    public int id;
    public RankingLine user1;
    public RankingLine user2;
    public RankingLine user3;
    public Text myScore;

    public bool infoLoaded;

    void Start()
    {
        user1.gameObject.SetActive(false);
        user2.gameObject.SetActive(false);
        user3.gameObject.SetActive(false);
    }
    public void Init(LevelSelector levelSelector, int levelID)
    {
        
        this.id = levelID;

        float _myScore = PlayerPrefs.GetFloat("Run_Level_" + levelID);
        
        if(_myScore>0)
             myScore.text = Data.Instance.levelsData.GetScoreString(levelID, _myScore);

        float myLastScore = PlayerPrefs.GetFloat("Run_Level_" + (levelID-1).ToString() );

        if (myLastScore == 0 && levelID > 1)
        {
            labelNum.text = "X";
            return;
        }
        
        labelNum.text = levelID.ToString();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            levelSelector.StartLevel(id);
        });
    }
    void Update()
    {
        if (infoLoaded) return;
        LevelsData.LevelsScore levelScore = Data.Instance.levelsData.GetLevelScores(id);
        if (levelScore != null && levelScore.scoreData1.score > 0 )
        {
            print("loading scores of " + id);
            infoLoaded = true;
            user1.gameObject.SetActive(true);
            user2.gameObject.SetActive(true);
            user3.gameObject.SetActive(true);

            user1.Init(id, levelScore.scoreData1.playerName, levelScore.scoreData1.score.ToString(), levelScore.scoreData1.facebookID);
            user2.Init(id, levelScore.scoreData2.playerName, levelScore.scoreData2.score.ToString(), levelScore.scoreData2.facebookID);
            user3.Init(id, levelScore.scoreData3.playerName, levelScore.scoreData3.score.ToString(), levelScore.scoreData3.facebookID);

        }
    }
    
    
}
