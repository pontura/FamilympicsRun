using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LevelSelector : MonoBehaviour {

    public GameObject loginButton;

	void Start () {
        if (Data.Instance.userData.logged) 
            loginButton.SetActive(false);
	}

    public void StartLevel(int id)
    {
        Application.LoadLevel("Players");
        Data.Instance.GetComponent<Levels>().currentLevel = id;
    }
    public void Login()
    {
        Application.LoadLevel("Login");
    }
    public void SaveHighScore()
    {
        int hiscore = 777;
        ParseObject gameScore = new ParseObject("Level_1");

        gameScore.Increment("score", hiscore);
        gameScore["playerName"] = Data.Instance.userData.username;
        gameScore["facebookID"] = Data.Instance.userData.facebookID;
        //gameScore["username"] = ParseUser.CurrentUser.Username;
        gameScore["objectId"] = ParseUser.CurrentUser.Username;

        gameScore.SaveAsync();
    }
}
