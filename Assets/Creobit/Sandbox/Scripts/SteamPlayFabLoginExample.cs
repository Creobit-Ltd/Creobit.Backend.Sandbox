using UnityEngine;

namespace Creobit.Backend
{
    [DisallowMultipleComponent]
    public sealed class SteamPlayFabLoginExample : MonoBehaviour
    {
        #region MonoBehaviour

#if CREOBIT_BACKEND_PLAYFAB && CREOBIT_BACKEND_STEAM
        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var playFabUser = new PlayFabUser(playFabAuth);

            var steamAuth = new SteamAuth(_appId);
            var steamUser = new SteamUser();

            var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);
            var steamPlayFabUser = new SteamPlayFabUser(playFabUser, steamUser);

            _auth = steamPlayFabAuth;
            _user = steamPlayFabUser;
        }
#endif

        private void Start()
        {
            Debug.Log("Auth.Login: Start");

            _auth.Login(
                () =>
                {
                    Debug.Log("Auth.Login: Complete");
                    Debug.Log($"User.UserName: {_user.Name}");
                },
                () =>
                {
                    Debug.LogError("Auth.Login: Failure");
                });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("User.Refresh: Start");

                _user.Refresh(
                    () =>
                    {
                        Debug.Log("User.Refresh: Complete");
                        Debug.Log($"User.UserName: {_user.Name}");
                    },
                    () =>
                    {
                        Debug.LogError("User.Refresh: Failure");
                    });
            }
        }

        #endregion
        #region SteamPlayFabLoginExample

        private IAuth _auth;
        private IUser _user;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [Header("Steam")]

        [SerializeField]
        private uint _appId = 695720;

        #endregion
    }
}
