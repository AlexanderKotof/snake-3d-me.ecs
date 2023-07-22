using Game;
using Game.Features;
using Game.Features.Input.Markers;
using Game.Features.Player.Components;
using ME.ECS;
using UnityEngine;
using UnityEngine.UI;
using static Game.GameManager;

namespace UI.Screens
{
    public class InGameScreen : BaseScreen
    {
        public TMPro.TMP_Text pointsCounter;

        public GameObject mobileControlsContainer;

        public Button rightButton;
        public Button leftButton;
        public Button upButton;
        public Button downButton;

        private PlayerData _playerData;

        public void SetModel(PlayerData playerData)
        {
            _playerData = playerData;
        }

        void Start()
        {
            mobileControlsContainer.SetActive(GameConfig.IsMobilePlatform);

            if (GameConfig.IsMobilePlatform)
            {
                rightButton.onClick.AddListener(() => ControllButtonPressed(Vector3.right));
                leftButton.onClick.AddListener(() => ControllButtonPressed(Vector3.left));
                upButton.onClick.AddListener(() => ControllButtonPressed(Vector3.forward));
                downButton.onClick.AddListener(() => ControllButtonPressed(Vector3.back));
            }
        }

        private void OnDestroy()
        {
            if (GameConfig.IsMobilePlatform)
            {
                rightButton.onClick.RemoveAllListeners();
                leftButton.onClick.RemoveAllListeners();
                upButton.onClick.RemoveAllListeners();
                downButton.onClick.RemoveAllListeners();
            }
        }

        private void ControllButtonPressed(Vector3 input)
        {
            Worlds.currentWorld.AddMarker(new InputMarker { inputDirection = input });
        }

        void Update()
        {
            pointsCounter.SetText(_playerData.PointsCount.ToString());
        }
    }
}