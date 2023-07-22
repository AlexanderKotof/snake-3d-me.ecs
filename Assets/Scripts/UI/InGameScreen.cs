using Game;
using System;
using UnityEngine;

namespace UI.Screens
{
    public class InGameScreen : BaseScreen
    {
        private PlayerData _playerData;

        public TMPro.TMP_Text pointsCounter;

        public void SetModel(PlayerData playerData)
        {
            _playerData = playerData;
        }

        void Start()
        {

        }

        void Update()
        {
            pointsCounter.SetText(_playerData.PointsCount.ToString());
        }
    }
}