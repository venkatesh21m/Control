using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using Unity.Services.Economy;
using System.Threading.Tasks;

public class SaveSystem
{
    public static string LevelsclearedDataKey = "LevelsKey";
    public static List<bool> levelClearedData = new List<bool>();
    public static string retrevedData;

    public static Dictionary<string, string> savedData = new Dictionary<string, string>();

    public static async void SavesomeData(string key, string value)
    {
        var data = new Dictionary<string, object> { { key, value } };
        await SaveData.ForceSaveAsync(data);

        Debug.Log("levels data saved");
    }

    public static async void LoadSomeData(string key)
    {
        Dictionary<string, string> savedData = await SaveData.LoadAsync(new HashSet<string> { key });
            Debug.Log("Done: " + savedData["key"]);
        retrevedData = savedData["key"];
    }

    public static async void RetrieveKeys()
    {
        List<string> keys = await SaveData.RetrieveAllKeysAsync();

        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i]);
        }
    }

}
