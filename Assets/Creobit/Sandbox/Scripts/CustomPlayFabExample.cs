using Creobit.Backend.Auth;
using Creobit.Backend.Inventory;
using Creobit.Backend.Store;
using Creobit.Backend.User;
using Creobit.Backend.Wallet;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    public sealed class CustomPlayFabExample : PlayFabExample
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB
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

            var customPlayFabAuth = new CustomPlayFabAuth(playFabAuth, _customId);

            Auth = customPlayFabAuth;
            Inventory = playFabInventory;
            Store = playFabStore;
            User = playFabUser;
            Wallet = playFabWallet;
        }
#endif

        #endregion
        #region CustomPlayFabExample

        [Header("Custom")]

        [SerializeField]
        private string _customId;

        #endregion
    }
}
