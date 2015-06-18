using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MultiplayerData : MonoBehaviour {

    [Serializable]
    public class PlayerData
    {
        public int playerID;
        public string username;
        public Color color;
    }
    public List<PlayerData> players;

    [Serializable]
    public class HiscoresData
    {
        public int levelID;
        public float score;
        public int playerID;
        public string username;
    }
    [Serializable]
    public class HiscoreLevel
    {
        public List<HiscoresData> hiscores;
        public int lastWinner;
    }
    public List<HiscoreLevel> hiscoreLevels;
    
	// Use this for initialization
    public void Init()
    {
        for (int i = 0; i < Data.Instance.levels.levels.Length; i++)
        {
            HiscoreLevel hiscoreLevel = new HiscoreLevel();
            hiscoreLevels.Add(hiscoreLevel);
            hiscoreLevel.hiscores = new List<HiscoresData>();
            for (int a = 0; a < 3; a++)
            {
                HiscoresData hiscoresData = new HiscoresData();
                hiscoreLevel.hiscores.Add( hiscoresData );
            }
        }
	}
    void Start()
    {
        Events.OnAddMultiplayerScore += OnAddMultiplayerScore;
    }
    void OnDestroy()
    {
        Events.OnAddMultiplayerScore -= OnAddMultiplayerScore;
    }
    void OnAddMultiplayerScore(int levelID, float score, int playerID, string username)
    {
        HiscoreLevel hiscoreLevel = hiscoreLevels[levelID];
        hiscoreLevel.lastWinner = playerID;

        bool newHiscore = false;
        HiscoresData newHiscoresData = null;

        print(levelID + " multi: " + score);

        foreach (HiscoresData hiscoresData in hiscoreLevel.hiscores)
        {
            print("newHiscore " + newHiscoresData);
            if (newHiscoresData != null)
            {
                HiscoresData hiscoresDataToReplace = new HiscoresData();
                hiscoresDataToReplace.levelID = hiscoresData.levelID;
                hiscoresDataToReplace.score = hiscoresData.score;
                hiscoresDataToReplace.username = hiscoresData.username;
                hiscoresDataToReplace.playerID = hiscoresData.playerID;

                hiscoresData.levelID = newHiscoresData.levelID;
                hiscoresData.score = newHiscoresData.score;
                hiscoresData.username = newHiscoresData.username;
                hiscoresData.playerID = newHiscoresData.playerID;

                newHiscoresData = hiscoresDataToReplace;
            } else 
            if (
                (Data.Instance.levels.GetCurrentLevelData().totalTime > 0 && hiscoresData.score < score)
                ||
                (Data.Instance.levels.GetCurrentLevelData().totalLaps > 0 && (hiscoresData.score > score || hiscoresData.score == 0))
                )
            {
                newHiscore = true;
                newHiscoresData = new HiscoresData();

                newHiscoresData.levelID = hiscoresData.levelID;
                newHiscoresData.score = hiscoresData.score;
                newHiscoresData.username = hiscoresData.username;
                newHiscoresData.playerID = hiscoresData.playerID;

                hiscoresData.levelID = levelID;
                hiscoresData.score = score;
                hiscoresData.username = username;
                hiscoresData.playerID = playerID;
            }
        }
    }
    public void ResetPlayers()
    {
        players.Clear();
    }
    public void AddPlayer(int id)
    {
        PlayerData data = new PlayerData();
        data.playerID = id;
        data.color = Data.Instance.colors[id - 1];
        players.Add(data);
    }
    public void DeletePlayer(int id)
    {
        PlayerData player = null;
        foreach (PlayerData data in players)
        {
            if (data.playerID == id)
                player = data;
        }
        if (player != null)
            players.Remove(player);
    }
    public PlayerData GetPlayer(int id)
    {
        foreach (PlayerData data in players)
            if (data.playerID == id)
                return data;
        return null;
        
    }
}
