using ME.ECS;

namespace Game.Features.Movement.Systems {

    #pragma warning disable
    using Game.Components; using Game.Modules; using Game.Systems; using Game.Markers;
    using Components; using Modules; using Systems; using Markers;
    using System.Numerics;
    using Game.Features.Player.Components;
    using System;
    using UnityEngine;
#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class MovementSystem : ISystemFilter
    { 
        private MovementFeature _movementFeature;
        private GameFieldFeatureFeature _gameFieldFeature;

        public World world { get; set; }

        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out this._movementFeature);
            this.GetFeature(out this._gameFieldFeature);
        }

        void ISystemBase.OnDeconstruct() {}
        
        #if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        #endif

        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() 
        {
            return Filter.Create("Filter-MovementSystem")
                .With<PositionComponent>()
                .With<MovementDirection>()
                .With<SnakeComponent>()
                .Push();
        }
    
        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            if (world.GetCurrentTick().v % _movementFeature.ticksPerMove != _movementFeature.ticksPerMove - 1)
                return;

            ref var position = ref entity.Get<PositionComponent>();
            var movementDirection = entity.Read<MovementDirection>();

            var previousPosition = position.value;
            position.value += movementDirection.value;

            if (Mathf.Abs(position.value.x) > _gameFieldFeature.xSize / 2)
            {
                position.value.x = Mathf.Sign(position.value.x) - position.value.x;
            }
            if (Mathf.Abs(position.value.z) > _gameFieldFeature.ySize / 2)
            {
                position.value.z = Mathf.Sign(position.value.z) - position.value.z;
            }

            foreach (var tail in entity.Read<SnakeComponent>().tail)
            {
                ref var tailPosition = ref tail.Get<PositionComponent>();
                var pos = tailPosition.value;
                tailPosition.value = previousPosition;
                previousPosition = pos;
            }
        }
    }
}