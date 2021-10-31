using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.Control.UI
{
    public class GameOverPanel : MonoBehaviour
    {

        public GameObject levelfailedPanel;
        public GameObject levelClearedPanel;

        [SerializeField] TMP_Text LevelName;
        [SerializeField] TMP_Text Time;
        [SerializeField] TMP_Text BestTime;

        [SerializeField] Image[] stars;

        // Start is called before the first frame update
        void Start()
        {
            Actions.LevelCleared += LevelCleared;
            Actions.GameOver += LevelFailed;
        }
        void OnDestroy()
        {
            Actions.LevelCleared += LevelCleared;
            Actions.GameOver += LevelFailed;
        }

        private void LevelFailed()
        {
            levelfailedPanel.SetActive(true);
            levelClearedPanel.SetActive(false);
            SetUpDetails();
        }

        private void LevelCleared()
        {
            levelfailedPanel.SetActive(false);
            levelClearedPanel.SetActive(true);
            SetUpDetails();
        }

        private void SetUpDetails()
        {
            LevelName.text = "level : 2";
            Time.text = "18.056s";
            BestTime.text = "10.454s";
        }



        public void OnHomeButtonPressed()
        {
            Actions.ChangeUIState?.Invoke(UiState.Home);
        }

        public void OnRetryButtonPressed()
        {
            Actions.ChangeUIState?.Invoke(UiState.Game);
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        }

        public void OnNextLevelPressed()
        {
            LevelManager.currentLevel++;
            Actions.ChangeUIState?.Invoke(UiState.Game);
            Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        }

    }
}
