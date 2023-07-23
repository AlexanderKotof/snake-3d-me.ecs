using ME.ECS;

namespace Game.Features.Input.Systems
{

#pragma warning disable
    using Game.Components;
    using Game.Modules;
    using Game.Systems;
    using Game.Markers;
    using Components;
    using Modules;
    using Systems;
    using Markers;
    using UnityEngine;
    using Game.Features.Player.Components;
    using UnityEngine.Windows;
#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class InputToRPCSystem : ISystem, IUpdate
    {
        private PlayerFeature _playerFeature;

        private RPCId inputRPCId;

        public World world { get; set; }
        
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _playerFeature);

            var network = world.GetModule<NetworkModule>();
            network.RegisterObject(this);

            inputRPCId = network.RegisterRPC(new System.Action<Vector3>(ChangeDirection).Method);
        }
        
        void ISystemBase.OnDeconstruct() {}

        public void Update(in float deltaTime)
        {
            if (world.GetMarker(out InputMarker marker))
            {
                var network = world.GetModule<NetworkModule>();
                network.RPC(this, inputRPCId, marker.inputDirection);
            }
        }

        private void ChangeDirection(Vector3 dir)
        {
            var playerEntity = _playerFeature.PlayerEntity;
            var currentDirection = playerEntity.Read<MovementDirection>().value;

            var dotProduct = Vector3.Dot(dir, currentDirection);
            // exclude same or opposite directions
            if (dotProduct != 0)
                return;

            var firstTailSegment = playerEntity.Read<SnakeComponent>().tail[0];
            var fromHeadToFirstDir = firstTailSegment.Read<PositionComponent>().value - playerEntity.Read<PositionComponent>().value;

            dotProduct = Vector3.Dot(fromHeadToFirstDir, dir);
            //exclude direction to tail
            if (dotProduct > 0)
                return;

            playerEntity.Set(new MovementDirection
            {
                value = dir,
            });
        }
    }
    
}