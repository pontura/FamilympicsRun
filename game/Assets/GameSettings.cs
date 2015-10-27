using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class GameSettings : MonoBehaviour  {

    [Serializable]
    public class Player
    {
        public float initialAacceleration = 0.2f;
        public float initialDeceleration = 1.2f;
        public float jumpDeceleration = 0.6f;
      //  public float maxSpeed = 2.2f;
        public float speedJump = 1;
    }

    public float wind_stops = 1.2f;
    public float PU_BOOST_Acceleration;
    public float PU_BOOST_duration;

    public float PU_PAUSED_duration;

    public Player player;
    public float LaneSeparation = 2.1f;

    public int stars_for_tournament_2;
    public int stars_for_tournament_3;
    public int stars_for_tournament_4;

    public string GetUsername(string _username)
    {
        _username = _username.ToUpper();
        string[] nameArr = Regex.Split(_username, " ");
        if (nameArr.Length > 0)
        {
            string firstLetter = "";
            if(nameArr.Length>1 && nameArr[1].Length > 0)
                 firstLetter = nameArr[1].Substring(0, 1);
            return nameArr[0] + " " + firstLetter;
        }
        else return _username;
    }
}
