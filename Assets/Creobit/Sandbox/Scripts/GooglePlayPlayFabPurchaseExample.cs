using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Creobit.Backend
{
    [DisallowMultipleComponent]
    public sealed class GooglePlayPlayFabPurchaseExample : MonoBehaviour
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_GOOGLEPLAYPLAYFAB && CREOBIT_BACKEND_PLAYFAB
        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var googlePlayAuth = new GooglePlayAuth();
            var googlePlayPlayFabAuth = new GooglePlayPlayFabAuth(playFabAuth, googlePlayAuth);

            _auth = googlePlayPlayFabAuth;

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
            var googlePlayPlayFabStore = new GooglePlayPlayFabStore(playFabStore)
            {
                ProductMap = new List<(string ProductId, ProductType ProductType)>
                {
                    ("Box", ProductType.Consumable),
                    ("Key", ProductType.Consumable)
                },
                PublicKey = _publicKey
            };

            _store = googlePlayPlayFabStore;
        }
#endif

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
        #region GooglePlayPlayFabPurchaseExample

        private IAuth _auth;
        private IStore _store;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [SerializeField]
        private string _catalogVersion;

        [SerializeField]
        private string _storeId;

        [Header("GooglePlay")]

        [SerializeField]
        private string _publicKey;

        #endregion
    }
}
