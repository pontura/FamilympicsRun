using UnityEngine;
using System.Collections;

public class LevelData : MonoBehaviour {

    //es solo para cuando jugas un level por challenge pero todavia no lo tenes desbloqueado:
    public bool dontSaveScore;

    public int winnerID;
    public int score;
    public int laps;
    public float time;

    //si es un challenge:
    public string challenge_username;
    public string challenge_facebookID;
    public string challenge_objectID;
    public float challenge_op_score;

    public void SetResultValues(int _winnerID, int _laps, float _time)
    {
        this.winnerID = _winnerID;
        this.laps = _laps;
        this.time = _time; 
    }
    public void CreateChallenge(string username, string facebookID)
    {
        this.challenge_username = username;
        this.challenge_facebookID = facebookID;
    }
    public void ResetChallenge()
    {
        challenge_username = "";
        challenge_facebookID = "";
        challenge_objectID = "";
        challenge_op_score = 0;
    }
    public void RematchChallenge()
    {
        winnerID = 0;
        laps = 0;
        time = 0;
        score = 0;
    }
}
