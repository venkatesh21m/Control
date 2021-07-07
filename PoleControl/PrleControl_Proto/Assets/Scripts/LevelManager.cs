using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
	[SerializeField] List<TextAsset> levels;

	[SerializeField]
	private GameObject[] obstacle;
	[SerializeField]
	private GameObject goal,collectable;

	[SerializeField] private ObstacleType obstacleType;
	private List<GameObject> obstacles = new List<GameObject>();
	private List<GameObject> collectables = new List<GameObject>();
	private List<GameObject> ActiveObstacles = new List<GameObject>();
	
	//[SerializeField] private TMP_Text levelText, scoreText;
	private int currentLevel = 1;
    public static int levelScore;

    private const string CurrentLevel = nameof(currentLevel);


    // Start is called before the first frame update
    void Start()
	{
		//CreatePool();
		
		Actions.NextLevel += SetNextLevel;
		Actions.LevelCleared += disableAllObstacles;
		Actions.GameOver += disableAllObstacles;
		Actions.CollectableCollected += CollectableCollected;
		
		if(PlayerPrefs.HasKey(CurrentLevel))
			currentLevel = PlayerPrefs.GetInt(CurrentLevel);

		SetUpLevel();
	}

	private void CollectableCollected()
	{
		levelScore++;
		// scoreText.text = "Score : " + levelScore;
	}

	private void disableAllObstacles()
	{
		foreach (var activeObstacle in ActiveObstacles)
		{
			activeObstacle.SetActive(false);
		}
		ActiveObstacles.Clear();
	}

	#region  pool
	private void CreatePool()
	{
		goal = Instantiate(goal);
		
		foreach (var ob in obstacle)
		{
			for (int i = 0; i < 30; i++)
			{
				GameObject obj = Instantiate(ob);
				obstacles.Add(obj);
				obj.SetActive(false);
			} 
		}

		for (int i = 0; i < 15; i++)
		{
			GameObject obj = Instantiate(collectable);
			collectables.Add(obj);
			obj.SetActive(false);
		}
	}

	#endregion
	GameObject GetObstacle(ObstacleType type)
	{
        return obstacle[((int)type)];
	}


	private void SetNextLevel(int level)
	{
		currentLevel = level;
		PlayerPrefs.SetInt(CurrentLevel,currentLevel);

        SetUpLevel();
	}

    private void SetUpLevel()
    {
		levelScore = 0;
		// scoreText.text = "Score : " + levelScore;
		//levelText.text = "Level : " + currentLevel;
		LevelData levelData = new LevelData();
		string jsondata = levels[currentLevel].ToString();
		levelData.LoadFromJsom(jsondata);
		LoadLevel(levelData);
	}

	public void LoadLevel(LevelData levelData)
	{
		disableAllObstacles();
		foreach (var item in levelData.objects)
		{
			GameObject obj = GetObstacle(item.obstacleType);
			obj = Instantiate(obj);
			obj.transform.position = item.pos;

			ActiveObstacles.Add(obj);
		}
	}
}
