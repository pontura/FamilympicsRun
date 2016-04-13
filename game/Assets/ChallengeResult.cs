using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeResult : MonoBehaviour {

    public GameObject panel;

    public string challenge_objectID;
    public string winner;
    public float myScore;

    public Color colorWin;
    public Color colorLose;

    public Text title1;
    public Text title2;

    public Text username1;
    public Text username2;

    public Text score1;
    public Text score2;

    public ProfilePicture profilePicture1;
    public ProfilePicture profilePicture2;

    public GameObject wonPanel;
    public GameObject losePanel;

    public void Init()
    {
        wonPanel.SetActive(false);
        losePanel.SetActive(false);

        panel.transform.localScale = Data.Instance.screenManager.scale;

        int levelID = Data.Instance.levels.currentLevel;

        username1.text = Data.Instance.gameSettings.GetUsername(Data.Instance.userData.username);
        username2.text = Data.Instance.gameSettings.GetUsername(Data.Instance.levelData.challenge_username);

        myScore = Data.Instance.levelData.time;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
            myScore = Data.Instance.levelData.laps;

        float _score2 = Data.Instance.levelData.challenge_op_score;

        score1.text = Data.Instance.levelsData.GetScoreString(levelID, myScore);
        score2.text = Data.Instance.levelsData.GetScoreString(levelID, _score2); 

        profilePicture1.setPicture(Data.Instance.userData.facebookID);
        profilePicture2.setPicture(Data.Instance.levelData.challenge_facebookID);

        bool youWon = false;

        if (Data.Instance.levels.GetCurrentLevelData().Sudden_Death && myScore > _score2) youWon = true;        
        else if (Data.Instance.levels.GetCurrentLevelData().totalLaps > 0 && myScore < _score2) youWon = true;
        else if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0 && myScore > _score2) youWon = true;

         string result = "";
         winner = "";
         if (youWon)
         {
             title1.color = colorWin;
             winner = Data.Instance.userData.facebookID;
             result = "YOU WON THE CHALLENGE";
             wonPanel.SetActive(true);
         }
         else
         {
             title1.color = colorLose;
             winner = Data.Instance.levelData.challenge_facebookID;
             losePanel.SetActive(true);
             result = "YOU LOST THE CHALLENGE";
         }

        title1.text = result;

        title2.text = "LEVEL " + levelID + " / " + Data.Instance.levels.GetSeason(levelID);

        challenge_objectID = Data.Instance.levelData.challenge_objectID;        
        
    }
    
	public void Share () 
    {
        //SHARE
        Debug.Log("SHARE");
        //if(winner == Data.Instance.userData.facebookID)
          //  Data.Instance.facebookShare.ShareToFriend(Data.Instance.levelData.challenge_username + " accepted your challenge and beat you!");
        Data.Instance.facebookShare.ShareToFriend(Data.Instance.levelData.challenge_facebookID, Data.Instance.userData.username + " accepted your challenge and beat you!");
        //else
           // Data.Instance.facebookShare.LostChallengeTo(Data.Instance.levelData.challenge_username);

        Close();
	}
    public void Dismiss()
    {
        print("Dismiss");
        Close();
    }
    public void Rematch()
    {
        print("Rematch");
        Data.Instance.levelData.RematchChallenge();
        Data.Instance.Load("GameSingle");
    }
    public void Close()
    {
        Events.OnChallengeClose(challenge_objectID, Data.Instance.levelData.challenge_facebookID, winner, myScore);
        Data.Instance.Load("LevelSelector");
        Data.Instance.levelData.ResetChallenge();
    }
}
