using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour {

    public RankingLine rankingLine;
    public GameObject container;
    private MultiplayerData multiplayerData;

	void Start () {
        multiplayerData = Data.Instance.GetComponent<MultiplayerData>();
	}
    public void LoadSinglePlayerWinners(int levelID)
    {
        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (!FB.IsLoggedIn) return;

        //List<LevelsData.ScoreData> scoresData = Data.Instance.levelsData.LoadFacebookHiscores(levelID);

        //foreach(LevelsData.ScoreData scoreData in scoresData)
        //{
        //      AddPlayer(scoreData.playerName, scoreData.score.ToString(), -1);
        //}

        List<LevelsData.ScoreData> scoresData = Data.Instance.levelsData.levelsScore[levelID].scoreData;

        foreach (LevelsData.ScoreData scoreData in scoresData)
        {
            AddPlayer(scoreData.playerName, scoreData.score.ToString(), -1);
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
            AddPlayer(data.username, data.score.ToString(), data.playerID);
        }
    }


    void AddPlayer(string username, string score, int playerId)
    {
        RankingLine rl = Instantiate(rankingLine) as RankingLine;
        rl.transform.SetParent(container.transform);
        rl.transform.localScale = Vector3.one;
        rl.Init(0, username, score, "");

        if(playerId>-1)
             rl.SetMultiplayerColor(playerId);
    }
   
}
