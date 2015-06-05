using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Score : MonoBehaviour {

    public Text usernameTXT;
    public Text scoreInputTXT;

	void Start () {
        var user = ParseUser.CurrentUser;
        IDictionary<string, string> userProfile = user.Get<IDictionary<string, string>>("profile");
        usernameTXT.text = userProfile["name"];

        ParseCloud.CallFunctionAsync<string>("httpRequest", new Dictionary<string, object>()).ContinueWith(t =>
            Debug.Log("received: " + t.Result));

	}
	
	public void SendScore () {
        SaveHighScore(scoreInputTXT.text);
	}
    void SaveNewHiscore(int hiscore)
    {
        ParseObject gameScore = new ParseObject("Scores_Level_1");

        gameScore.Increment("score", hiscore);
        gameScore["userName"] = usernameTXT.text;
        gameScore["playerName"] = ParseUser.CurrentUser.Username;

        gameScore.SaveAsync();
    }
    void SaveHighScore(string score)
    {
        var query = new ParseQuery<ParseObject>("Scores_Level_1")
            .WhereEqualTo ("objectId", "aDxnie8OPJ");

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["score"] = 99999;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("Token has been updated!");
        });


        

    }
    public void backToMain()
    {
        Application.LoadLevel("Menu");
    }
}
