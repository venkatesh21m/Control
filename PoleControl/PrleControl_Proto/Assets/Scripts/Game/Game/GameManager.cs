using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rudrac.Control.UI;
using Rudrac.Control.Services;

namespace Rudrac.Control
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ui references")]
        [SerializeField] LevelsUI levelsUI;
        [SerializeField] LevelEndUI LevelEnd;
        [SerializeField] GameUI Gameui;

        public static int CurrentLevelStarsCollected = 0;
        private bool levelstarted;
        private float time;

        private void Start()
        {
            Actions.GameOver += GameOvermethod;
            Actions.LevelCleared += LevelClearedmethod;
            Actions.CollectableCollected += StarCollected;
        }

        private void OnDestroy()
        {
            Actions.GameOver -= GameOvermethod;
            Actions.LevelCleared -= LevelClearedmethod;
            Actions.CollectableCollected += StarCollected;
        }

        private void Update()
        {
            if (levelstarted)
            {
                time += Time.deltaTime;
                Gameui.SetTime(GetGameTime());
            }
        }

        #region Actions listeners   

        private void GameOvermethod()
        {
            levelstarted = false;
            Gameui.gameObject.SetActive(false);
            LevelEnd.LevelFailed();
            LevelEnd.SetGameTime(GetGameTime());
            LevelEnd.gameObject.SetActive(true);
        }


        private void LevelClearedmethod()
        {
            levelstarted = false;
            Gameui.gameObject.SetActive(false);
            levelsUI.unlockeNextLevel();
            EconomyManager.IncreaseStars?.Invoke(3);
            LevelEnd.LevelCleared();
            LevelEnd.SetGameTime(GetGameTime());
            LevelEnd.gameObject.SetActive(true);
        }

        private void StarCollected()
        {
            CurrentLevelStarsCollected++;
        }

        #endregion


        #region Button clickevent listeners


        public void LoadLevel(int level)
        {
            resetCurrentLevelStarsColectedCount();
            Actions.StartGame?.Invoke(level);

            Gameui.gameObject.SetActive(true);
            StartTime();
        }

        private void StartTime()
        {
            time = 0;
            levelstarted = true;
        }


        public void Restart()
        {
            resetCurrentLevelStarsColectedCount();
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
           
            Gameui.gameObject.SetActive(true);
            StartTime();

        }

        public void NextLevel()
        {
            resetCurrentLevelStarsColectedCount();
            LevelManager.currentLevel++;
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
           
            Gameui.gameObject.SetActive(true);
            StartTime();

        }

        #endregion


        private static void resetCurrentLevelStarsColectedCount() => CurrentLevelStarsCollected = 0;
        private string GetGameTime() => time.ToString("F1");
    }
}
