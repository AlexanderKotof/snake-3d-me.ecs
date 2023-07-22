using Game.Client;
using Game.Client.Messages;
using Game.Features.Collectables.Systems;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static class GameConfig
    {
        public const string uri = "wss://dev.match.qubixinfinity.io/snake";

        public const string gameCreatedMessageType = "game-created";
        public const string gameEndedMessageType = "game-ended";
    }

    public class GameManager : MonoBehaviour
    {
        public IClient Client { get; private set; }
        public PlayerData PlayerData { get; private set; }


        private void Start()
        {
            Client = new WebSocketClient();

            Client.StartConnectionAsync(GameConfig.uri);

            Client.ConnectedToServer += OnConnected;
            Client.OnDisconected += OnDisconected;

            PlayerData = new PlayerData();

            CollectSystem.AddPoints += AddPoints;

            DontDestroyOnLoad(this);
        }

        private void OnConnected()
        {
            //TODO await button click and show loading screen
            Client.SendMessage(JSONSerializer.Serialize(
                MessageFactory.StartGameMessage
                ));
        }

        private void OnDisconected()
        {

        }

        private void OnMessageReceived(string message)
        {
            var m = JSONSerializer.Deserialize<IncomingMessage>(message);

            switch (m.type)
            {
                case GameConfig.gameCreatedMessageType:

                    PlayerData.SetGameId(m.payload.id);
                    LoadGameScene();

                    Debug.Log($"Game Started message id {PlayerData.GameId}");
                    break;

                case GameConfig.gameEndedMessageType:
                    Debug.Log("Game Ended message");
                    break;

                default:
                    Debug.Log($"Error message {message}");
                    break;
            }
        }

        private void Update()
        {
            if (Client.HasMessageReceived(out var message))
            {
                OnMessageReceived(message);
            }
        }

        private async void LoadGameScene()
        {
            var operation = SceneManager.LoadSceneAsync("Game");

            while (!operation.isDone)
                await Task.Yield();
        }

        [ContextMenu("Update game")]
        private void SendUpdateGameMessage()
        {
            Client.SendMessage(JSONSerializer.Serialize(
               MessageFactory.CreateUpdateGameMessage(PlayerData.GameId, PlayerData.PointsCount, PlayerData.SnakeLenght)
               ));
        }

        [ContextMenu("End game")]
        private void SendEndGameMessage()
        {
            Client.SendMessage(JSONSerializer.Serialize(
                MessageFactory.CreateEndGameMessage(PlayerData.GameId)
                ));
        }

        private void AddPoints(int pointsCount, int snakeLenght)
        {
            PlayerData.AddPoints(pointsCount);
            PlayerData.SetSnakeLength(snakeLenght);

            SendUpdateGameMessage();
        }

        private void OnDestroy()
        {
            CollectSystem.AddPoints -= AddPoints;

            Client.ConnectedToServer -= OnConnected;
            Client.OnDisconected -= OnDisconected;
            Client.CloseConnectionAsync();

            Client = null;
        }
    }
}
