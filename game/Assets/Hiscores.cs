using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Parse;


public class Hiscores : MonoBehaviour {

   // IEnumerable<ParseObject> results;
    public Text hiscoresTXT;

	void Start () {
        GetScores();
	}

    void GetScores()
    {
        var query = ParseObject.GetQuery("Scores_Level_1")
            .OrderByDescending("score")
            .Limit(3);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerable<ParseObject> results = t.Result;
           foreach (var result in results)
           {
               Debug.Log("Score: " + result["score"] + " and Name: " + result["playerName"]);
           }
        });        
    }

    public void backToMain()
    {
        Application.LoadLevel("Menu");
    }
}
