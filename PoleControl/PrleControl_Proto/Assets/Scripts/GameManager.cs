using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    private VisualElement RootVisualElement;
    private int currentLevel = 0;

    private Button StartButton,RestartButton,NextLevelButton;
    private VisualElement mainMenu,gameMenu,gameOver,LevelCleared;
    private Label levelNumber;

    public void Start()
    {
        RootVisualElement = UI.GetComponent<UIDocument>().rootVisualElement;

        mainMenu = RootVisualElement.Q<VisualElement>("MainMenu");
        gameMenu = RootVisualElement.Q<VisualElement>("GameMenu");
        gameOver = RootVisualElement.Q<VisualElement>("GameOver");
        LevelCleared = RootVisualElement.Q<VisualElement>("LevelCleared");

        levelNumber = RootVisualElement.Q<Label>("LevelNumber");

        StartButton = RootVisualElement.Q<Button>("StartButton");
        StartButton.RegisterCallback<ClickEvent>(ev => StartGame());

        RestartButton = RootVisualElement.Q<Button>("RestartButton");
        RestartButton.RegisterCallback<ClickEvent>(ev => Restart());
          
        NextLevelButton = RootVisualElement.Q<Button>("NextLevelButton");
        NextLevelButton.RegisterCallback<ClickEvent>(ev => NextLevel());

        Actions.GameOver += GameOvermethod;
        Actions.LevelCleared += LevelClearedmethod;
    }

    private void OnDestroy()
    {
        Actions.GameOver -= GameOvermethod;
        Actions.LevelCleared -= LevelClearedmethod;
    }

    void StartGame()
    {
        mainMenu.style.display = DisplayStyle.None;
        gameMenu.style.display = DisplayStyle.Flex;
        levelNumber.text = "Level: " + currentLevel.ToString();
        // MainMenu.SetActive(false);
        Actions.StartGame?.Invoke();
    }

    public void Restart()
    {
        Actions.NextLevel?.Invoke(currentLevel);
        //gameOverMenu.gameObject.SetActive(false);
        gameOver.style.display = DisplayStyle.None;
        gameMenu.style.display = DisplayStyle.Flex;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        currentLevel++;
        Actions.NextLevel?.Invoke(currentLevel);
        //levelCleared.gameObject.SetActive(false);
        levelNumber.text = "Level: " + currentLevel.ToString();
        LevelCleared.style.display = DisplayStyle.None;
        gameMenu.style.display = DisplayStyle.Flex;

    }

    void GameOvermethod()
    {
        //gameOverMenu.SetActive(true); 
        gameMenu.style.display = DisplayStyle.None;
        gameOver.style.display = DisplayStyle.Flex;
    }

    void LevelClearedmethod()
    {
        //levelCleared.SetActive(true);
        gameMenu.style.display = DisplayStyle.None;
        LevelCleared.style.display = DisplayStyle.Flex;
    }
}
