using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

namespace Rudrac.Control.UI
{
    public class LevelEndUI : MonoBehaviour
    {
        public TMP_Text GameTime;
        public TMP_Text gameoverText;
        public GameObject StarsHolder;
        public Transform[] Stars;

        public GameObject NextButton;

        public void SetUpStars()
        {
            int starcount = GameManager.CurrentLevelStarsCollected;
            for (int i = 0; i < Stars.Length; i++)
            {
                if(i<starcount)
                    Stars[i].gameObject.SetActive(true);
                else
                    Stars[i].gameObject.SetActive(false);
            }
            //Do stars Animations
        }

        public void ResetStars()
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(false);
            }
        }

        public void SetGameTime(string text) => GameTime.text = text + " s";

        internal void LevelCleared()
        {
            gameoverText.text = "Level Cleared";

            NextButton.SetActive(true);
            StarsHolder.SetActive(true);

            SetUpStars();
        }

        internal void LevelFailed()
        {
            gameoverText.text = "Level Failed";
            NextButton.SetActive(false);
            StarsHolder.SetActive(false);
        }
    }
}
