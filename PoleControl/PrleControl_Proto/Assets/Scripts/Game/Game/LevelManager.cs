using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Linq;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private AssetReference[] _levels;
	//[SerializeField]
	//private GameObject[] obstacle;
	[SerializeField]
	private AssetReference[] _obstacle;

	[SerializeField] private ObstacleType obstacleType;
	
	//[SerializeField] private TMP_Text levelText, scoreText;
	public static int currentLevel = 1;
    public static int levelScore;

    private const string CurrentLevel = nameof(currentLevel);


    // Start is called before the first frame update
    void Start()
	{
		Actions.NextLevel += SetNextLevel;
		Actions.LevelCleared += disableAllObstacles;
		Actions.GameOver += disableAllObstacles;
		Actions.CollectableCollected += CollectableCollected;
        Actions.StartGame += SetNextLevel;

		if (PlayerPrefs.HasKey(CurrentLevel))
			currentLevel = PlayerPrefs.GetInt(CurrentLevel);

	}

	private void CollectableCollected()
	{
		levelScore++;
	}

	private void disableAllObstacles()
	{
		List<Randomiser> activeObstacles = FindObjectsOfType<Randomiser>().ToList();
		foreach (var activeObstacle in activeObstacles)
		{
			Addressables.ReleaseInstance(activeObstacle.gameObject);
		}
		activeObstacles.Clear();
	}


	private void SetNextLevel(int level)
	{
		Debug.LogError(currentLevel);
		currentLevel = level;
		PlayerPrefs.SetInt(CurrentLevel,currentLevel);

        StartCoroutine(SetUpLevel());
	}

    private IEnumerator SetUpLevel()
    {
		levelScore = 0;
		// scoreText.text = "Score : " + levelScore;
		//levelText.text = "Level : " + currentLevel;
		LevelData levelData = new LevelData();
		var level = Addressables.LoadAssetAsync<TextAsset>(_levels[currentLevel]);
		yield return level;
		string jsondata = level.Result.ToString();
		levelData.LoadFromJsom(jsondata);
		LoadLevel(levelData);
	}

	public void LoadLevel(LevelData levelData)
	{
		foreach (var item in levelData.objects)
		{
			Addressables.InstantiateAsync(_obstacle[((int)item.obstacleType)],item.pos,Quaternion.identity);
		}
	}

}
