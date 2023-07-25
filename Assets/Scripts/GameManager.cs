using Game.Client;
using Game.Client.Messages;
using Game.Events;
using Game.SceneUtils;
using Game.Serializer;
using ME.ECS;
using System;
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

    public partial class GameManager : MonoBehaviour
    {
        public IClient Client { get; private set; }
        public PlayerData PlayerData { get; private set; }

        public AddPointsGlobalEvent addPointsEvent;
        public GameOverGlobalEvent gameOverEvent;

        private void Start()
        {
            DontDestroyOnLoad(this);

            PlayerData = new PlayerData();
            Client = new WebSocketClient();

            Client.ConnectedToServer += OnConnected;
            Client.OnDisconected += OnDisconected;

            Client.StartConnection(GameConfig.uri);

            addPointsEvent.Subscribe(OnAddPoints);
            gameOverEvent.Subscribe(OnGameOver);

            UIManager.Instance.SetModel(PlayerData);
            UIManager.Instance.ShowLoading();
        }

        private void OnDestroy()
        {
            addPointsEvent.Unsubscribe(OnAddPoints);
            gameOverEvent.Unsubscribe(OnGameOver);

            Client.ConnectedToServer -= OnConnected;
            Client.OnDisconected -= OnDisconected;

            Client.CloseConnection();

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

            Client.StartConnection(GameConfig.uri);
        }
        private void OnMessageReceived(string message)
        {
            Debug.Log($"Message received {message}");
            var m = JSONSerializer.Deserialize<IncomingMessage>(message);

            switch (m.type)
            {
                case GameConfig.gameCreatedMessageType:

                    PlayerData.SetGameId(m.payload.id);
                    UIManager.Instance.ShowLoading();
                    SceneLoadUtils.LoadScene(GameConfig.gameSceneName, UIManager.Instance.ShowInGame);

                    Debug.Log($"Game Started, id {PlayerData.GameId}");
                    break;

                case GameConfig.gameEndedMessageType:
                    Debug.Log("Game Ended");
                    var gameInfo = new UIManager.GameInfo()
                    {
                        gameId = m.payload.id,
                        applesCount = m.payload.collectedApples,
                        snakeLength = m.payload.snakeLength,
                        gameTime = TimeSpan.FromMilliseconds(m.payload.finishAt.Value - m.payload.startAt),
                    };

                    UIManager.Instance.ShowGameOver(() =>
                    {
                        Client.SendMessage(JSONSerializer.Serialize(
                        MessageFactory.StartGameMessage
                        ));
                        UIManager.Instance.ShowLoading();
                    }, gameInfo);


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

        private void OnGameOver(in Entity entity)
        {
            GameOverAfter(gameOverEvent.delay);
        }

        private void OnAddPoints(in Entity entity)
        {
            AddPoints(addPointsEvent.addApples, addPointsEvent.snakeLength);
        }

        private void GameOverAfter(float delay)
        {
            Invoke(nameof(SendEndGameMessage), delay);
        }
        private void AddPoints(int pointsCount, int snakeLength)
        {
            PlayerData.AddPoints(pointsCount);
            PlayerData.SetSnakeLength(snakeLength);

            SendUpdateGameMessage();
        }
    }
}
