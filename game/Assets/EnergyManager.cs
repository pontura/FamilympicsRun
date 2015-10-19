using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soomla.Store;

public class EnergyManager : MonoBehaviour {

    public int ENERGY_TO_START = 10;
    public int MINUTES_TO_ENERGY = 20;
    public int MAX_ENERGY;
    public int energy;
    public string timerString;

    public int diffTimestamp;
    public int lasTimeStamp;
    public int nowTimeStamp;


    private string playerPref_TIME = "energyTimestamp";
    private string playerPref_ENERGY = "energy";

    private float SECONDS_TO_ENERGY;

    


    void Start () {

        Events.StartGame += StartGame;
        Events.ReFillEnergy += ReFillEnergy;
        Events.SendEnergyTo += SendEnergyTo;
        Events.RejectEnergyTo += RejectEnergyTo;
        Events.BuyEnergyPack += BuyEnergyPack;
        StoreEvents.OnMarketPurchase += onMarketPurchase;

        SECONDS_TO_ENERGY = MINUTES_TO_ENERGY * 60;
        lasTimeStamp = PlayerPrefs.GetInt(playerPref_TIME);

        if (PlayerPrefs.HasKey(playerPref_ENERGY))
        {
            energy = PlayerPrefs.GetInt(playerPref_ENERGY);
             print("energy grabada " + energy);
        }
        else
        {
            energy = ENERGY_TO_START;
            SaveEnergy();
        }

        Loop();
	}
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.ReFillEnergy -= ReFillEnergy;
        Events.SendEnergyTo -= SendEnergyTo;
        Events.RejectEnergyTo -= RejectEnergyTo;
        Events.BuyEnergyPack = BuyEnergyPack;
        StoreEvents.OnMarketPurchase -= onMarketPurchase;
    }

    void StartGame()
    {
        if (energy > 0)
            ConsumeEnergy();
    }
    void ConsumeEnergy()
    {
        print("_______________  ConsumeEnergy : ConsumeEnergy");
        energy--;
        SaveEnergy();
        SaveNewTime();
    }
    public bool ReplayCheck()
    {
        if (energy < 1)
        {
            Data.Instance.Load("LevelSelector");
            return false;
        }
        return true;
    }
    void Loop()
    {
        timerString = "00:00";
        if (energy < MAX_ENERGY)
        {
            var epochStart = new System.DateTime(2010, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            nowTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            diffTimestamp = nowTimeStamp - lasTimeStamp;

            System.TimeSpan t = System.TimeSpan.FromSeconds(SECONDS_TO_ENERGY - diffTimestamp);

            timerString = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);

            

            if (diffTimestamp >= SECONDS_TO_ENERGY)
            {
                float energyToWin = Mathf.Floor(diffTimestamp / SECONDS_TO_ENERGY);

                energy += (int)energyToWin;

                if (energy >= MAX_ENERGY)
                {
                    energy = MAX_ENERGY;
                    diffTimestamp = 0;
                }

                SaveEnergy();
                SaveNewTime();
            }
        }
        Invoke("Loop", 1);
    }
    void SaveEnergy()
    {
        PlayerPrefs.SetInt(playerPref_ENERGY, energy);
        print("GRABA playerPref_ENERGY:" +  energy);
        Events.OnEnergyWon();
    }
    void SaveNewTime()
    {
        var epochStart = new System.DateTime(2010, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        lasTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        PlayerPrefs.SetInt(playerPref_TIME, lasTimeStamp);
    }
    public void BuyEnergyPack()
    {
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

        if (pvi.ID == "energy1")
            ReFillEnergy(10);
    }
    public void ReFillEnergy(int qty)
    {
        energy += qty;
        if (energy > MAX_ENERGY) energy = MAX_ENERGY;
        SaveEnergy();
       // Data.Instance.Load("LevelSelector");
    }
    void SendEnergyTo(string facebookID)
    {
        UpdataNotification(facebookID, Data.Instance.userData.facebookID, "1");
        Data.Instance.facebookShare.ShareToFriend(facebookID, Data.Instance.userData.username + " sent you energy! Get back in the game.");
    }
    void RejectEnergyTo(string facebookID)
    {
        UpdataNotification(facebookID, Data.Instance.userData.facebookID , "2");
        Data.Instance.facebookShare.ShareToFriend(facebookID, Data.Instance.userData.username + " rejected sending you energy!");
    }
    void UpdataNotification(string facebookID, string asked_facebookID, string status)
    {
        var query = new ParseQuery<ParseObject>("Notifications")
            .WhereEqualTo("facebookID", facebookID)
            .WhereEqualTo("asked_facebookID", asked_facebookID);

        query.FindAsync().ContinueWith(t =>
        {
            IEnumerator<ParseObject> enumerator = t.Result.GetEnumerator();
            enumerator.MoveNext();
            var data = enumerator.Current;
            data["status"] = status;
            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("On Notifications to facebookID: " + facebookID + " asked_facebookID : " + asked_facebookID + " status: " + status);
        });
    }
    
}
