using UnityEngine;

namespace Creobit.Backend
{
    [DisallowMultipleComponent]
    public sealed class SteamPlayFabLoginExample : MonoBehaviour
    {
        #region MonoBehaviour

        private void Awake()
        {
            var playFabAuth = new PlayFabAuth(_titleId);
            var steamAuth = new SteamAuth(_appId);
            var steamPlayFabAuth = new SteamPlayFabAuth(playFabAuth, steamAuth);

            _auth = steamPlayFabAuth;
        }

        private void Start()
        {
            _auth.Login(
                () =>
                {
                    Debug.Log("Auth.Login: Complete");
                },
                () =>
                {
                    Debug.LogError("Auth.Login: Failure");
                });
        }

        #endregion
        #region SteamPlayFabLoginExample

        private IAuth _auth;

        [Header("PlayFab")]

        [SerializeField]
        private string _titleId = "12513";

        [Header("Steam")]

        [SerializeField]
        private uint _appId = 695720;

        #endregion
    }
}