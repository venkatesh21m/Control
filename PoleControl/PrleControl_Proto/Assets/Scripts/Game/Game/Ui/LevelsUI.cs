using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using Rudrac.Control;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] LevelssaveData levelsData;
    [SerializeField] Button[] levelButtons;


    private void OnEnable()
    {
        LoadLevelsData();
    }

    private void OnDisable()
    {
        saveLevelsData();
    }

    public async void LoadLevelsData()
    {
        levelsData = new LevelssaveData()
        {
            levelsLockData = new bool[levelButtons.Length]
        };
        levelsData.levelsLockData[0] = true;
        Dictionary<string, string> savedData = await SaveData.LoadAsync(new HashSet<string> { SaveSystem.LevelsclearedDataKey });
        //retrevedData = savedData["key"];
        Debug.Log("Done: " + savedData[SaveSystem.LevelsclearedDataKey]);
        var data = savedData[SaveSystem.LevelsclearedDataKey];
        data = data.Replace(@"\", string.Empty);
        data = data.Remove(0,1);
        data = data.Remove(data.Length-1,1);
        Debug.Log("Done: " + data);
        levelsData = JsonUtility.FromJson<LevelssaveData>(data);
        SetLevelButtons();
    }

    internal void unlockeNextLevel()
    {
        levelsData.levelsLockData[LevelManager.currentLevel + 1] = true;
        saveLevelsData();
    }

    private void SetLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = levelsData.levelsLockData[i];
            levelButtons[i].GetComponentInChildren<TMPro.TMP_Text>().text = "Level " + i;
        }
    }

    async void saveLevelsData()
    {
        var leveldata = JsonUtility.ToJson(levelsData);
        Debug.Log("saving string ___ " + leveldata);
        var data = new Dictionary<string, object> { { SaveSystem.LevelsclearedDataKey, leveldata } };
        await SaveData.ForceSaveAsync(data);
    }
}



    [Serializable]
    public class LevelssaveData
    {
        public bool[] levelsLockData;
    }
