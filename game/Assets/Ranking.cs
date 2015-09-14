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
        
        LevelsData.LevelsScore levelScore = Data.Instance.levelsData.levelsScore[levelID];

        if (levelScore.scoreData1.playerName != "")
            AddPlayer(levelScore.scoreData1.playerName, levelScore.scoreData1.score.ToString(), -1);
        if (levelScore.scoreData2.playerName != "")
            AddPlayer(levelScore.scoreData2.playerName, levelScore.scoreData2.score.ToString(), -1);
        if (levelScore.scoreData3.playerName != "")
            AddPlayer(levelScore.scoreData3.playerName, levelScore.scoreData3.score.ToString(), -1);

    }
    public void LoadMultiplayerWinners(int id)
    {
        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<MultiplayerData.HiscoresData> hiscoreData = multiplayerData.hiscoreLevels[id].hiscores;

        if (hiscoreData[0].score > 0)
            AddPlayer(hiscoreData[0].username, hiscoreData[0].score.ToString(), hiscoreData[0].playerID);
        if (hiscoreData[1].score > 0)
            AddPlayer(hiscoreData[1].username, hiscoreData[1].score.ToString(), hiscoreData[1].playerID);
        if (hiscoreData[2].score > 0)
            AddPlayer(hiscoreData[2].username, hiscoreData[2].score.ToString(), hiscoreData[2].playerID);
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
