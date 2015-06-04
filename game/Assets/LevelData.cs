using UnityEngine;
using System.Collections;

public class LevelData : MonoBehaviour {

    public int numPlayers;
    public int winnerID;
    public int score;
    public int laps;
    public float time;

    public void SetResultValues(int _winnerID, int _laps, float _time)
    {
        this.winnerID = _winnerID;
        this.laps = _laps;
        this.time = _time; 
    }
}
