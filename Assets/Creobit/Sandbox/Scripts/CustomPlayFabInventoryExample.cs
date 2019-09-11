using Creobit.Backend.Auth;
using Creobit.Backend.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    [DisallowMultipleComponent]
    public sealed class CustomPlayFabInventoryExample : MonoBehaviour
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB
        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var playFabInventory = new PlayFabInventory(_catalogVersion)
            {
                CurrencyDefinitionMap = new List<(string CurrencyDefinitionId, string PlayFabVirtualCurrencyId)>
                {
                    // Virtual Currencies
                    ("Coins", "CC"),
                    ("Gold", "GG"),
                    // Real Currencies
                    ("Money", "RM")
                },
                ItemDefinitionMap = new List<(string ItemDefinitionId, string PlayFabItemId)>
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
                }
            };

            var customPlayFabAuth = new CustomPlayFabAuth(playFabAuth, _customId);

            _auth = customPlayFabAuth;
            _inventory = playFabInventory;
        }
#endif

        private async void Start()
        {
            await _auth.LoginAsync();
            await _inventory.LoadCurrencyDefinitionsAsync();

            Debug.Log($"{nameof(IInventory.LoadItemDefinitions)}:");

            foreach (var currencyDefinition in _inventory.CurrencyDefinitions)
            {
                Debug.Log($"  {nameof(ICurrencyDefinition.Id)}: {currencyDefinition.Id}");
            }

            await _inventory.LoadCurrencyInstancesAsync();

            Debug.Log($"{nameof(IInventory.LoadCurrencyInstances)}:");

            foreach (var currencyInstance in _inventory.CurrencyInstances)
            {
                Debug.Log($"  {nameof(ICurrencyDefinition.Id)}: {currencyInstance.CurrencyDefinition.Id} {nameof(ICurrencyInstance.Count)}: {currencyInstance.Count}");
            }

            await _inventory.LoadItemDefinitionsAsync();

            Debug.Log($"{nameof(IInventory.LoadItemDefinitions)}:");

            foreach (var itemDefinition in _inventory.ItemDefinitions)
            {
                Debug.Log($"  {nameof(IItemDefinition.Id)}: {itemDefinition.Id}");
            }

            await _inventory.LoadItemInstancesAsync();

            Debug.Log($"{nameof(IInventory.LoadItemInstances)}:");

            foreach (var itemInstance in _inventory.ItemInstances)
            {
                Debug.Log($"  {nameof(IItemDefinition.Id)}: {itemInstance.ItemDefinition.Id} {nameof(IItemInstance.Count)}: {itemInstance.Count}");
            }
        }

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var itemDefinition = _inventory.ItemDefinitions
                    .FirstOrDefault(x => x.Id == "potion");
                var itemInstance = _inventory.ItemInstances
                    .FirstOrDefault(x => x.ItemDefinition.Id == "potion");

                if (itemInstance == null)
                {
                    Debug.Log($"{nameof(IItemInstance.Consume)}:");
                    Debug.Log($"  {nameof(IItemDefinition.Id)}: {itemDefinition.Id} {nameof(IItemInstance.Count)}: 0");
                }
                else
                {
                    await itemInstance.ConsumeAsync(1);

                    Debug.Log($"{nameof(IItemInstance.Consume)}:");
                    Debug.Log($"  {nameof(IItemDefinition.Id)}: {itemDefinition.Id} {nameof(IItemInstance.Count)}: {itemInstance.Count}");
                }
            }
        }

        #endregion
        #region CustomPlayFabInventoryExample

        private IAuth _auth;
        private IInventory<ICurrencyDefinition, ICurrencyInstance<ICurrencyDefinition>, IItemDefinition, IItemInstance<IItemDefinition>> _inventory;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [SerializeField]
        private string _catalogVersion;

        [SerializeField]
        private string _customId;

        #endregion
    }
}
