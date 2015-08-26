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

    void Start()
    {
        
    }
    public void Init(Challenges challenges, int _id)
    {
        hide();

        this.challenges = challenges;
        this.id = _id;
    }
    void Update()
    {
        if (infoLoaded) return;
        if (challenges.userData[id].facebookID != "")
        {
            string title = "";
            if (challenges.type == Challenges.types.MADE)
            {
               // title = "To: ";
                InactiveButtons();
            }
            else
            {
               // title = "From: ";
            }

            if (challenges.userData[id].winner != "")
            {
                if (challenges.userData[id].winner == Data.Instance.userData.facebookID)
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
            this.objectID = challenges.userData[id].objectID;
            this.facebookID = challenges.userData[id].facebookID;
            usernameLabel.text = title + challenges.userData[id].playerName;
            levelId = challenges.userData[id].level;
            level.text = "LEVEL " + levelId;
            this.username = challenges.userData[id].playerName;
            scoreLabel.text = Data.Instance.levelsData.GetScoreString(challenges.userData[id].level, challenges.userData[id].score);
            op_score = challenges.userData[id].score;
            profilePicture.setPicture(challenges.userData[id].facebookID);
            infoLoaded = true;

            show();
        }
    }
    void InactiveButtons()
    {
        ok.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
    }
    private void hide()
    {
        Vector3 pos = transform.localPosition;
        pos.x = 1000;
        transform.localPosition = pos;
    }
    private void show()
    {
        Vector3 pos = transform.localPosition;
        pos.x = 0;
        transform.localPosition = pos;
    }
    public void Accept()
    {
        Data.Instance.levels.currentLevel = levelId;
        challenges.Confirm(username, objectID, facebookID, op_score);
    }
    public void Cancel()
    {
       // challenges.Confirm(challenges.userData[id].playerName, challenges.userData[id].facebookID);   
    }
}
