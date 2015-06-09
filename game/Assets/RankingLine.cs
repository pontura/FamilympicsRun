using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class RankingLine : MonoBehaviour {

    public Text username;
    public Text score;
    public ProfilePicture picture;

    public void Init(int levelID, string _username, string _score, string _facebookID)
    {
        if (_username == null) return;

        string[] nameArr = Regex.Split(_username, " ");

        this.username.text = nameArr[0];

        if (Data.Instance.levels.levels[levelID].totalLaps > 0)
        {
            this.score.text = GetTimer(float.Parse(_score));
        }
        else
        {
            this.score.text = _score;
        }
        picture.setPicture(_facebookID);
    }
    private string GetTimer(float timer)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        return string.Format("{0:00}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds);
    }
}
