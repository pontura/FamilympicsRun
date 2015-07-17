using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class RankingLine : MonoBehaviour {

    public Image circleImage;
    public Text username;
    public Text score;
    public ProfilePicture picture;
    public string facebookID;
    public string playerName;
    public GameObject maskImage;

    public void Init(int levelID, string _username, string _score, string _facebookID)
    {
        if (_username == null) return;

        this.facebookID = _facebookID;
        this.playerName = _username;

        string[] nameArr = Regex.Split(_username, " ");

        this.username.text = nameArr[0];

        this.score.text = Data.Instance.levelsData.GetScoreString(levelID, float.Parse(_score));

        if(!Data.Instance.OnlyMultiplayer)
            picture.setPicture(_facebookID);
    }
    public void SetSinglePlayer()
    {
        maskImage.SetActive(true);
        circleImage.gameObject.SetActive(false);
    }
    public void SetMultiplayerColor(int playerID)
    {
        maskImage.SetActive(false);
        circleImage.gameObject.SetActive(true);
        Color color = Data.Instance.colors[playerID-1];
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        circleImage.color = color;
    }
}
