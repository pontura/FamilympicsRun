using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChallengersManager : MonoBehaviour {

    public bool showFacebookFriends;

	void Start () {
        Events.OnChallengeCreate += OnChallengeCreate;
        Events.OnChallengeClose += OnChallengeClose;
	}

    public void OnChallengeCreate(string oponent_username, string oponent_facebookID, int level, float score)
    {
        ParseObject data = new ParseObject("Challenges");
        data["playerName"] = Data.Instance.userData.username;
        data["facebookID"] = Data.Instance.userData.facebookID;
        
        data["op_playerName"] = oponent_username;
        data["op_facebookID"] = oponent_facebookID;

        data["level"] = level;
        data["score"] = score;

        data.SaveAsync();
        print("Challenge Saved");
    }
    public void OnChallengeClose(string objectID, string op_facebookID, string winner, float newScore)
    {

        var query = new ParseQuery<ParseObject>("Challenges")
            .WhereEqualTo("objectId", objectID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["score2"] = newScore;
            data["winner"] = winner;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Score updated!");
        });   
    }
}
