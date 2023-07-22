using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        public Button enterGameButton;
        public GameObject loadingLabel;

        public Action onEnterButtonPressed;

        private void Start()
        {
            enterGameButton.onClick.AddListener(() => onEnterButtonPressed?.Invoke());
        }

        public void ShowEnterButton()
        {
            enterGameButton.gameObject.SetActive(true);
            loadingLabel.SetActive(false);
        }

        public void ShowLoading()
        {
            enterGameButton.gameObject.SetActive(false);
            loadingLabel.SetActive(true);
        }
    }
}