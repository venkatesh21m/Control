using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using Unity.Services.Economy;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;


namespace Rudrac.Control.Services
{
    public class DailyRewards : MonoBehaviour
    {
        public string cloudCodeCooldownScriptName = "DailyRewardCooldown";
        public string cloudCodeGrantScriptName = "DailyReward";

        public Button CollectButton;
        public TextMeshProUGUI TimeIndicator;

        int m_DefaultCooldownSeconds;
        int m_CooldownSeconds;

        void Awake()
        {
            Services.PlayerSignedIn += signedIn;
        }

        async void signedIn()
        {
            Services.PlayerSignedIn -= signedIn;
            if (this == null) return;
            await UpdateCooldownStatusFromCloudCode();

            ShowCooldownStatus();

            await WaitForCooldown();
        }

        async Task UpdateCooldownStatusFromCloudCode()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called because you're not logged in.");
                return;
            }

            try
            {
                var grantCooldownResult = await CloudCode.CallEndpointAsync<GrantCooldownResult>(
                    cloudCodeCooldownScriptName, new object());
                if (this == null) return;

                Debug.Log($"Retrieved cooldown flag:{grantCooldownResult.canGrantFlag} time:{grantCooldownResult.grantCooldown} default:{grantCooldownResult.defaultCooldown}");

                m_DefaultCooldownSeconds = grantCooldownResult.defaultCooldown;
                m_CooldownSeconds = grantCooldownResult.grantCooldown;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        void ShowCooldownStatus()
        {
            if (m_CooldownSeconds > 0)
            {
                CollectButton.interactable = false;
                TimeIndicator.text = m_CooldownSeconds > 1
                    ? "... ready in " + (m_CooldownSeconds > 60 ? "" + m_CooldownSeconds / 60 + " minutes " : m_CooldownSeconds + "seconds")
                    : "... ready in 1 second.";
            }
            else
            {
                CollectButton.interactable = true;
                TimeIndicator.text = "Claim Daily Reward";
            }
        }

        async Task WaitForCooldown()
        {
            while (m_CooldownSeconds > 0)
            {
                ShowCooldownStatus();

                await Task.Delay(1000);
                if (this == null) return;

                m_CooldownSeconds--;
            }

            ShowCooldownStatus();
        }




        public async void GrantTimedRandomReward()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called to grant the Daily Reward because you're not logged in.");
                return;
            }

            Debug.Log($"Calling Cloud Code {cloudCodeGrantScriptName} to grant the Daily Reward now.");

            CollectButton.interactable = false;
            TimeIndicator.text = "Claiming Daily Reward";

            try
            {
                var grantResult = await CloudCode.CallEndpointAsync<GrantResult>(
                    cloudCodeGrantScriptName, new object());
                if (this == null) return;

                Debug.Log("CloudCode script rewarded: " + GetGrantInfoString(grantResult));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            EconomyManager.UpdateEconomy?.Invoke();
            if (this == null) return;

            m_CooldownSeconds = m_DefaultCooldownSeconds;

            await WaitForCooldown();
        }

        string GetGrantInfoString(GrantResult grantResult)
        {
            string grantResultString = "";

            int currencyCount = grantResult.currencyId.Count;
            // int inventoryCount = grantResult.inventoryItemId.Count;
            for (int i = 0; i < currencyCount; i++)
            {
                if (i == 0)
                {
                    grantResultString += $"{grantResult.currencyQuantity[i]} {grantResult.currencyId[i]}(s)";
                }
                else
                {
                    grantResultString += $", {grantResult.currencyQuantity[i]} {grantResult.currencyId[i]}(s)";
                }
            }

            //for (int i = 0; i < inventoryCount; i++)
            //{
            //    if (i < inventoryCount - 1)
            //    {
            //        grantResultString += $", {grantResult.inventoryItemQuantity[i]} {grantResult.inventoryItemId[i]}(s)";
            //    }
            //    else
            //    {
            //        grantResultString += $" and {grantResult.inventoryItemQuantity[i]} {grantResult.inventoryItemId[i]}(s)";
            //    }
            //}

            return grantResultString;
        }



        public struct GrantCooldownResult
        {
            public bool canGrantFlag;
            public int grantCooldown;
            public int defaultCooldown;
        }

        // Struct used to receive the result of the Daily Reward from Cloud Code
        public struct GrantResult
        {
            public List<string> currencyId;
            public List<int> currencyQuantity;

            //public List<string> inventoryItemId;
            //public List<int> inventoryItemQuantity;
        }

    }
}