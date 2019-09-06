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

            var customPlayFabAuth = new CustomPlayFabAuth(playFabAuth, _customId);

            _auth = customPlayFabAuth;
            _inventory = playFabInventory;
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
        #region CustomPlayFabInventoryExample

        private IAuth _auth;
        private IInventory _inventory;

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
