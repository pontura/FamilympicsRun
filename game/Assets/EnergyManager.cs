using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soomla.Store;

public class EnergyManager : MonoBehaviour {

    public int energy;
    public int plusEnergy;

    public int ENERGY_TO_START = 10;
    public int MINUTES_TO_ENERGY = 20;
    public int MAX_ENERGY;
    
    public string timerString;

    public int diffTimestamp;
    public int lasTimeStamp;
    public int nowTimeStamp;


    private string playerPref_TIME = "energyTimestamp";
    private string playerPref_ENERGY = "energy";
    private string playerPref_PLUS_ENERGY = "plusEnergy";

    private float SECONDS_TO_ENERGY;

    


    void Start () {

        Events.StartGame += StartGame;
        Events.ReFillEnergy += ReFillEnergy;
        Events.SendEnergyTo += SendEnergyTo;
        Events.RejectEnergyTo += RejectEnergyTo;
        Events.BuyEnergyPack += BuyEnergyPack;
        Events.AddPlusEnergy += AddPlusEnergy;

        SECONDS_TO_ENERGY = MINUTES_TO_ENERGY * 60;
        lasTimeStamp = PlayerPrefs.GetInt(playerPref_TIME);

        if (PlayerPrefs.HasKey(playerPref_ENERGY))
            energy = PlayerPrefs.GetInt(playerPref_ENERGY);
        else
        {
            energy = ENERGY_TO_START;
            SaveEnergy();
        }

        if (PlayerPrefs.HasKey(playerPref_PLUS_ENERGY))
            plusEnergy = PlayerPrefs.GetInt(playerPref_PLUS_ENERGY);
        else plusEnergy = 0;

        Loop();
        
	}
    public void AddPlusEnergy(int qty)
    {
        plusEnergy += qty;
        SavePlusEnergy();
    }
    void SavePlusEnergy()
    {
        PlayerPrefs.SetInt(playerPref_PLUS_ENERGY, plusEnergy);
    }
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
        Events.ReFillEnergy -= ReFillEnergy;
        Events.SendEnergyTo -= SendEnergyTo;
        Events.RejectEnergyTo -= RejectEnergyTo;
        Events.BuyEnergyPack = BuyEnergyPack;
        Events.AddPlusEnergy -= AddPlusEnergy;
       
    }
    public void ConsumePlusEnergy()
    {
        print("_______________  ConsumePlusEnergy");
        energy = 10;
        plusEnergy--;
        SaveEnergy();
        SaveNewTime();
        SavePlusEnergy();
    }
    void StartGame()
    {
        if (Data.Instance.tournament.isOn)
        {
            print("energy manager StartGame: " + Data.Instance.tournament.GetTotalMatches());
            if (Data.Instance.tournament.GetTotalMatches() == 0)
                ConsumeEnergy(8);
        } else
            ConsumeEnergy(1);
    }
    void ConsumeEnergy(int qty)
    {

        if (energy > qty-1)
            energy-=qty;
        else 
        if (plusEnergy > 0)
        {
            energy = 10 - (qty - energy);
            plusEnergy--;
            SavePlusEnergy();
        }
       
        SaveEnergy();
        SaveNewTime();
    }
    public bool ReplayCheck()
    {
        if (energy < 1 && plusEnergy == 0)
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
        Events.OnEnergyWon();
    }
    void SaveNewTime()
    {
        var epochStart = new System.DateTime(2010, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        lasTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        PlayerPrefs.SetInt(playerPref_TIME, lasTimeStamp);
    }
    public void BuyEnergyPack(int qty)
    {
        
    }
    public void ReFillEnergy(int qty)
    {
        energy += qty;
        if (energy > MAX_ENERGY)
        {
            energy = 1;
            plusEnergy++;
            Debug.Log("ReFillEnergy energy " + energy + "     plusEnergy: " + plusEnergy);
        }
        SaveEnergy();
        SavePlusEnergy();
       // Data.Instance.Load("LevelSelector");
    }
    void SendEnergyTo(string facebookID)
    {
        UpdataNotification(facebookID, Data.Instance.userData.facebookID, "1");
        Data.Instance.facebookShare.ShareToFriend(facebookID, Data.Instance.userData.username + " sent you energy! Get back in the game.");
    }
    void RejectEnergyTo(string facebookID)
    {
       // UpdataNotification(facebookID, Data.Instance.userData.facebookID , "2");
        Data.Instance.notifications.DeleteNotification(facebookID, Data.Instance.userData.facebookID);
       // Data.Instance.facebookShare.ShareToFriend(facebookID, Data.Instance.userData.username + " rejected sending you energy!");
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

            if(status == "1")
                data["notificated"] = false;

            return data.SaveAsync();
        }).Unwrap().ContinueWith(t =>
        {
            Debug.Log("On Notifications to facebookID: " + facebookID + " asked_facebookID : " + asked_facebookID + " status: " + status);
        });
    }
    
}
