using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using UnityEngine;

public class EconomyService
{

    public static int Stars;


    static List<CurrencyDefinition> Currencydefinitions;
   
    public static async void GetCurrencies()
    {
        Currencydefinitions = await Economy.Configuration.GetCurrenciesAsync();
        LoadCurrencies();
    }

    private static void LoadCurrencies()
    {
        foreach (var item in Currencydefinitions)
        {
            switch (item.Id)
            {
                case "STARS":
                   
                    break;
                default:
                    break;
            }
        }
    }
}
