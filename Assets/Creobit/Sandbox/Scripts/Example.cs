using Creobit.Backend.Auth;
using Creobit.Backend.Inventory;
using Creobit.Backend.Store;
using Creobit.Backend.User;
using Creobit.Backend.Wallet;
using System.Linq;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    [DisallowMultipleComponent]
    public abstract class Example : MonoBehaviour
    {
        #region MonoBehaviour

        protected virtual void Awake()
        {
        }

        private async void Start()
        {
            await Auth.LoginAsync();

            LogUser();

            Debug.Log($"=> {nameof(IWallet.Refresh)}Wallet");

            await Wallet.RefreshAsync();

            LogCurrencies();

            Debug.Log($"=> {nameof(IInventory.Refresh)}Inventory");

            await Inventory.RefreshAsync();

            LogItems();

            Debug.Log($"=> {nameof(IStore.Refresh)}Store");

            await Store.RefreshAsync();

            LogProducts();
            LogSubscriptions();

            Debug.Log($"{KeyCode.Alpha1} => {nameof(ICurrency.Grant)}Currency");
            Debug.Log($"{KeyCode.Alpha2} => {nameof(ICurrency.Consume)}Currency");
            Debug.Log($"{KeyCode.Alpha3} => {nameof(IItem.Grant)}Item");
            Debug.Log($"{KeyCode.Alpha4} => {nameof(IItem.Consume)}Item");
            Debug.Log($"{KeyCode.Alpha5} => {nameof(IProduct.Purchase)}Product");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GrantCurrency();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ConsumeCurrency();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GrantItem();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ConsumeItem();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PurchaseProduct();
            }

            async void ConsumeCurrency()
            {
                Debug.Log($"=> {nameof(ConsumeCurrency)}");

                var currency = Wallet.Currencies
                    .FirstOrDefault(x => x.Id == "coins" && x.Count > 0);

                if (currency != null && currency.Count >= 1)
                {
                    await currency.ConsumeAsync(1);
                }

                LogCurrencies();
            }

            async void GrantCurrency()
            {
                Debug.Log($"=> {nameof(GrantCurrency)}");

                var currency = Wallet.Currencies
                    .FirstOrDefault(x => x.Id == "coins");

                if (currency != null)
                {
                    await currency.GrantAsync(1);
                }

                LogCurrencies();
            }

            async void ConsumeItem()
            {
                Debug.Log($"=> {nameof(ConsumeItem)}");

                var item = Inventory.Items
                    .FirstOrDefault(x => x.Id == "potion" && x.Count > 0);

                if (item != null && item.Count >= 1)
                {
                    await item.ConsumeAsync(1);
                }

                LogItems();
            }

            async void GrantItem()
            {
                Debug.Log($"=> {nameof(GrantItem)}");

                var item = Inventory.Items
                    .FirstOrDefault(x => x.Id == "potion");

                if (item != null)
                {
                    await item.GrantAsync(1);
                }

                LogItems();
            }

            async void PurchaseProduct()
            {
                Debug.Log($"=> {nameof(PurchaseProduct)}");

                var product = Store.Products
                    .FirstOrDefault(x => x.Id == "potion" && x.Price.Id == "money");

                if (product == null)
                {
                    Debug.Log($"=> {nameof(PurchaseProduct)} Failure!");
                }
                else
                {
                    await product.PurchaseAsync();

                    Debug.Log($"=> {nameof(PurchaseProduct)} Complete!");
                }
            }
        }

        #endregion
        #region Example

        protected virtual IAuth Auth
        {
            get;
            set;
        }

        protected virtual IInventory Inventory
        {
            get;
            set;
        }

        protected virtual IStore Store
        {
            get;
            set;
        }

        protected virtual IUser User
        {
            get;
            set;
        }

        protected virtual IWallet Wallet
        {
            get;
            set;
        }

        private void LogCurrencies()
        {
            Debug.Log($"{nameof(IWallet.Currencies)}:");

            foreach (var currency in Wallet.Currencies)
            {
                Debug.Log(currency);
            }
        }

        private void LogItems()
        {
            Debug.Log($"{nameof(IPlayFabInventory.Items)}:");

            foreach (var item in Inventory.Items)
            {
                Debug.Log(item);
            }
        }

        private void LogProducts()
        {
            Debug.Log($"{nameof(IPlayFabStore.Products)}:");

            foreach (var product in Store.Products)
            {
                Debug.Log(product);
            }
        }

        private void LogSubscriptions()
        {
            Debug.Log($"{nameof(IPlayFabStore.Subscriptions)}:");

            foreach (var subscription in Store.Subscriptions)
            {
                Debug.Log(subscription);
            }
        }

        private void LogUser()
        {
            Debug.Log($"{nameof(IPlayFabUser.Name)}: {User.Name}");
        }

        #endregion
    }
}
