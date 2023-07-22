using Game.Client;
using System;

namespace Game
{
    public static class ServicesProvider
    {
        public static IClient Client { get; private set; }

        public static void Initialize(IClient client)
        {
            Client = client;
        }

        public static void Dispose()
        {
            Client = null;
        }
    }
}
