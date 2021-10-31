using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Rudrac.Control
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] LevelsUI levelsUI;

        [Space]
        [SerializeField] GameObject LevelFailed;
        [SerializeField] GameObject NextLevelButton;
        [SerializeField] TMP_Text gameoverText;
        private void Start()
        {
            Actions.GameOver += GameOvermethod;
            Actions.LevelCleared += LevelClearedmethod;
        }

        private void OnDestroy()
        {
            Actions.GameOver -= GameOvermethod;
            Actions.LevelCleared -= LevelClearedmethod;
        }

        #region Actions listeners   

        private void GameOvermethod()
        {
            NextLevelButton.SetActive(false);
            gameoverText.text = "Level Failed";
            LevelFailed.SetActive(true);
        }
        private void LevelClearedmethod()
        {
            NextLevelButton.SetActive(true);
            levelsUI.unlockeNextLevel();
            EconomyManager.IncreaseStars?.Invoke(3);
            gameoverText.text = "Level Cleared";
            LevelFailed.SetActive(true);
        }

        #endregion


        #region Button clickevent listeners


        public void LoadLevel(int level)
        {
            Actions.StartGame?.Invoke(level);
        }

        public void Restart()
        {
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        }

        public void NextLevel()
        {
            LevelManager.currentLevel++;
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        }

        #endregion
    }
}
