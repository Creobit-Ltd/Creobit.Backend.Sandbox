using System.Collections.Generic;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    public abstract class PlayFabExample : Example
    {
        #region PlayFabExample

        protected readonly IEnumerable<(string CurrencyId, string PlayFabVirtualCurrencyId)> PlayFabWalletCurrencyMap = new (string CurrencyId, string PlayFabVirtualCurrencyId)[]
        {
            ("coins", "CC"),
            ("gold", "GG")
        };

        protected readonly IEnumerable<(string ItemId, string PlayFabItemId)> PlayFabInventoryItemMap = new (string ItemId, string PlayFabItemId)[]
        {
            // Items
            ("bow", "Bow"),
            ("knife", "Knife"),
            ("potion", "Potion"),
            ("shield", "Shield"),
            ("sword", "Sword"),
            // Bundles
            ("archer_pack", "ArcherPack"),
            ("swordsman_pack", "SwordsmanPack")
        };

        protected readonly IEnumerable<(string PriceId, string PlayFabVirtualCurrencyId)> PlayFabStorePriceMap = new (string PriceId, string PlayFabVirtualCurrencyId)[]
        {
            // Virtual Currencies
            ("coins", "CC"),
            ("gold", "GG"),
            // Real Currencids
            ("money", "RM")
        };

        protected readonly IEnumerable<(string ProductId, string PlayFabItemId)> PlayFabStoreProductMap = new (string ProductId, string PlayFabItemId)[]
        {
            // Items
            ("potion", "Potion"),
            // Bundles
            ("archer_pack", "ArcherPack"),
            ("swordsman_pack", "SwordsmanPack")
        };

        [Header("PlayFab")]

        [SerializeField]
        protected string _titleId = "12513";

        [SerializeField]
        protected string _catalogVersion;

        [SerializeField]
        protected string _storeId;

        #endregion
    }
}
