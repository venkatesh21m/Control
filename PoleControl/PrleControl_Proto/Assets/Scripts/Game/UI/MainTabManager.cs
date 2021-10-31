using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.Control.UI
{
    public class MainTabManager : MonoBehaviour
    {

        #region Variables
        public static Action OnShopTabClick;
        public static Action OnLevelsTabClick;
        public static Action OnDailyTabClick;
        public static Action OnThemesTabClick;

        [SerializeField] Image[] TabImages;
        [SerializeField] RectTransform[] TabPanels;

        [Header("Animation Variables")]
        [SerializeField] Color normalColor;
        [SerializeField] Color SelectedColor;

        public List<RectTransform> TabButtonRects = new List<RectTransform>();
        #endregion

        private void Awake()
        {
            //SetPanelsSize();
            SetUpPrvates();
            OnTabButtonClicked(1);
        }

        private void SetUpPrvates()
        {
            foreach (var item in TabImages)
            {
                TabButtonRects.Add(item.GetComponent<RectTransform>());
            }
        }

        #region Tab Button Press Listener
        int currentPanel;
        public void OnTabButtonClicked(int index)
        {
            currentPanel = index;

            for (int i = 0; i < TabImages.Length; i++)
            {
                if (i == index)
                {
                    TabPanels[i].gameObject.SetActive(true);
                    TabImages[i].color = SelectedColor;
                    TabButtonRects[i].DOSizeDelta(new Vector2(225, 250), 0.25f);
                    TabPanels[i].DOAnchorPos(new Vector2(0, 0), 0.25f);
                }
                else
                {
                    TabImages[i].color = normalColor;
                    TabButtonRects[i].DOSizeDelta(new Vector2(225, 200), 0.25f);
                    if (i > index)
                        TabPanels[i].DOAnchorPos(new Vector2(Screen.width, 0), 0.25f);
                    else
                        TabPanels[i].DOAnchorPos(new Vector2(-Screen.width, 0), 0.25f);
                }
            }

            Invoke(nameof(DisableOffScreenPanel), .25f);
        }

        private void DisableOffScreenPanel()
        {
            for (int i = 0; i < TabImages.Length; i++)
            {
                if (i != currentPanel)
                {
                    TabPanels[i].gameObject.SetActive(false);
                }
            }
        }

        #endregion

    }
}
