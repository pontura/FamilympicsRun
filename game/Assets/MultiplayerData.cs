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
    [Serializable]
    public class Players
    {
        public List<PlayerData> playerData;
    }

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
    void OnAddMultiplayerScore(int levelID, float score, int playerID, string username)
    {
        HiscoreLevel hiscoreLevel = hiscoreLevels[levelID];
        hiscoreLevel.lastWinner = playerID;
    }
}
