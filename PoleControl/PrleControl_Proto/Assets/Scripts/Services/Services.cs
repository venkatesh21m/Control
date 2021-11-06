using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using Rudrac.Control.UI;

namespace Rudrac.Control.Services
{
    public class Services : MonoBehaviour
    {
        public LevelsUI levelui;
        public static event Action PlayerSignedIn;
        // Start is called before the first frame update
        async void Start()
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
            await SignInAnonymouslyAsync();

        }

        async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException exception)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(exception);
            }
        }


        // Setup authentication event handlers if desired
        void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

                PlayerSignedIn?.Invoke();
                levelui.LoadLevelsData();
            };

            AuthenticationService.Instance.SignInFailed += (err) =>
            {
                Debug.LogError(err);
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("Player signed out.");
            };
        }

    }
}
