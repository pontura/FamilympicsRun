using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengesLine : MonoBehaviour
{
    public string objectID;
    public string facebookID;
    public Text usernameLabel;
    public Text scoreLabel;
    public ProfilePicture profilePicture;
    public int id = 0;
    private Challenges challenges;
    public bool infoLoaded;

    public void Init(Challenges challenges, int _id)
    {
        this.challenges = challenges;
        this.id = _id;


    }
    void Update()
    {
        if (infoLoaded) return;
        if (challenges.userData[id].facebookID != "")
        {
            usernameLabel.text = challenges.userData[id].playerName;
            scoreLabel.text = Data.Instance.levelsData.GetScoreString(challenges.userData[id].level, challenges.userData[id].score);
            profilePicture.setPicture(challenges.userData[id].facebookID);
            infoLoaded = true;
        }
    }
    public void Accept()
    {
      //  challenges.Confirm(challenges.userData[id].playerName, challenges.userData[id].facebookID);
    }
    public void Cancel()
    {
       // challenges.Confirm(challenges.userData[id].playerName, challenges.userData[id].facebookID);   
    }
}
