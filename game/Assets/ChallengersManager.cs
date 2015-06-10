using UnityEngine;
using System.Collections;
using Parse;

public class ChallengersManager : MonoBehaviour {

	void Start () {
        Events.OnChallengeCreate += OnChallengeCreate;
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
}
