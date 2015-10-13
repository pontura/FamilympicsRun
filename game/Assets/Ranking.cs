using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour {

    public RankingLine rankingLine;
    public GameObject container;
    private MultiplayerData multiplayerData;
    private int levelID;

	void Start () {
        multiplayerData = Data.Instance.GetComponent<MultiplayerData>();
	}
    public void LoadSinglePlayerWinners(int levelID)
    {
        this.levelID = levelID;
        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (!FB.IsLoggedIn) return;

        List<LevelsData.ScoreData> scoresData = Data.Instance.levelsData.levelsScore[levelID].scoreData;

        foreach (LevelsData.ScoreData scoreData in scoresData)
        {
            AddPlayer(scoreData.playerName, scoreData.score.ToString(), -1, scoreData.facebookID);
        }
    }

    public void LoadMultiplayerWinners(int id)
    {
        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<MultiplayerData.HiscoresData> hiscoreData = multiplayerData.hiscoreLevels[id].hiscores;

        foreach (MultiplayerData.HiscoresData data in hiscoreData)
        {
            AddPlayer(data.username, data.score.ToString(), data.playerID, "");
        }
    }


    void AddPlayer(string username, string score, int playerId, string facebookID)
    {
        //Debug.Log(username + " scoRe: " + score + " playerId: " + playerId + " facebookID: " + facebookID);

        RankingLine rl = Instantiate(rankingLine) as RankingLine;
        rl.transform.SetParent(container.transform);
        rl.transform.localScale = Vector3.one;
        rl.Init(levelID, username, score, facebookID);

        if(playerId>-1)
             rl.SetMultiplayerColor(playerId);
        else
            rl.SetSinglePlayer();
    }
   
}
