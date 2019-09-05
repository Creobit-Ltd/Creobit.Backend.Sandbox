using Creobit.Backend.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    [DisallowMultipleComponent]
    public sealed class SteamPlayFabInventoryExample : MonoBehaviour
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB && CREOBIT_BACKEND_STEAM && UNITY_STANDALONE
        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var playFabInventory = new PlayFabInventory(_catalogVersion)
            {
                DefinitionMap = new List<(string DefinitionId, string PlayFabItemId)>
                {
                    // Items
                    ( "bow", "Bow" ),
                    ( "knife", "Knife" ),
                    ( "potion", "Potion" ),
                    ( "shield", "Shield" ),
                    ( "sword", "Sword" ),
                    // Bundles
                    ( "archer_pack", "ArcherPack" ),
                    ( "swordsman_pack", "SwordsmanPack" )
                }
            };

            var steamAuth = new SteamAuth(_appId);
            var steamInventory = new SteamInventory();

            var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);
            var steamPlayFabInventory = new SteamPlayFabInventory(playFabInventory, steamInventory);

            _auth = steamPlayFabAuth;
            _inventory = steamPlayFabInventory;
        }
#endif

        private void Start()
        {
            Login();

            void Login()
            {
                Debug.Log("Auth.Login: Invoke");

                _auth.Login(
                    () =>
                    {
                        Debug.Log("Auth.Login: Complete");

                        LoadDefinitions();
                    },
                    () =>
                    {
                        Debug.LogError("Auth.Login: Failure");
                    });
            }

            void LoadDefinitions()
            {
                Debug.Log("Inventory.LoadDefinitions: Invoke");

                _inventory.LoadDefinitions(
                    () =>
                    {
                        Debug.Log("Inventory.LoadDefinitions: Complete");

                        foreach (var definition in _inventory.Definitions)
                        {
                            Debug.Log($"  Definition->Id: {definition.Id}");
                        }

                        LoadItems();
                    },
                    () =>
                    {
                        Debug.LogError("Inventory.LoadDefinitions: Failure");
                    });
            }

            void LoadItems()
            {
                Debug.Log("Inventory.LoadItems: Invoke");

                _inventory.LoadItems(
                    () =>
                    {
                        Debug.Log("Inventory.LoadItems: Complete");

                        foreach (var item in _inventory.Items)
                        {
                            Debug.Log($"  Item->Definition->Id: {item.Definition.Id} Item->RemainingUses: {item.RemainingUses}");
                        }
                    },
                    () =>
                    {
                        Debug.LogError("Inventory.LoadItems: Failure");
                    });
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Consume();
            }

            void Consume()
            {
                Debug.Log("Item.Consume: Invoke");

                var item = _inventory.Items
                    .FirstOrDefault(x => x.Definition.Id == "potion");

                item?.Consume(1,
                    () =>
                    {
                        Debug.Log("Item.Consume: Complete");
                        Debug.Log($"  Item->RemainingUses: {item.RemainingUses}");
                    },
                    () =>
                    {
                        Debug.LogError("Item.Consume: Failure");
                        Debug.Log($"  Item->RemainingUses: {item.RemainingUses}");
                    });
            }
        }

        #endregion
        #region SteamPlayFabInventoryExample

        private IAuth _auth;
        private IInventory _inventory;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [SerializeField]
        private string _catalogVersion;

        [Header("Steam")]

        [SerializeField]
        private uint _appId = 695720;

        #endregion
    }
}
