using System;

namespace Game.Client.Messages
{
    public static class MessageFactory
    {
        private const string _createGameMessageType = "create-game";
        private const string _updateGameDataMessageType = "collect-apple";
        private const string _endGameMessageType = "end-game";

        public static readonly IMessage StartGameMessage = new OutgoingMessage(_createGameMessageType, null);

        public static IMessage CreateUpdateGameMessage(int gameId, int collectedApples, int snakeLength)
        {
            var message = new OutgoingMessage(_updateGameDataMessageType, new OutgoingMessage.UpdateGamePayload
            {
                game_id = gameId,
                appleCount = collectedApples,
                snakeLength = snakeLength,
            });
            return message;
        }

        public static IMessage CreateEndGameMessage(int gameId)
        {
            return new OutgoingMessage(_endGameMessageType, new OutgoingMessage.EndGamePayload
            {
                game_id = gameId,
            });
        }
    }

    public interface IMessage { }
    public interface IPayload { }

    [Serializable]
    public class OutgoingMessage : IMessage
    {
        public string type;
        public IPayload payload;

        [Serializable]
        public class UpdateGamePayload : IPayload
        {
            public int game_id;
            public int appleCount;
            public int snakeLength;
        }

        [Serializable]
        public class EndGamePayload : IPayload
        {
            public int game_id;
        }

        public OutgoingMessage(string type, IPayload payload)
        {
            this.type = type;
            this.payload = payload;
        }
    }

    [Serializable]
    public class IncomingMessage : IMessage
    {
        public string type;
        public Payload payload;

        [Serializable]
        public class Payload
        {
            public string clientAddress;
            public long startAt;
            public long? finishAt;
            public int id;
            public int collectedApples;
            public int snakeLength;
            public DateTime created_at;
            public DateTime updated_at;
        }

        public IncomingMessage() { }
    }
}
