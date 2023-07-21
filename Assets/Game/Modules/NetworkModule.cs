namespace Game.Modules {
    
    using TState = GameState;
    
    /// <summary>
    /// We need to implement our own NetworkModule class without any logic just to catch your State type into ECS.Network
    /// You can use some overrides to setup history config for your project
    /// </summary>
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class NetworkModule : ME.ECS.Network.NetworkModule<TState> {

        protected override int GetRPCOrder() {

            // Order all RPC packages by world id
            return this.world.id;

        }

        protected override ME.ECS.Network.NetworkType GetNetworkType() {
            
            // Initialize network with RunLocal and SendToNet
            return ME.ECS.Network.NetworkType.SendToNet | ME.ECS.Network.NetworkType.RunLocal;
            
        }

        protected override void OnInitialize() {

            var instance = (ME.ECS.Network.INetworkModuleBase)this;
            instance.SetTransporter(new FakeTransporter(this.GetNetworkType()));
            instance.SetSerializer(new FSSerializer());

        }

    }

    public class FakeTransporter : ME.ECS.Network.ITransporter
    {

        private struct Buffer
        {

            public byte[] data;

        }

        private readonly System.Collections.Generic.Queue<Buffer> buffers = new System.Collections.Generic.Queue<Buffer>();
        private readonly ME.ECS.Network.NetworkType networkType;
        private int sentCount;
        private int sentBytesCount;
        private int receivedCount;
        private int receivedBytesCount;
        private double ping;

        public FakeTransporter(ME.ECS.Network.NetworkType networkType)
        {

            this.networkType = networkType;

        }

        public bool IsConnected()
        {

            return true;

        }

        public void SendSystem(byte[] bytes)
        {

            this.Send(bytes);

        }

        public void Send(byte[] bytes)
        {

            if ((this.networkType & ME.ECS.Network.NetworkType.RunLocal) == 0)
            {

                // Add to local buffer if RunLocal flag is not set.
                // If flag RunLocal is set, this event has been run already.
                // This is FakeTransporter behaviour only.
                this.AddToBuffer(bytes);

            }

            // TODO: Here you need to send bytes array via your real transport layer to test real network environment.

            this.sentBytesCount += bytes.Length;
            ++this.sentCount;

        }

        private void AddToBuffer(byte[] bytes)
        {

            this.buffers.Enqueue(new Buffer()
            {
                data = bytes,
            });

        }

        private Buffer currentBuffer;

        public byte[] Receive()
        {

            // This method run every tick and should return data from network
            // byte[] array will be deserialized by ISerializer into HistoryEvent

            if (this.currentBuffer.data != null && this.currentBuffer.data.Length > 0)
            {

                this.receivedBytesCount += this.currentBuffer.data.Length;
                ++this.receivedCount;
                return this.currentBuffer.data;

            }

            if (this.buffers.Count > 0)
            {

                var buffer = this.buffers.Dequeue();
                this.currentBuffer = buffer;

            }

            return null;

        }

        public int GetEventsSentCount()
        {

            return this.sentCount;

        }

        public int GetEventsBytesSentCount()
        {

            return this.sentBytesCount;

        }

        public int GetEventsReceivedCount()
        {

            return this.receivedCount;

        }

        public int GetEventsBytesReceivedCount()
        {

            return this.receivedBytesCount;

        }

    }

    public class FSSerializer : ME.ECS.Network.ISerializer
    {

        public byte[] SerializeStorage(ME.ECS.StatesHistory.HistoryStorage historyEvent)
        {

            return ME.ECS.Serializer.Serializer.Pack(historyEvent);

        }

        public ME.ECS.StatesHistory.HistoryStorage DeserializeStorage(byte[] bytes)
        {

            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryStorage>(bytes);

        }

        public byte[] Serialize(ME.ECS.StatesHistory.HistoryEvent historyEvent)
        {

            return ME.ECS.Serializer.Serializer.Pack(historyEvent);

        }

        public ME.ECS.StatesHistory.HistoryEvent Deserialize(byte[] bytes)
        {

            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryEvent>(bytes);

        }

        public byte[] SerializeWorld(ME.ECS.World.WorldState data)
        {

            return ME.ECS.Serializer.Serializer.Pack(data);

        }

        public ME.ECS.World.WorldState DeserializeWorld(byte[] bytes)
        {

            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.World.WorldState>(bytes);

        }

    }
}