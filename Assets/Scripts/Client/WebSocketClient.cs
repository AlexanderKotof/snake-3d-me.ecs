using HybridWebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Client
{
    public interface IClient
    {
        void StartConnection(string uri);

        void SendMessage(string message);

        void CloseConnection();

        bool HasMessageReceived(out string message);

        event Action ConnectedToServer;

        event Action OnDisconected;
    }
    

    public class WebSocketClient : IClient
    {
        private WebSocket _client;

        private Queue<string> _messageQueue = new Queue<string>();

        public event Action ConnectedToServer;
        public event Action OnDisconected;


        public void StartConnection(string uri)
        {
            Debug.Log($"Connecting to server {uri}");

            _client = WebSocketFactory.CreateInstance(uri);

            _client.OnError += _client_OnError;
            _client.OnOpen += _client_OnOpen;
            _client.OnMessage += _client_OnMessage;
            _client.OnClose += _client_OnClose;

            _client.Connect();
        }

        private void _client_OnClose(WebSocketCloseCode closeCode)
        {
            Debug.Log($"Disconected, close code {(int)closeCode}-{closeCode}");
            OnDisconected?.Invoke();
        }

        private void _client_OnMessage(byte[] data)
        {
            string message = Encoding.UTF8.GetString(data, 0, data.Length);
            Debug.Log($"Message received {message}");
            _messageQueue.Enqueue(message);
        }

        private void _client_OnError(string errorMsg)
        {
            Debug.LogError($"Error: {errorMsg}");
        }

        public void CloseConnection()
        {
            _client.OnOpen -= _client_OnOpen;
            _client.OnMessage -= _client_OnMessage;
            _client.OnClose -= _client_OnClose;
            _client.OnError -= _client_OnError;

            if (_client.GetState() != WebSocketState.Closed)
                _client.Close();
            
            _client = null;
        }

        public bool HasMessageReceived(out string message)
        {
            return _messageQueue.TryDequeue(out message);
        }

        public void SendMessage(string message)
        {
            Debug.Log($"Send message {message}");
            var bytes = Encoding.UTF8.GetBytes(message);
            _client.Send(bytes);
        }
 
        private void _client_OnOpen()
        {
            Debug.Log($"Connected to server!");
            ConnectedToServer?.Invoke();
        }
    }
}
