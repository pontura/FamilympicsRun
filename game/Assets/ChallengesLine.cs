using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengesLine : MonoBehaviour
{
    public Color lostColor;
    public Color WinColor;

    public string objectID;
    public string facebookID;
    public string username;
    public float op_score;
    public int levelId;
    public Text usernameLabel;
    public Text scoreLabel;
    public Text result;
    public Text level;
    public ProfilePicture profilePicture;
    public int id = 0;
    private Challenges challenges;
    public bool infoLoaded;

    public Button ok;
    public Button cancel;
    public GameObject RemindButton;
    public Challenges.PlayerData playerData;

    public void Init(Challenges challenges, int _id, Challenges.PlayerData playerData)
    {
        RemindButton.SetActive(false);
        this.playerData = playerData;
        this.challenges = challenges;
        this.id = _id;
        
        if (challenges.type == Challenges.types.MADE)
        {
            InactiveButtons();
            RemindButton.SetActive(true);
        }


        if (playerData.winner != "")
        {
            if (playerData.winner == Data.Instance.userData.facebookID)
            {
                result.text = "YOU WON";
                result.color = WinColor;
            }
            else
            {
                result.text = "YOU LOST";
                result.color = lostColor;
            }
            InactiveButtons();
        }
        this.objectID = playerData.objectID;
        this.facebookID = playerData.facebookID;

        username = Data.Instance.gameSettings.GetUsername(playerData.playerName);
        usernameLabel.text = username;
        

        levelId = playerData.level;
        level.text = "LEVEL " + levelId;

        string score = Data.Instance.levelsData.GetScoreString(playerData.level, playerData.score);

        Levels.LevelData data = Data.Instance.levels.GetData(levelId);

        if(data.Sudden_Death)
            scoreLabel.text = score;
        else
        if (data.totalTime > 0)
            scoreLabel.text = score + " in " + Data.Instance.levelsData.GetTimer(data.totalTime);
        else
            scoreLabel.text = data.totalLaps + "000" + "m in " + score;

        op_score = playerData.score;
        profilePicture.setPicture(facebookID);
        infoLoaded = true;
    }
    void InactiveButtons()
    {
        RemindButton.SetActive(false);
        ok.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
    }
    public void Accept()
    {
        Data.Instance.levels.currentLevel = levelId;
        challenges.Confirm(username, objectID, facebookID, op_score);
    }
    public void Cancel()
    {
        challenges.CancelChallenge(objectID);
        Destroy(gameObject);
    }
    public void Remind()
    {
        InactiveButtons();
        Events.OnChallengeRemind(objectID, facebookID);
    }
}
