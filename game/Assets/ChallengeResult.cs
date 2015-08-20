using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeResult : MonoBehaviour {

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

        int levelID = Data.Instance.levels.currentLevel;

        username1.text = Data.Instance.userData.username;
        username2.text = Data.Instance.levelData.challenge_username;

        float _score1 = Data.Instance.levelData.time;
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0)
           _score1  = Data.Instance.levelData.laps;

        float _score2 = Data.Instance.levelData.challenge_op_score;      

        score1.text = Data.Instance.levelsData.GetScoreString(levelID, _score1);
        score2.text = Data.Instance.levelsData.GetScoreString(levelID, _score2); 

        profilePicture1.setPicture(Data.Instance.userData.facebookID);
        profilePicture2.setPicture(Data.Instance.levelData.challenge_facebookID);

        bool youWon = false;

        if (Data.Instance.levels.GetCurrentLevelData().totalLaps > 0 && _score1 < _score2) youWon = true;
            else
        if (Data.Instance.levels.GetCurrentLevelData().totalTime > 0 && _score1 > _score2) youWon = true;

         string result = "";
         if (youWon)
         {
             result = "YOU WON THE CHALLENGE";
             wonPanel.SetActive(true);
         }
         else
         {
             losePanel.SetActive(true);
             result = "YOU LOST THE CHALLENGE";
         }

        title1.text = result;

        title2.text = "LEVEL " + levelID;
        
    }	
	public void Share () 
    {
	
	}
    public void Dismiss()
    {

    }
    public void Rematch()
    {

    }
    public void Close()
    {

    }
}
