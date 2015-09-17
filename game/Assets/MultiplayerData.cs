using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;


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
            for (int a = 0; a < 3; a++)
            {
                HiscoresData hiscoresData = new HiscoresData();
                hiscoreLevel.hiscores.Add( hiscoresData );
            }
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
        GetPlayer(player.id).time = timer;
    }
    void StartGame()
    {
        foreach (PlayerData data in players)
            data.meters = 0;
    }
    public void Reset()
    {
        foreach (HiscoreLevel hiscoreLevel in hiscoreLevels)
        {
            hiscoreLevel.lastWinner = 0;
            foreach (HiscoresData hiscoresData in hiscoreLevel.hiscores)
            {
                hiscoresData.levelID = 0;
                hiscoresData.playerID = 0;
                hiscoresData.score = 0;
                hiscoresData.username = "";
            }
        }
    }
    void OnAddMultiplayerScore(int levelID, float score, int playerID, string username)
    {
        print("OnAddMultiplayerScore: " + levelID + " score:" + score + " playerID:" + playerID + " username:" + username);

        HiscoreLevel hiscoreLevel = hiscoreLevels[levelID];
        hiscoreLevel.lastWinner = playerID;

        bool newHiscore = false;
        HiscoresData newHiscoresData = null;

        print(levelID + " multi: " + score);

        int num = 1;
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
                SavePlayerPrefs(levelID, num, newHiscoresData.playerID, newHiscoresData.username, newHiscoresData.score);

                newHiscoresData = hiscoresDataToReplace;
            } else 
            if (
                (Data.Instance.levels.GetCurrentLevelData().totalTime > 0 && hiscoresData.score < score)
                ||
                (Data.Instance.levels.GetCurrentLevelData().totalLaps > 0 && (hiscoresData.score > score || hiscoresData.score == 0))
                ||
                (Data.Instance.levels.GetCurrentLevelData().Sudden_Death && (hiscoresData.score < score))
                )
            {
                newHiscore = true;
                newHiscoresData = new HiscoresData();

                newHiscoresData.levelID = hiscoresData.levelID;
                newHiscoresData.score = hiscoresData.score;
                newHiscoresData.username = hiscoresData.username;
                newHiscoresData.playerID = hiscoresData.playerID;

                SavePlayerPrefs(levelID, num, playerID, username, score);

                hiscoresData.levelID = levelID;
                hiscoresData.score = score;
                hiscoresData.username = username;
                hiscoresData.playerID = playerID;
            }
            num++;
        }
    }
    void SavePlayerPrefs(int levelID, int num, int playerID, string username, float score)
    {
        string strName = "Multi_" + levelID + "_" + num;
        string strValue = playerID + "_" + username + "_" + score;
        PlayerPrefs.SetString(strName, strValue);
        print("______________" + strName + "   strValue: " + strValue);
    }
    void LeadPlayerPrefs()
    {
        for (int i = 0; i < Data.Instance.levels.levels.Length; i++)
        {
            for (int a = 0; a < 3; a++)
            {
                LeadPlayerPrefData( i + 1, a+1);
            }
        }
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
    void LeadPlayerPrefData(int levelID, int num)
    {
        string result = PlayerPrefs.GetString("Multi_" + levelID + "_" + num);

        if (result.Length < 2) return;
        string[] nameArr = Regex.Split(result, "_");
        if (nameArr.Length < 1) return;

        int playerID = int.Parse(nameArr[0]);
        string username = nameArr[1];
        float score = float.Parse(nameArr[2]);

        hiscoreLevels[levelID].hiscores[num - 1].levelID = levelID;
        hiscoreLevels[levelID].hiscores[num - 1].playerID = playerID;
        hiscoreLevels[levelID].hiscores[num - 1].username = username;
        hiscoreLevels[levelID].hiscores[num - 1].score = score;
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
