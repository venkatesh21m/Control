using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    private VisualElement RootVisualElement;
    // private int currentLevel = 0;

    [SerializeField] public int TotalLevles;
    private static Button StartButton,RestartButton,NextLevelButton,levelsButton_GO, levelsButton_LC;
    private VisualElement mainMenu,levelsMenu,gameMenu,gameOver,LevelCleared;
    
    private static GroupBox SignInButtons;
    private Button GuestSignInButton, GoogleSignInButton;
    private Label levelNumber;

    private List<Button> levelButtons = new List<Button>();

    public void Start()
    {
        RootVisualElement = UI.GetComponent<UIDocument>().rootVisualElement;

        mainMenu = RootVisualElement.Q<VisualElement>("MainMenu");
        levelsMenu = RootVisualElement.Q<VisualElement>("Levels");
        gameMenu = RootVisualElement.Q<VisualElement>("GameMenu");
        gameOver = RootVisualElement.Q<VisualElement>("GameOver");
        LevelCleared = RootVisualElement.Q<VisualElement>("LevelCleared");

        SignInButtons = mainMenu.Q<GroupBox>("SIgnInButtons");
        GuestSignInButton = mainMenu.Q<Button>("AnonymousButton");
        GoogleSignInButton = mainMenu.Q<Button>("GoogleButton");

        GuestSignInButton.RegisterCallback<ClickEvent>(ev => Services.SignInAnonymousPressed());

        ScrollView scrollView = levelsMenu.Q<ScrollView>("LevelsHolder");
        for (int i = 0; i < TotalLevles; i++)
        {
            Button b = scrollView.Q<Button>("Level" + (i + 1));
            levelButtons.Add(b);
        }
        levelButtons[0].RegisterCallback<ClickEvent>(ev => LoadLevel(0));
        levelButtons[1].RegisterCallback<ClickEvent>(ev => LoadLevel(1));
        levelButtons[2].RegisterCallback<ClickEvent>(ev => LoadLevel(2));
        levelButtons[3].RegisterCallback<ClickEvent>(ev => LoadLevel(3));
        levelButtons[4].RegisterCallback<ClickEvent>(ev => LoadLevel(4));
        levelButtons[5].RegisterCallback<ClickEvent>(ev => LoadLevel(5));
        levelButtons[6].RegisterCallback<ClickEvent>(ev => LoadLevel(6));
        levelButtons[7].RegisterCallback<ClickEvent>(ev => LoadLevel(7));

        levelNumber = RootVisualElement.Q<Label>("LevelNumber");

        StartButton = RootVisualElement.Q<Button>("StartButton");
        StartButton.RegisterCallback<ClickEvent>(ev => StartGame());

        RestartButton = RootVisualElement.Q<Button>("RestartButton");
        RestartButton.RegisterCallback<ClickEvent>(ev => Restart());
          
        NextLevelButton = RootVisualElement.Q<Button>("NextLevelButton");
        NextLevelButton.RegisterCallback<ClickEvent>(ev => NextLevel());

        levelsButton_GO = RootVisualElement.Q<Button>("LevelsButton_GO");
        levelsButton_GO.RegisterCallback<ClickEvent>(ev => showLevels());
       
        levelsButton_LC = RootVisualElement.Q<Button>("LevelsButton_LC");
        levelsButton_LC.RegisterCallback<ClickEvent>(ev => showLevels());

        Actions.GameOver += GameOvermethod;
        Actions.LevelCleared += LevelClearedmethod;
        Actions.SignInComplete += SinInCompleteCallback;
       // currentLevel = LevelManager.currentLevel;
    }

    public static void SinInCompleteCallback()
    {
        StartButton.style.display = DisplayStyle.Flex;
        SignInButtons.style.display = DisplayStyle.None;
    }


    private void OnDestroy()
    {
        Actions.GameOver -= GameOvermethod;
        Actions.LevelCleared -= LevelClearedmethod;
    }

    void StartGame()
    {
        mainMenu.style.display = DisplayStyle.None;
        levelsMenu.style.display = DisplayStyle.Flex;
        levelNumber.text = "Level: " + LevelManager.currentLevel.ToString();
        // MainMenu.SetActive(false);
    }

    void LoadLevel(int level)
    {
        Actions.StartGame?.Invoke(level);
        levelsMenu.style.display = DisplayStyle.None;
        gameMenu.style.display = DisplayStyle.Flex;
    }

    void showLevels()
    {
        levelsMenu.style.display = DisplayStyle.Flex;
        gameOver.style.display = DisplayStyle.None;
        LevelCleared.style.display = DisplayStyle.None;   

    }

    public void Restart()
    {
        Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        //gameOverMenu.gameObject.SetActive(false);
        gameOver.style.display = DisplayStyle.None;
        gameMenu.style.display = DisplayStyle.Flex;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

    public void NextLevel()
    {
        LevelManager.currentLevel++;
        Actions.NextLevel?.Invoke(LevelManager.currentLevel);
        //levelCleared.gameObject.SetActive(false);
        levelNumber.text = "Level: " + LevelManager.currentLevel.ToString();
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
