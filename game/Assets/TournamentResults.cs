using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TournamentResults : MonoBehaviour {

    public GameObject panel;

    public MultiplayerResultLine puesto1;
    public MultiplayerResultLine puesto2;
    public MultiplayerResultLine puesto3;
    public MultiplayerResultLine puesto4;

    [Serializable]
    public class PlayerScore
    {
        public int id;
        public int score;
    }
    public List<PlayerScore> players;


	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(false);
        Events.OnTournamentFinish += OnTournamentFinish;
	}
    void OnDestroy()
    {
        Events.OnTournamentFinish -= OnTournamentFinish;
    }
    public void OnTournamentFinish()
    {
        if (!Data.Instance.tournament.isOn) return;

        List<Tournament.Hiscore> hiscores = Data.Instance.tournament.hiscores;

        if (hiscores.Count == 0)
        {
            Data.Instance.Load("LevelSelector");
            return;
        }

        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");           

        foreach (int id in hiscores[0].playersID)
        {
            PlayerScore playerscore = new PlayerScore();
            playerscore.id = id;
            players.Add(playerscore);
        }
        foreach(Tournament.Hiscore hiscore in hiscores)
        {
            int winner = hiscore.playersID[0];

            foreach(PlayerScore playersScore in players)
            {
                if (playersScore.id == winner)
                    playersScore.score++;
            }
        }

        players = players.OrderBy(x => x.score).ToList();
        players.Reverse();

        MultiplayerData multiplayerData = Data.Instance.multiplayerData;
        puesto1.Init(multiplayerData.GetPlayer(players[0].id).username, "", multiplayerData.GetPlayer(players[0].id).color);
        puesto2.Init(multiplayerData.GetPlayer(players[1].id).username, "", multiplayerData.GetPlayer(players[1].id).color);

        if (players.Count < 3)
        {
            puesto3.gameObject.SetActive(false);
            puesto4.gameObject.SetActive(false);
        }
        else if (players.Count < 4)
        {
            puesto3.Init(multiplayerData.GetPlayer(players[2].id).username, "", multiplayerData.GetPlayer(players[2].id).color);
            puesto4.gameObject.SetActive(false);
        }
        else
        {
            puesto3.Init(multiplayerData.GetPlayer(players[2].id).username, "", multiplayerData.GetPlayer(players[2].id).color);
            puesto4.Init(multiplayerData.GetPlayer(players[3].id).username, "", multiplayerData.GetPlayer(players[3].id).color);
        }
         
    }
    public void Close()
    {
        Data.Instance.Load("LevelSelector");
    }

}
