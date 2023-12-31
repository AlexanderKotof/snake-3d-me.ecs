using Game;
using System;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public class GameInfo
        {
            public int applesCount;
            public int snakeLength;
            public int gameId;
            public TimeSpan gameTime;
        }

        public static UIManager Instance { get; private set; }

        public MainMenuScreen mainMenuScreen;
        public InGameScreen inGameScreen;
        public GameOverScreen gameOverScreen;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this);

            DontDestroyOnLoad(this);
        }

        public void SetModel(PlayerData data)
        {
            inGameScreen.SetModel(data);
        }

        public void ShowMainMenu(Action enterButtonCallback)
        {
            mainMenuScreen.Show();
            inGameScreen.Hide();
            gameOverScreen.Hide();

            mainMenuScreen.ShowEnterButton();
            mainMenuScreen.onEnterButtonPressed = enterButtonCallback;
        }

        public void ShowLoading()
        {
            mainMenuScreen.Show();
            inGameScreen.Hide();
            gameOverScreen.Hide();

            mainMenuScreen.ShowLoading();
        }

        public void ShowInGame()
        {
            mainMenuScreen.Hide();
            inGameScreen.Show();
            gameOverScreen.Hide();
        }


        public void ShowGameOver(Action restartButtonCallback, GameInfo info)
        {
            mainMenuScreen.Hide();
            inGameScreen.Hide();
            gameOverScreen.Show();

            gameOverScreen.SetModel(info);
            gameOverScreen.onRestartButtonPressed = restartButtonCallback;
        }
    }
}