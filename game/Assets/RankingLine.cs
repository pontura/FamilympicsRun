using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class RankingLine : MonoBehaviour {

    public Text username;
    public Text score;
    public ProfilePicture picture;

    public void Init(string _username, string _score, string _facebookID)
    {
        if (_username == null) return;

        string[] nameArr = Regex.Split(_username, " ");

        this.username.text = nameArr[0];
        this.score.text = _score;
        picture.setPicture(_facebookID);
    }
}
