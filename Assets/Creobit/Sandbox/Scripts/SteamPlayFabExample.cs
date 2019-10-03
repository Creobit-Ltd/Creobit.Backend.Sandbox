using Creobit.Backend.Auth;
using Creobit.Backend.Inventory;
using Creobit.Backend.Store;
using Creobit.Backend.User;
using Creobit.Backend.Wallet;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    public sealed class SteamPlayFabExample : PlayFabExample
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB && CREOBIT_BACKEND_STEAM && UNITY_STANDALONE
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

            var steamAuth = new SteamAuth(_appId);
            var steamUser = new SteamUser();

            var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);
            var steamPlayFabUser = new SteamPlayFabUser(playFabUser, steamUser);
            var steamPlayFabStore = new SteamPlayFabStore(_catalogVersion, _storeId)
            {
                PriceMap = PlayFabStorePriceMap,
                ProductMap = PlayFabStoreProductMap
            };

            Auth = steamPlayFabAuth;
            Inventory = playFabInventory;
            Store = steamPlayFabStore;
            User = steamPlayFabUser;
            Wallet = playFabWallet;
        }
#endif

        #endregion
        #region SteamPlayFabExample

        [Header("Steam")]

        [SerializeField]
        private uint _appId = 695720;

        #endregion
    }
}
