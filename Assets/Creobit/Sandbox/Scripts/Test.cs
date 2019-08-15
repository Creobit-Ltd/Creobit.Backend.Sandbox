using Creobit.Backend;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        var playFabAuth = new PlayFabAuth("12513");
        var steamAuth = new SteamAuth(695720);
        var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);
        var auth = (IAuth)steamPlayFabAuth;

        var playFabUser = new PlayFabUser(playFabAuth);
        var steamUser = new SteamUser();
        var steamPlayFabUser = new SteamPlayFabUser(playFabUser, steamUser);
        var user = (IUser)steamPlayFabUser;

        var playFabLink = new PlayFabLink();
        var steamPlayFabLink = new SteamPlayFabLink(playFabLink, steamPlayFabAuth);
        var link = (ILink)steamPlayFabLink;

        var playFabStore = new PlayFabStore("Default", null)
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
        var steamPlayFabStore = new SteamPlayFabStore(playFabStore);
        var store = (IStore)steamPlayFabStore;

        auth.Login(
            () =>
            {
                Debug.LogError("Auth.Login: Complete");

                //Debug.Log($"OLD UserName: {user.UserName}");
                //user.SetUserName("Bunny Robin",
                //    () =>
                //    {
                //        Debug.Log("User.SetUserName: Complete");
                //        Debug.Log($"NEW UserName: {user.UserName}");
                //    },
                //    () =>
                //    {
                //        Debug.Log("User.SetUserName: Failure");
                //    });

                //Debug.Log($"OLD UserName: {user.UserName}");
                //link.Link("qwerty",
                //    () =>
                //    {
                //        Debug.Log("Link.Link: Complete");
                //        Debug.Log($"NEW UserName: {user.UserName}");
                //    },
                //    () =>
                //    {
                //        Debug.Log("Link.Link: Failure");
                //        Debug.Log($"NEW UserName: {user.UserName}");
                //    });

                //link.RequestLinkKey(8,
                //    linkKey =>
                //    {
                //        Debug.Log("Link.RequestLinkKey: Complete");
                //        Debug.Log($"LinkKey: {linkKey}");
                //    },
                //    () =>
                //    {
                //        Debug.Log("Link.RequestLinkKey: Failure");
                //    });

                store.LoadProducts(
                    () =>
                    {
                        Debug.LogError("Store.LoadProducts: Complete");
                        foreach (var product in store.Products)
                        {
                            Debug.LogError($"Id: {product.Id} Coins: {product.GetPrice("Coins")} Money: {product.GetPrice("Money")}");
                        }

                        var xproduct = store.Products.FirstOrDefault(x => x.Id == "_key");

                        xproduct.Purchase("Money",
                            () =>
                            {
                                Debug.LogError("Store.Purchase: Complete");
                            },
                            () =>
                            {
                                Debug.LogError("Store.Purchase: Failure");
                            });
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
}
