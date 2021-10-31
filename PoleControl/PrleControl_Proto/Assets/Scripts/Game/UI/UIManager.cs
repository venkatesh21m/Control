using UnityEngine;

namespace Rudrac.Control.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup mainMenu;
        [SerializeField] CanvasGroup GameMenu;
        [SerializeField] CanvasGroup GameOverMenu;

        private void Start()
        {
            Actions.StartGame += GameStarted;
            Actions.LevelCleared += LevelCleared;
            Actions.GameOver += LevelFailed;

            Actions.ChangeUIState += ChangeUIState;
        }


        private void OnDestroy()
        {
            Actions.StartGame -= GameStarted;
            Actions.LevelCleared -= LevelCleared;
            Actions.GameOver -= LevelFailed;

            Actions.ChangeUIState -= ChangeUIState;
        }

        private void LevelFailed()
        {
            GameOverMenu.Show(0.25f);
        }

        private void LevelCleared()
        {
            GameOverMenu.Show(0.25f);
        }
        private void ChangeUIState(UiState state)
        {
            switch (state)
            {
                case UiState.Home:
                    mainMenu.Show(0.25f);
                    GameOverMenu.Hide(0.25f);
                    break;
                case UiState.Game:
                    GameMenu.Show(0.25f);
                    mainMenu.Hide(0.25f);
                    GameOverMenu.Hide(0.25f);
                    break;
                case UiState.GameOver:
                    GameMenu.Hide(0.25f);
                    GameOverMenu.Show(0.25f);
                    break;
                default:
                    break;
            }
        }

        private void GameStarted(int obj)
        {
            mainMenu.Hide(0.75f);
            GameMenu.Show(0.25f);
        }
    }

    [SerializeField]
    public enum UiState
    {
        Home,
        Game,
        GameOver
    }

}
