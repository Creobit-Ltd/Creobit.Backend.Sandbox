#if CREOBIT_BACKEND_GOOGLEPLAY && CREOBIT_BACKEND_PLAYFAB && CREOBIT_BACKEND_UNITY && UNITY_ANDROID
#define IS_GOOGLE_ENABLED
#endif
#if IS_GOOGLE_ENABLED
using Creobit.Backend.Auth;
using Creobit.Backend.Inventory;
using Creobit.Backend.Store;
using Creobit.Backend.User;
using Creobit.Backend.Wallet;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    public sealed class GooglePlayPlayFabExample : PlayFabExample
    {
        #region MonoBehaviour

#if IS_GOOGLE_ENABLED
        protected override void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var playFabUser = new PlayFabUser(playFabAuth);
            var playFabWallet = new PlayFabWallet()
            {
                CurrencyMap = PlayFabWalletCurrencyMap
            };
            var playFabInventory = new PlayFabInventory(_catalogVersion, playFabUser, playFabWallet)
            {
                ItemMap = PlayFabInventoryItemMap
            };
            var playFabStore = new PlayFabStore(_catalogVersion, _storeId)
            {
                PriceMap = PlayFabStorePriceMap,
                ProductMap = PlayFabStoreProductMap
            };

            var googlePlayAuth = new GooglePlayAuth();
            var googlePlayStore = new GooglePlayStore(_publicKey)
            {
                ProductMap = UnityStoreProductMap,
                SubscriptionMap = UnityStoreSubscriptionMap
            };

            var googlePlayPlayFabAuth = new GooglePlayPlayFabAuth(playFabAuth, googlePlayAuth);
            var googlePlayPlayFabStore = new GooglePlayPlayFabStore(playFabStore, googlePlayStore);

            Auth = googlePlayPlayFabAuth;
            Inventory = playFabInventory;
            Store = googlePlayPlayFabStore;
            User = playFabUser;
            Wallet = playFabWallet;
        }
#endif

        #endregion
        #region GooglePlayPlayFabExample

        private readonly IEnumerable<(string ProductId, (string Id, bool Consumable) NativeProduct)> UnityStoreProductMap = new List<(string ProductId, (string Id, bool Consumable) NativeProduct)>()
        {
        };

        private readonly IEnumerable<(string SubscriptionId, string NativeProductId)> UnityStoreSubscriptionMap = new List<(string SubscriptionId, string NativeProductId)>()
        {
        };

        [Header("GooglePlay")]

        [SerializeField]
        private string _publicKey;

        #endregion
    }
}
