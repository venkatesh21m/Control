using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Economy.Model;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Economy;
using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public string CurrencyID = "STARS";

    [SerializeField] CurrencyHudView[] currencyHudViews;
    public static event Action<string, long> CurrencyBalanceChanged;
    public static Action<int> IncreaseStars;
    public static Action UpdateEconomy;
    GetBalancesResult getBalancesResult;
    private void OnEnable()
    {
        foreach (var item in currencyHudViews)
        {
            CurrencyBalanceChanged += item.UpdateBalanceField;
        }
        IncreaseStars += IncreaseBalanceAsync;
        UpdateEconomy += InitialiseEconomyandCorrencies;
    }

    private void OnDisable()
    {
        foreach (var item in currencyHudViews)
        {
            CurrencyBalanceChanged -= item.UpdateBalanceField;
        }
        IncreaseStars -= IncreaseBalanceAsync;
        UpdateEconomy -= InitialiseEconomyandCorrencies;
    }



    // Start is called before the first frame update
    void Awake()
    {
        Services.PlayerSignedIn += InitialiseEconomyandCorrencies;
    }

    async void InitialiseEconomyandCorrencies()
    {
       await RefreshCurrencies();
    }


    async Task RefreshCurrencies()
    {
        try
        {
            var balancesOptions = new PlayerBalances.GetBalancesOptions { ItemsPerFetch = 100 };
            getBalancesResult = await Economy.PlayerBalances.GetBalancesAsync(balancesOptions);
            if (this == null) return;

            Debug.Log("refreshing currencies");
            UpdateCurrencies();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void UpdateCurrencies()
    {
        foreach (var balance in getBalancesResult.Balances)
        {
            Debug.Log("refreshing currencies ---" + balance.CurrencyId + " -----  " + balance.Balance);
            CurrencyBalanceChanged?.Invoke(balance.CurrencyId, balance.Balance);
        }
    }

    public async void IncreaseBalanceAsync(int newAmount)
    {
        PlayerBalance newBalance = await Economy.PlayerBalances.IncrementBalanceAsync(CurrencyID, newAmount);
        await RefreshCurrencies();
    }

}
