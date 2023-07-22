using Game;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameOverScreen : BaseScreen
    {
        public Button restartButton;

        public Action onRestartButtonPressed;
        private PlayerData _playerData;

        public void SetModel(PlayerData playerData)
        {
            _playerData = playerData;
        }

        void Start()
        {
            restartButton.onClick.AddListener(() => onRestartButtonPressed?.Invoke());
        }

        void Update()
        {

        }
    }
}