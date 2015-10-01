using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SummaryChallenge : MonoBehaviour
{

    public Text title_txt;
    public Text results_txt;
    public Text desc;

    private LevelData levelData;

    void Start()
    {

        levelData = Data.Instance.levelData;

        System.TimeSpan t = System.TimeSpan.FromSeconds(levelData.time);

        string timerFormatted = string.Format("{0:00}:{1:00}.{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
        Levels.LevelData CurrentlevelData = Data.Instance.levels.GetCurrentLevelData();

        float score = 0;
        if (CurrentlevelData.totalLaps > 0)
        {
            title_txt.text = "TIME";
            results_txt.text = timerFormatted;
            score = levelData.time;
        }
        else if (CurrentlevelData.totalTime > 0)
        {
            title_txt.text = "LAPS";
            results_txt.text = levelData.laps.ToString();
            score = levelData.laps;
        }

        int currentLevel = Data.Instance.levels.currentLevel;


        if (Data.Instance.levelData.challenge_objectID != "")
            CloseChallenge(currentLevel, score);
        else if (Data.Instance.levelData.challenge_username != "")
            SendChallenge(currentLevel, score);

    }
    private void SendChallenge(int currentLevel, float score)
    {
        desc.text = "Challenge sended";
        string username = Data.Instance.levelData.challenge_username;
        string facebookID = Data.Instance.levelData.challenge_facebookID;
        Events.OnChallengeCreate(username, facebookID, currentLevel, score);
        Data.Instance.levelData.ResetChallenge();
    }
    private void CloseChallenge(int currentLevel, float score)
    {
        float challenge_op_score = Data.Instance.levelData.challenge_op_score;
        string challenge_objectID = Data.Instance.levelData.challenge_objectID;

        string winner = Data.Instance.levelData.challenge_facebookID;

        if (score < challenge_op_score && Data.Instance.levels.GetCurrentLevelData().totalLaps > 0)
            winner = Data.Instance.userData.facebookID;
        else if (score > challenge_op_score && Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            winner = Data.Instance.userData.facebookID;


        Events.OnChallengeClose(challenge_objectID, Data.Instance.levelData.challenge_facebookID, winner, score);
        Data.Instance.levelData.ResetChallenge();
    }

    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }

}
