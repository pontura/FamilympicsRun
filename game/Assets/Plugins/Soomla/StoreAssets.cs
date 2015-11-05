using UnityEngine;
using System.Collections;

namespace Soomla.Store.Example															//Allows for access to Soomla API
{
    public class StoreAssets : IStoreAssets 											//Extend from IStoreAssets (required to define assets)
    {
        public int GetVersion()
        {														// Get Current Version
            return 1;
        }

        public VirtualCurrency[] GetCurrencies()
        {										// Get/Setup Virtual Currencies
            return new VirtualCurrency[] { };
        }

        public VirtualGood[] GetGoods()
        {												// Add "TURN_GREEN" IAP to GetGoods
            return new VirtualGood[] { SEASON_2_UNLOCK, SEASON_3_UNLOCK, ALL_SEASONS_UNLOCK, ENERGY_1, ENERGY_3, ENERGY_10 };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {								// Get/Setup Currency Packs
            return new VirtualCurrencyPack[] { };
        }

        public VirtualCategory[] GetCategories()
        {										// Get/ Setup Categories (for In App Purchases)
            return new VirtualCategory[] { };
        }

        //****************************BOILERPLATE ABOVE(modify as you see fit/ if nessisary)***********************
        public const string SEASON_2_UNLOCK_PRODUCT_ID = "season2unlock";				//create a string to store the "turn green" in app purchase
        public const string SEASON_3_UNLOCK_PRODUCT_ID = "season3unlock";
        public const string SEASONS_ALL_UNLOCK_PRODUCT_ID = "season4unlock";

        public const string ENERGY_1_PRODUCT_ID = "energy1bis";
        public const string ENERGY_3_PRODUCT_ID = "energy2";
        public const string ENERGY_10_PRODUCT_ID = "energy3";


        /** Lifetime Virtual Goods (aka - lasts forever **/

        // Create the 'TURN_GREEN' LifetimeVG In-App Purchase
        public static VirtualGood SEASON_2_UNLOCK = new LifetimeVG(
            "Season 2 unlock",														    		// Name of IAP
            "Unlock season 2 to play 8 new levels!",											// Description of IAP
            "season2unlock",													            	// Item ID (different from 'product id" used by itunes, this is used by soomla)

            // 1. assign the purchase type of the IAP (purchaseWithMarket == item cost real money),
            // 2. assign the IAP as a market item (using its ID)
            // 3. set the item to be a non-consumable purchase type

            //			1.					2.						3.
            new PurchaseWithMarket(SEASON_2_UNLOCK_PRODUCT_ID, 0.99)
        );
        public static VirtualGood SEASON_3_UNLOCK = new LifetimeVG(
            "Season 3 unlock",
            "Unlock season 3 to play 8 new levels!",
            "season3unlock",
            new PurchaseWithMarket(SEASON_3_UNLOCK_PRODUCT_ID, 0.99)
        );
        public static VirtualGood ALL_SEASONS_UNLOCK = new LifetimeVG(
           "All seasons unlock",
           "Unlock all seasons!",
           "season4unlock",
           new PurchaseWithMarket(SEASONS_ALL_UNLOCK_PRODUCT_ID, 24.99)
       );
        public static VirtualGood ENERGY_1 = new SingleUseVG(
            "Energy",
            "Buy 1 energy pack",
            "energy1bis",
            new PurchaseWithMarket(ENERGY_1_PRODUCT_ID, 0.99)
        );
        public static VirtualGood ENERGY_3 = new SingleUseVG(
            "Energy",
            "Buy 3 energy packs",
            "energy2",
            new PurchaseWithMarket(ENERGY_3_PRODUCT_ID, 0.99)
        );
        public static VirtualGood ENERGY_10 = new SingleUseVG(
            "Energy",
            "Buy 10 energy packs",
            "energy3",
            new PurchaseWithMarket(ENERGY_10_PRODUCT_ID, 0.99)
        );
    }
}
