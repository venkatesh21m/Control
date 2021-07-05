using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu, gameOverMenu,levelCleared;
    
    private int currentLevel = 0;
    public void Start()
    {
       Actions.GameOver += GameOvermethod;
       Actions.LevelCleared += LevelClearedmethod;
    }

    private void OnDestroy()
    {
        Actions.GameOver -= GameOvermethod;
        Actions.LevelCleared -= LevelClearedmethod;
    }

    public void Restart()
    {
        Actions.NextLevel?.Invoke(currentLevel);
        gameOverMenu.gameObject.SetActive(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        currentLevel++;
        Actions.NextLevel?.Invoke(currentLevel);
        levelCleared.gameObject.SetActive(false);
    }
    
    void GameOvermethod()
    {
        gameOverMenu.SetActive(true); 
    }  
    
    void LevelClearedmethod()
    {
        levelCleared.SetActive(true);
    }
}
