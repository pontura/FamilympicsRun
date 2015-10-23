using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class Tournaments : MonoBehaviour {

    public GameObject[] tournamentButtons;
    public ScrollLimit scrollLimit;
 
    public int limitScrollSeason1 = -926;
    public int limitScrollSeason2 = -2100;
    public int limitScrollSeason3 = -3200;
  //  public int limitScrollSeason4 = -4824;

    public GameObject lock1;
    public GameObject lock2;
  //  public GameObject lock3;

    public Text text1;
    public Text text2;
   // public Text text3;

    public void Init()
    {
        CheckForStarsAndThenStart();
        if( Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            SetTournamentButtons(false);

        Events.OnChangePlayMode += OnChangePlayMode;
        StoreEvents.OnMarketPurchase += onMarketPurchase;
    }
    void OnDestroy()
    {
        Events.OnChangePlayMode -= OnChangePlayMode;
        StoreEvents.OnMarketPurchase -= onMarketPurchase;
    }
    public void Buy(int id)
    {
        Debug.Log("BUY season: " + id);
        StoreInventory.BuyItem("season2unlock");
    }
    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {
        // pvi - the PurchasableVirtualItem that was just purchased
        // payload - a text that you can give when you initiate the purchase operation and
        //    you want to receive back upon completion
        // extra - contains platform specific information about the market purchase
        //    Android: The "extra" dictionary will contain: 'token', 'orderId', 'originalJson', 'signature', 'userId'
        //    iOS: The "extra" dictionary will contain: 'receiptUrl', 'transactionIdentifier', 'receiptBase64', 'transactionDate', 'originalTransactionDate', 'originalTransactionIdentifier'

        if (pvi.ID == "season3unlock" || pvi.ID == "season2unlock")
            Data.Instance.Load("LevelSelector");
    }
    void CheckForStarsAndThenStart()
    {
        text1.text = "COMPLETE SEASON 1 WITH AT LEAST " + Data.Instance.gameSettings.stars_for_tournament_2 + " STARS TO UNLOCK";
        text2.text = "COMPLETE SEASON 2 WITH AT LEAST " + Data.Instance.gameSettings.stars_for_tournament_3 + " STARS TO UNLOCK";
       // text3.text = "COMPLETE SEASON 3 WITH AT LEAST " + Data.Instance.gameSettings.stars_for_tournament_4 + " STARS TO UNLOCK";
        
        scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason1));

        int tournamentAvailable = Data.Instance.userData.GetTournamentAvailable();

      //  print("_______________" + Data.Instance.userData.levelProgressionId + " __________   " + stars);

        if (tournamentAvailable == 2)
        {
            Destroy(lock1);
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason2));
        } else
        if (tournamentAvailable >2)
        {
            Destroy(lock1);
            Destroy(lock2);
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason3));
        }        

        Invoke("CheckForStarsAndThenStart", 2);
	}
    
    public void PlayTournament(int id)
    {
        int levelID = 1;
        switch (id)
        {
            case 1: levelID = 1; break;
            case 2: levelID = 9; if (Data.Instance.userData.levelProgressionId < 8) return; break;
            case 3: levelID = 17; if (Data.Instance.userData.levelProgressionId < 16) return; break;
            case 4: if (Data.Instance.userData.levelProgressionId < 24) return; break;
        }
        Events.OnTournamentStart(levelID);
        Data.Instance.GetComponent<Levels>().currentLevel = levelID;
        Data.Instance.Load("Players");
    }
    void OnChangePlayMode(UserData.modes mode)
    {
        switch (mode)
        {
            case UserData.modes.MULTIPLAYER:
                SetTournamentButtons(true);
                break;
            case UserData.modes.SINGLEPLAYER:
                SetTournamentButtons(false);
                break;
        }
    }
    void SetTournamentButtons(bool on)
    {
        foreach (GameObject button in tournamentButtons)
            button.SetActive(on);
    }
}
