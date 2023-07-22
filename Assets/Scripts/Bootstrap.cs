using Game.Client;
using Game.Client.Messages;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        public int GameId { get; private set; }

        private const string _uri = "wss://dev.match.qubixinfinity.io/snake";

        private const string _gameCreatedMessageType = "game-created";
        private const string _gameEndedMessageType = "game-ended";

        private void Start()
        {
            var client = new WebSocketClient();

            client.StartConnectionAsync(_uri);

            client.ConnectedToServer += OnConnected;
            client.OnDisconected += OnDisconected; ;
            client.OnMessageReceived += OnMessageReceived;

            ServicesProvider.Initialize(client);
        }

        private void OnConnected()
        {
            //TODO await button click and show loading screen
            ServicesProvider.Client.SendMessage(JSONSerializer.Serialize(
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
                case _gameCreatedMessageType:
                    GameId = m.payload.id;
                    LoadGameScene();
                    break;

                case _gameEndedMessageType:
                    Debug.Log("Game Ended message");
                    break;

                default:
                    Debug.Log($"Error message {message}");
                    break;
            }
        }

        private async void LoadGameScene()
        {
            var operation = SceneManager.LoadSceneAsync("Game");

            while (!operation.isDone)
                await Task.Yield();

            //TODO hide loading screen
        }

        [ContextMenu("Update game")]
        private void SendUpdateGameMessage()
        {
            ServicesProvider.Client.SendMessage(JSONSerializer.Serialize(
                MessageFactory.CreateUpdateGameMessage(GameId, Random.Range(1, 10), Random.Range(3, 10))
                ));
        }

        [ContextMenu("End game")]
        private void SendEndGameMessage()
        {
            ServicesProvider.Client.SendMessage(JSONSerializer.Serialize(
                MessageFactory.CreateEndGameMessage(GameId)
                ));
        }

        private void OnDestroy()
        {
            ServicesProvider.Client.ConnectedToServer -= OnConnected;
            ServicesProvider.Client.OnMessageReceived -= OnMessageReceived;
            ServicesProvider.Client.CloseConnectionAsync();
            ServicesProvider.Dispose();
        }
    }
}
