using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class Services : MonoBehaviour
{
    private async void Start()
    {
       // UnityServices.Initialize() will initialize all services that are subscribed to Core.
       await UnityServices.Initialize();
        Debug.Log(UnityServices.State);

        AuthenticationService.Instance.SignedIn += SignInComplete;
        AuthenticationService.Instance.SignInFailed += SignInFailed;

    }


    #region Static Methods

    public static void SignInAnonymousPressed()
    {
        AuthenticationService.Instance.SignInAnonymouslyAsync();
    } 
    
    #endregion


    #region callbacks

    private void SignInComplete()
    {

        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        // Shows how to get an access token
        Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        GameManager.SinInCompleteCallback();

        //Actions.SignInComplete?.Invoke();
    }

    private void SignInFailed(AuthenticationException exception)
    {
        Debug.LogError(exception);
    }

    #endregion
}
