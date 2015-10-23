using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tournament : MonoBehaviour {

    [Serializable]
    public class Hiscore
    {
        public List<int> playersID;
    }
    public List<Hiscore> hiscores;
    public bool isOn;
    public int tournamentID;

	void Start () {
        Events.OnTournamentStart += OnTournamentStart;
        Events.OnTournamentAddScores += OnTournamentAddScores;
	    Events.OnTournamentFinish += OnTournamentFinish;
	}
    public int GetTotalMatches()
    {
        return hiscores.Count;
    }
    void OnTournamentStart(int tournamentID)
    {
        this.tournamentID = tournamentID;
        isOn = true;
        hiscores.Clear();
    }
    void OnTournamentAddScores(int levelID, List<int> playersID)
    {
        if (!isOn) return;
        Hiscore hiscore = new Hiscore();
        hiscore.playersID = playersID;
        int id = levelID-1;
        if (hiscores.Count == id)
            hiscores.Add(hiscore);
        else
            hiscores[id] = hiscore;
    }
    void OnTournamentFinish()
    {
        //...
    }
    public void Reset()
    {
        isOn = false;
        hiscores.Clear();
    }

}
