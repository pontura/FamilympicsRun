using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soomla.Store;
using Soomla.Store.Example;

public class Buy : MonoBehaviour {

	void Start () {
        StoreEvents.OnMarketPurchase += onMarketPurchase;
	}
    void OnDestroy()
    {
        StoreEvents.OnMarketPurchase -= onMarketPurchase;
    }
    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
    public void BuyEnergy(int qty)
    {
        switch (qty)
        {
            case 1:     StoreInventory.BuyItem(StoreAssets.ENERGY_1_PRODUCT_ID); break;
            case 3:     StoreInventory.BuyItem(StoreAssets.ENERGY_3_PRODUCT_ID); break;
            case 10:    StoreInventory.BuyItem(StoreAssets.ENERGY_10_PRODUCT_ID); break;
        }
    }
    public void BuySeason()
    {
        if(Data.Instance.userData.GetTournamentAvailable()==1)
            StoreInventory.BuyItem(StoreAssets.SEASON_2_UNLOCK_PRODUCT_ID);
        else
            StoreInventory.BuyItem(StoreAssets.SEASON_3_UNLOCK_PRODUCT_ID);
    }
    public void BuyAllSeasons()
    {
        StoreInventory.BuyItem(StoreAssets.SEASON_2_UNLOCK_PRODUCT_ID);
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
        {
            Events.ReFillEnergy(10);
        }
        else if (pvi.ID == "energy3")
        {
            Events.ReFillEnergy(10);
            Events.AddPlusEnergy(3);
        }
        else if (pvi.ID == "energy10")
        {
            Events.ReFillEnergy(10);
            Events.AddPlusEnergy(10);
        }
        Data.Instance.Load("MainMenu");
    }
}
