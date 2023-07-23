using Game;
using Game.Client.Messages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI.UIManager;

namespace UI.Screens
{
    public class GameOverScreen : BaseScreen
    {
        public Button restartButton;
        public Action onRestartButtonPressed;

        public TMP_Text[] infoTexts;

        public void SetModel(GameInfo info)
        {
            infoTexts[0].SetText($"Game ID - {info.gameId}");
            infoTexts[1].SetText($"Apples Collected - {info.applesCount}");
            infoTexts[2].SetText($"Snake length - {info.snakeLength}");
            infoTexts[3].SetText($"Game time - {info.gameTime}");
        }

        private void Start()
        {
            restartButton.onClick.AddListener(() => onRestartButtonPressed?.Invoke());
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveAllListeners();
        }
    }
}