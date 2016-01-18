using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class MultiplayerData : MonoBehaviour {

    [Serializable]
    public class PlayerData
    {
        public int playerID;
        public string username;
        public int meters;
        public float time;
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
    public int activePlayerId;
    public List<HiscoreLevel> hiscoreLevels;

    public string playerName1;
    public string playerName2;
    public string playerName3;
    public string playerName4;
    
	// Use this for initialization
    public void Init()
    {
        playerName1 = PlayerPrefs.GetString("playerName1");
        playerName2 = PlayerPrefs.GetString("playerName2");
        playerName3 = PlayerPrefs.GetString("playerName3");
        playerName4 = PlayerPrefs.GetString("playerName4");

        for (int i = 0; i < Data.Instance.levels.levels.Length; i++)
        {
            HiscoreLevel hiscoreLevel = new HiscoreLevel();
            hiscoreLevels.Add(hiscoreLevel);
            hiscoreLevel.hiscores = new List<HiscoresData>();
           // hiscoreLevel.hiscores = new List<HiscoresData>();
            //for (int a = 0; a < 3; a++)
            //{
            //    HiscoresData hiscoresData = new HiscoresData();
            //    hiscoreLevel.hiscores.Add( hiscoresData );
            //}
        }
        LeadPlayerPrefs();
	}
    void Start()
    {
        Events.OnAddMultiplayerScore += OnAddMultiplayerScore;
        Events.StartGame += StartGame;
        Events.OnAvatarDie += OnAvatarDie;
    }
    void OnDestroy()
    {
        Events.OnAddMultiplayerScore -= OnAddMultiplayerScore;
        Events.StartGame -= StartGame;
        Events.OnAvatarDie -= OnAvatarDie;
    }
    void OnAvatarDie(Player player)
    {
        float timer = GameObject.Find("UICanvas").GetComponent<Chronometer>().timer;
        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER) return;
            GetPlayer(player.id).time = timer;
    }
    void StartGame()
    {
        foreach (PlayerData data in players)
            data.meters = 0;
    }
    public void Reset()
    {
        print("RESET");
        foreach (HiscoreLevel hiscoreLevel in hiscoreLevels)
        {
            hiscoreLevel.lastWinner = 0;
            hiscoreLevel.hiscores.Clear();
        }
    }
    void OnAddMultiplayerScore(int levelID, float score, int playerID, string username)
    {
        //print("OnAddMultiplayerScore: " + levelID + " score:" + score + " playerID:" + playerID + " username:" + username);

        HiscoreLevel hiscoreLevel = hiscoreLevels[levelID];
        hiscoreLevel.lastWinner = playerID;

        int num = 1;

        HiscoresData hiscoresData = new HiscoresData();
        hiscoresData.levelID = levelID;
        hiscoresData.score = score;
        hiscoresData.username = username;
        hiscoresData.playerID = playerID;
        
        hiscoreLevel.hiscores.Add(hiscoresData);
        ArrengeListByScore(levelID);

        SavePlayerPrefs();
    }
    void SavePlayerPrefs()
    {
        foreach (HiscoreLevel hiscoreLevel in hiscoreLevels)
        {
            int a = 1;
            foreach (HiscoresData hiscoresData in hiscoreLevel.hiscores)
            {
                if (a <= 10)
                {
                    string strName = "Multi_" + hiscoresData.levelID + "_" + a;
                    string strValue = hiscoresData.playerID + "_" + hiscoresData.username + "_" + hiscoresData.score;
                    PlayerPrefs.SetString(strName, strValue);
                }
                a++;
            }
        }
    }
    void LeadPlayerPrefs()
    {
        for (int i = 0; i < Data.Instance.levels.levels.Length; i++)
        {
            for (int a = 0; a < 10; a++)
            {
                HiscoresData newHiscoresData = LeadPlayerPrefData( i+1, a+1);
                if (newHiscoresData != null)
                {
                    hiscoreLevels[i+1].hiscores.Add(newHiscoresData);
                }
            }
            ArrengeListByScore(i);
        }
    }
    void ArrengeListByScore(int levelId)
    {
        hiscoreLevels[levelId].hiscores = hiscoreLevels[levelId].hiscores.OrderBy(x => x.score).ToList();
        if (Data.Instance.levels.levels[levelId].Sudden_Death) hiscoreLevels[levelId].hiscores.Reverse();
        if (Data.Instance.levels.levels[levelId].totalTime > 0) hiscoreLevels[levelId].hiscores.Reverse();
    }
    public void OnSaveName(string username, int id)
    {
        username = username.ToUpper();
        if (username == "") username = "RUN" + id;

        foreach (PlayerData data in players)
        {
            if(data.playerID == id)
                data.username = username;
        }
        switch (id)
        {
            case 1: playerName1 = username; PlayerPrefs.SetString("playerName1", username); break;
            case 2: playerName2 = username; PlayerPrefs.SetString("playerName2", username); break;
            case 3: playerName3 = username; PlayerPrefs.SetString("playerName3", username); break;
            case 4: playerName4 = username; PlayerPrefs.SetString("playerName4", username); break;
        }
    }

    //llega esto:
    //Multi_1_1   strValue:1_xcv_2.799766
    HiscoresData LeadPlayerPrefData(int levelID, int num)
    {
        HiscoresData hiscoresData = new HiscoresData();
        string result = PlayerPrefs.GetString("Multi_" + levelID + "_" + num);

        if (result.Length < 2) return null;
        string[] nameArr = Regex.Split(result, "_");
        if (nameArr.Length < 1) return null;

        int playerID = int.Parse(nameArr[0]);
        string username = nameArr[1];
        float score = float.Parse(nameArr[2]);

        hiscoresData.levelID = levelID;
        hiscoresData.playerID = playerID;
        hiscoresData.username = username;
        hiscoresData.score = score;

        return hiscoresData;
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
        

        switch (id)
        {
            case 1: data.username = playerName1; break;
            case 2: data.username = playerName2; break;
            case 3: data.username = playerName3; break;
            case 4: data.username = playerName4; break;
        }
        if(data.username == "") data.username = "RUN" + id;
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
    public void SetMeters(int id, int meters)
    {
        foreach (PlayerData data in players)
            if (data.playerID == id)
                data.meters = meters;
    }
}
