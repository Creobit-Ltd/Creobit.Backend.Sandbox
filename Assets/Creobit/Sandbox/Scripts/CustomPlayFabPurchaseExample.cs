using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creobit.Backend
{
    [DisallowMultipleComponent]
    public sealed class CustomPlayFabPurchaseExample : MonoBehaviour
    {
        #region MonoBehaviour

        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var customPlayFabAuth = new CustomPlayFabAuth(playFabAuth, _customId);

            _auth = customPlayFabAuth;

            var playFabStore = new PlayFabStore(_catalogVersion, _storeId)
            {
                CurrencyMap = new List<(string CurrencyId, string VirtualCurrency)>
                {
                    ( "Money", "RM" ),
                    ( "Coins", "CC" )
                },
                ProductMap = new List<(string ProductId, string ItemId)>
                {
                    ("_box", "Box"),
                    ("_key", "Key")
                }
            };

            _store = playFabStore;
        }

        private void Start()
        {
            _auth.Login(
                () =>
                {
                    Debug.Log("Auth.Login: Complete");

                    _store.LoadProducts(
                        () =>
                        {
                            Debug.Log("Store.LoadProducts: Complete");
                        },
                        () =>
                        {
                            Debug.LogError("Store.LoadProducts: Failure");
                        });
                },
                () =>
                {
                    Debug.LogError("Auth.Login: Failure");
                });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var product = _store.Products.Where(x => x.Id == "_box").FirstOrDefault();

                product.Purchase("Coins",
                    () =>
                    {
                        Debug.Log("Product.Purchase: Complete");
                    },
                    () =>
                    {
                        Debug.LogError("Product.Purchase: Failure");
                    });
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                var product = _store.Products.Where(x => x.Id == "_box").FirstOrDefault();

                product.Purchase("Money",
                    () =>
                    {
                        Debug.Log("Product.Purchase: Complete");
                    },
                    () =>
                    {
                        Debug.LogError("Product.Purchase: Failure");
                    });
            }
        }

        #endregion
        #region CustomPlayFabPurchaseExample

        private IAuth _auth;
        private IStore _store;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [SerializeField]
        private string _catalogVersion = "Default";

        [SerializeField]
        private string _storeId = "Default";

        [SerializeField]
        private string _customId;

        #endregion
    }
}