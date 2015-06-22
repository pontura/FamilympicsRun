using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class RankingLine : MonoBehaviour {

    public Text username;
    public Text score;
    public ProfilePicture picture;
    public string facebookID;
    public string playerName;

    public void Init(int levelID, string _username, string _score, string _facebookID)
    {
        if (_username == null) return;

        this.facebookID = _facebookID;
        this.playerName = _username;

        string[] nameArr = Regex.Split(_username, " ");

        this.username.text = nameArr[0];

        this.score.text = Data.Instance.levelsData.GetScoreString(levelID, float.Parse(_score));

        picture.setPicture(_facebookID);
    }
    public void SetMultiplayerColor(int playerID)
    {
        picture.SetMultiplayerColor(playerID);
    }
}
