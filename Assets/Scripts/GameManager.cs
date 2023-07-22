using Game.Client;
using Game.Client.Messages;
using Game.Features.Collectables.Systems;
using Game.Features.Player.Systems;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Game
{
    public class PlayerData
    {
        public int GameId { get; private set; }
        public int PointsCount { get; private set; }
        public int SnakeLenght { get; private set; }

        public void AddPoints(int value)
        {
            PointsCount += value;
        }

        public void SetSnakeLength(int value)
        {
            SnakeLenght = value;
        }

        public void SetGameId(int value)
        {
            GameId = value;
        }
    }

    public class GameManager : MonoBehaviour
    {
        public IClient Client { get; private set; }
        public PlayerData PlayerData { get; private set; }

        public static class GameConfig
        {
            public const string uri = "wss://dev.match.qubixinfinity.io/snake";

            public const string gameCreatedMessageType = "game-created";
            public const string gameEndedMessageType = "game-ended";

            public const string menuSceneName = "Startup";
            public const string gameSceneName = "Game";

            public static bool IsMobilePlatform => true;// Application.isMobilePlatform;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);

            PlayerData = new PlayerData();
            Client = new WebSocketClient();

            Client.StartConnectionAsync(GameConfig.uri);

            Client.ConnectedToServer += OnConnected;
            Client.OnDisconected += OnDisconected;

            CollectSystem.AddPoints += AddPoints;
            GameOverSystem.GameOverAfter += OnGameOver;

            UIManager.Instance.SetModel(PlayerData);
            UIManager.Instance.ShowLoading();
        }
        private void OnDestroy()
        {
            GameOverSystem.GameOverAfter -= OnGameOver;
            CollectSystem.AddPoints -= AddPoints;

            Client.ConnectedToServer -= OnConnected;
            Client.OnDisconected -= OnDisconected;

            Client.CloseConnectionAsync();

            Client = null;
        }
        private void Update()
        {
            if (Client.HasMessageReceived(out var message))
            {
                OnMessageReceived(message);
            }
        }

        private void OnConnected()
        {
            UIManager.Instance.ShowMainMenu(() =>
            {
                Client.SendMessage(JSONSerializer.Serialize(
                    MessageFactory.StartGameMessage
                    ));
                UIManager.Instance.ShowLoading();
            });
        }
        private void OnDisconected()
        {
            Debug.Log("Disconected");

            UIManager.Instance.ShowLoading();

            Client.StartConnectionAsync(GameConfig.uri);
        }
        private void OnMessageReceived(string message)
        {
            var m = JSONSerializer.Deserialize<IncomingMessage>(message);

            switch (m.type)
            {
                case GameConfig.gameCreatedMessageType:

                    PlayerData.SetGameId(m.payload.id);
                    UIManager.Instance.ShowLoading();
                    SceneLoadUtils.LoadScene(GameConfig.gameSceneName, UIManager.Instance.ShowInGame);

                    Debug.Log($"Game Started message id {PlayerData.GameId}");
                    break;

                case GameConfig.gameEndedMessageType:

                    UIManager.Instance.ShowGameOver(() =>
                    {
                        Client.SendMessage(JSONSerializer.Serialize(
                        MessageFactory.StartGameMessage
                        ));
                        UIManager.Instance.ShowLoading();
                    });

                    Debug.Log("Game Ended message");
                    break;

                default:
                    Debug.Log($"Error message {message}");
                    break;
            }
        }


        private void SendUpdateGameMessage()
        {
            Client.SendMessage(JSONSerializer.Serialize(
               MessageFactory.CreateUpdateGameMessage(PlayerData.GameId, PlayerData.PointsCount, PlayerData.SnakeLenght)
               ));
        }
        private void SendEndGameMessage()
        {
            Client.SendMessage(JSONSerializer.Serialize(
                MessageFactory.CreateEndGameMessage(PlayerData.GameId)
                ));
        }

        private async void OnGameOver(float delay)
        {
            await Task.Delay((int)(delay * 1000));

            SendEndGameMessage();
        }
        private void AddPoints(int pointsCount, int snakeLenght)
        {
            PlayerData.AddPoints(pointsCount);
            PlayerData.SetSnakeLength(snakeLenght);

            SendUpdateGameMessage();
        }
    }
}
