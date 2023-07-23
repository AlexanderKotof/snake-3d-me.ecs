using UnityEngine;

namespace Game
{
    public static class GameConfig
    {
        public const string uri = "wss://dev.match.qubixinfinity.io/snake";

        public const string gameCreatedMessageType = "game-created";
        public const string gameEndedMessageType = "game-ended";

        public const string menuSceneName = "Startup";
        public const string gameSceneName = "Game";

        public static bool IsMobilePlatform => Application.isMobilePlatform;
    }

}
