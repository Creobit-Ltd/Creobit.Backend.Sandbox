using Creobit.Backend.Auth;
using Creobit.Backend.Link;
using Creobit.Backend.User;
using UnityEngine;

namespace Creobit.Backend.Sandbox
{
    [DisallowMultipleComponent]
    public sealed class SteamPlayFabLoginExample : MonoBehaviour
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB && CREOBIT_BACKEND_STEAM
        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var playFabLink = new PlayFabLink();
            var playFabUser = new PlayFabUser(playFabAuth);

            var steamAuth = new SteamAuth(_appId);
            var steamUser = new SteamUser();

            var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);
            var steamPlayFabLink = new SteamPlayFabLink(playFabLink, steamPlayFabAuth);
            var steamPlayFabUser = new SteamPlayFabUser(playFabUser, steamUser);

            _auth = steamPlayFabAuth;
            _link = steamPlayFabLink;
            _user = steamPlayFabUser;
        }
#endif

        private async void Start()
        {
            Debug.Log($"{nameof(IAuth.Login)}:");

            await _auth.LoginAsync();

            Debug.Log($"  {nameof(IUser.AvatarUrl)}: {_user.AvatarUrl}");
            Debug.Log($"  {nameof(IUser.Name)}: {_user.Name}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                RequestLinkKey();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Link();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Refresh();
            }

            async void RequestLinkKey()
            {
                Debug.Log($"{nameof(ILink.RequestLinkKey)}:");

                var (LinkKey, LinkKeyExpirationTime) = await _link.RequestLinkKeyAsync(_linkKeyLenght);

                Debug.Log($"  {nameof(LinkKey)}: {LinkKey}");
                Debug.Log($"  {nameof(LinkKeyExpirationTime)}: {LinkKeyExpirationTime}");

                _linkKey = LinkKey;
            }

            async void Link()
            {
                Debug.Log($"{nameof(ILink.Link)}:");

                await _link.LinkAsync(_linkKey);

                Debug.Log($"  {nameof(IUser.AvatarUrl)}: {_user.AvatarUrl}");
                Debug.Log($"  {nameof(IUser.Name)}: {_user.Name}");
            }

            async void Refresh()
            {
                Debug.Log($"{nameof(IUser.Refresh)}:");

                await _user.RefreshAsync();

                Debug.Log($"  {nameof(IUser.AvatarUrl)}: {_user.AvatarUrl}");
                Debug.Log($"  {nameof(IUser.Name)}: {_user.Name}");
            }
        }

        #endregion
        #region SteamPlayFabLoginExample

        private IAuth _auth;
        private ILink _link;
        private IUser _user;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [SerializeField]
        private string _linkKey;

        [SerializeField]
        private int _linkKeyLenght = 16;

        [Header("Steam")]

        [SerializeField]
        private uint _appId = 695720;

        #endregion
    }
}
