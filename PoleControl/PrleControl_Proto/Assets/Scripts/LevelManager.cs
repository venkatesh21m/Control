using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] obstacle;
	[SerializeField]
	private GameObject goal,collectable;

	[SerializeField] private ObstacleType obstacleType;
	private List<GameObject> obstacles = new List<GameObject>();
	private List<GameObject> collectables = new List<GameObject>();
	private List<GameObject> ActiveObstacles = new List<GameObject>();
	
	[SerializeField] private TMP_Text levelText, scoreText;
	private int currentLevel = 1;
    public static int levelScore;

    private const string CurrentLevel = nameof(currentLevel);


    // Start is called before the first frame update
    void Start()
	{
		CreatePool();
		
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

	GameObject GetObstacle(ObstacleType type)
	{
		foreach (var ob in obstacles)
		{
			if (ob.activeSelf) continue;
			if (ob.GetComponent<Randomiser>().GetObstacleType() != type)
				continue;
			return ob;
		}
		return null;
	}

	#endregion

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

		levelText.text = "Level : " + currentLevel;

		var levelObstacles = 10+currentLevel;
		Vector3 pos;
		for (var i = 0; i < levelObstacles; i++)
		{
			GameObject obj;
			if (i % 2 == 0)
			{
				obj = GetObstacle(obstacleType);
				pos = obj.transform.position;
				pos.y = (i * 2 )+2;
				obj.transform.position = pos;
				obj.SetActive(true);
			}
			else
			{
				obj = GetObstacle(ObstacleType.Static);
				pos = obj.transform.position;
				pos.y = (i * 2) + 2;
				obj.transform.position = pos;
				obj.SetActive(true);
			}

			ActiveObstacles.Add(obj);
		}

		pos = goal.transform.position;
		pos.y = (levelObstacles * 2) +2;
		goal.transform.position = pos;
	}
}
