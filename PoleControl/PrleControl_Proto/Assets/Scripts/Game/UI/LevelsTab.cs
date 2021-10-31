using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rudrac.Control.UI
{
    public class LevelsTab : MonoBehaviour
    {
        [SerializeField] RectTransform LevelPanel,levelsHolder;
        [SerializeField] List<Image> Chapters;

        [Header("Display references")]
        [SerializeField] TMP_Text CurrentChapterHeader;
        [Space]
        [SerializeField] TMP_Text CurrentLevelText;
        [SerializeField] TMP_Text currentScoreText;

        [Header("Animation Variables")]
        [SerializeField] LevelButton levelButton;
        [SerializeField] List<LevelButton> levelButtons;

        [Header("Animation Variables")]
        [SerializeField] Color normalColor;
        [SerializeField] Color SelectedColor;

        private List<RectTransform> TabButtonRects = new List<RectTransform>();
        private RuntimeLibrary runtimeLibrary;
        private void Start()
        {
            SetUpPrvates();
            MainTabManager.OnLevelsTabClick += LevelTabOpened;
            OnChapterClicked(0);
        }

        private void OnDestroy()
        {
            MainTabManager.OnLevelsTabClick -= LevelTabOpened;
        }


        private void SetUpPrvates()
        {
            foreach (var item in Chapters)
            {
                TabButtonRects.Add(item.GetComponent<RectTransform>());
            }
        }


        private void LevelTabOpened()
        {

        }

        int currentChapter;
        bool active;
        public void OnChapterClicked(int index)
        {
            if (currentChapter == index && active)
            {
                Chapters[index].color = normalColor;
                TabButtonRects[index].DOSizeDelta(new Vector2(175, 200), 0.25f);
                LevelPanel.DOAnchorPos(new Vector2(0, -Screen.height), 0.2f);
                active = false;
                return;
            }
            currentChapter = index;
            for (int i = 0; i < Chapters.Count; i++)
            {
                if (i == index)
                {
                    Chapters[i].color = SelectedColor;
                    TabButtonRects[i].DOSizeDelta(new Vector2(175, 250), 0.25f);
                }
                else
                {
                    Chapters[i].color = normalColor;
                    TabButtonRects[i].DOSizeDelta(new Vector2(175, 200), 0.25f);
                }
            }

            StartCoroutine(SetUpLevelS());
            active = true;
        }

        private IEnumerator SetUpLevelS()
        {
            LevelPanel.DOAnchorPos(new Vector2(0, -Screen.height), 0.2f);
            //TODO: setup level buttons
            if (runtimeLibrary == null) runtimeLibrary = RuntimeLibrary.Instatce;

            CurrentChapterHeader.text = "---- \n ------- Chapter " + currentChapter + " ------- \n ---------------------------------";
            CurrentLevelText.text = "Level : 1";
            currentScoreText.text = "Best Time : 30.00s";

            int levelsCount = runtimeLibrary.LevelsLibrary.GetChapterLevelsCount(currentChapter);

            int difference = levelButtons.Count - levelsCount;
            if (difference < 0)
            {
                for (int i = difference; i < 0; i++)
                {
                    LevelButton _levelButton = Instantiate(levelButton, levelsHolder);
                    levelButtons.Add(_levelButton);
                }
            }

            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (i <= levelsCount)
                {
                    levelButtons[i].SetUpLevelButton(i);
                }
                else
                {
                    levelButtons[i].gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.2f);
            LevelPanel.DOAnchorPos(new Vector2(0, -50), 0.2f);

        }
    }
}
