using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Client
{
    public class WebSocketClientNoWeb : IClient
    {
        public Uri Uri { get; private set; }

        private ClientWebSocket Socket { get; set; }

        private BlockingCollection<string> SendQueue = new BlockingCollection<string>();

        private Queue<string> ReceivedMessages = new Queue<string>();

        private CancellationTokenSource SocketLoopTokenSource;
        private CancellationTokenSource SendLoopTokenSource;

        public event Action ConnectedToServer;
        public event Action OnDisconected;

        private const int CLOSE_SOCKET_TIMEOUT_MS = 10000;
        private const int KEYSTROKE_TRANSMIT_INTERVAL_MS = 500;

        public async void StartConnection(string uri)
        {
            Uri = new Uri(uri);

            Debug.Log($"Connecting to server {Uri}");

            SocketLoopTokenSource = new CancellationTokenSource();
            SendLoopTokenSource = new CancellationTokenSource();

            try
            {
                Socket = new ClientWebSocket();
                await Socket.ConnectAsync(Uri, CancellationToken.None);

                _ = Task.Run(() => SocketProcessingLoopAsync().ConfigureAwait(false));
                _ = Task.Run(() => TransmitLoopAsync().ConfigureAwait(false));

                Debug.Log($"Connected to server");

                ConnectedToServer?.Invoke();
            }
            catch (OperationCanceledException)
            {
                // normal upon task/token cancellation, disregard
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void SendMessage(string message)
        {
            SendQueue.Add(message);
        }

        public async void CloseConnection()
        {
            Debug.Log("Closing connection");

            SendLoopTokenSource.Cancel();
            SocketLoopTokenSource.Cancel();

            if (Socket == null || Socket.State != WebSocketState.Open)
                return;

            // close the socket first, because ReceiveAsync leaves an invalid socket (state = aborted) when the token is cancelled
            var timeout = new CancellationTokenSource(CLOSE_SOCKET_TIMEOUT_MS);
            try
            {
                // after this, the socket state which change to CloseSent
                await Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", timeout.Token);
                // now we wait for the server response, which will close the socket
                while (Socket.State != WebSocketState.Closed && !timeout.Token.IsCancellationRequested);
            }
            catch (OperationCanceledException)
            {
                // normal upon task/token cancellation, disregard
            }
            // whether we closed the socket or timed out, we cancel the token causing RecieveAsync to abort the socket

            // the finally block at the end of the processing loop will dispose and null the Socket object
        }

        private async Task SocketProcessingLoopAsync()
        {
            var cancellationToken = SocketLoopTokenSource.Token;
            try
            {
                var buffer = WebSocket.CreateClientBuffer(4096, 4096);
                while (Socket.State != WebSocketState.Closed && !cancellationToken.IsCancellationRequested)
                {
                    var receiveResult = await Socket.ReceiveAsync(buffer, cancellationToken);
                    // if the token is cancelled while ReceiveAsync is blocking, the socket state changes to aborted and it can't be used
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        // the server is notifying us that the connection will close; send acknowledgement
                        if (Socket.State == WebSocketState.CloseReceived && receiveResult.MessageType == WebSocketMessageType.Close)
                        {
                            Debug.Log("Close frame received from server");
                            SendLoopTokenSource.Cancel();
                            await Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Acknowledge Close frame", CancellationToken.None);
                        }

                        // display text or binary data
                        if (Socket.State == WebSocketState.Open && receiveResult.MessageType != WebSocketMessageType.Close)
                        {
                            string message = Encoding.UTF8.GetString(buffer.Array, 0, receiveResult.Count);
                            ReceivedMessages.Enqueue(message);
                        }
                    }
                }

                OnDisconected?.Invoke();

                Debug.Log($"Ending processing loop in state {Socket.State}");
            }
            catch (OperationCanceledException)
            {
                // normal upon task/token cancellation, disregard
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                SendLoopTokenSource.Cancel();
                Socket.Dispose();
                Socket = null;
            }
        }

        private async Task TransmitLoopAsync()
        {
            var cancellationToken = SendLoopTokenSource.Token;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(KEYSTROKE_TRANSMIT_INTERVAL_MS, cancellationToken);
                    if (!cancellationToken.IsCancellationRequested && SendQueue.TryTake(out var message))
                    {
                        Debug.Log($"Sending message: {message}");

                        var msgbuf = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                        await Socket.SendAsync(msgbuf, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
                    }
                }
                catch (OperationCanceledException)
                {
                    // normal upon task/token cancellation, disregard
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public bool HasMessageReceived(out string message)
        {
            return ReceivedMessages.TryDequeue(out message);
        }
    }
}
