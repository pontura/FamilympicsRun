using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class RankingLine : MonoBehaviour {

    public float score_float;
    public Image circleImage;
    public Text username;
    public Text score;
    public ProfilePicture picture;
    public string facebookID;
    public string playerName;
    public GameObject maskImage;

    public void Init(int levelID, string _username, string _score, string _facebookID)
    {
        this.score_float = float.Parse(_score);
        if (_username == null) return;

        this.facebookID = _facebookID;
        this.playerName = Data.Instance.gameSettings.GetUsername(_username);

        this.username.text = playerName;

      //  print("score: " + _score);
        this.score.text = Data.Instance.levelsData.GetScoreString(levelID, float.Parse(_score));
      //  print("levelID: " + levelID + "   score: " +  Data.Instance.levelsData.GetScoreString(levelID, float.Parse(_score)));
        
    }
    public void SetSinglePlayer()
    {
        maskImage.SetActive(true);
        circleImage.gameObject.SetActive(false);
        picture.gameObject.SetActive(true);
        picture.setPicture(facebookID);
    }
    public void SetMultiplayerColor(int playerID)
    {
        maskImage.SetActive(false);
        circleImage.gameObject.SetActive(true);
        if (playerID == 0)
        {
            Debug.Log("SetMultiplayerColor " + playerID);
            return;
        }
        Color color = Data.Instance.colors[playerID-1];
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        circleImage.color = color;
    }
}
