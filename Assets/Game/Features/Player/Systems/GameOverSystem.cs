using ME.ECS;

namespace Game.Features.Player.Systems
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
    using Game.Features.Player.Components;
    using UnityEngine;
    using System;
#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class GameOverSystem : ISystem, IAdvanceTick
    {
        private PlayerFeature _feature;

        private Filter _snakeFilter;

        public World world { get; set; }

        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out this._feature);

            // all snake tail without head
            _snakeFilter = Filter.Create("Snake").With<IsSnake>().Without<SnakeComponent>().Push();
        }
        
        void ISystemBase.OnDeconstruct()
        {
        }
        
        void IAdvanceTick.AdvanceTick(in float deltaTime)
        {
            if (_feature.PlayerEntity.IsEmpty())
                return;

            var playerPosition = _feature.PlayerEntity.Read<PositionComponent>().value;

            foreach (var tail in _snakeFilter)
            {
                if (tail.Read<PositionComponent>().value != playerPosition)
                    continue;

                DestroySnake();
                break;
            }
        }

        private void DestroySnake()
        {
            float timeOffset = _feature.destroyTimePerSegment;
            _feature.PlayerEntity.Set(new DestroyAfterComponent { destoyAfter = timeOffset });

            foreach (var snakeSegment in _snakeFilter)
            {
                timeOffset += _feature.destroyTimePerSegment;
                snakeSegment.Set(new DestroyAfterComponent { destoyAfter = timeOffset });
            }

            _feature.gameOverEvent.Execute(timeOffset);

            world.Pause();
        }
    }
}