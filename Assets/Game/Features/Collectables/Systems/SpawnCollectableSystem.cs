using ME.ECS;

namespace Game.Features.Collectables.Systems
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
    using System;
    using UnityEngine;
#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SpawnCollectableSystem : ISystem, ISystemBase, IAdvanceTick, IAdvanceTickBase
    {
        private CollectablesFeature feature;
        private PlayerFeature playersFeature;

        private Filter _snakeFilter;
        private Filter _collectablesFilter;

        private Entity _collectablesCounter;
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out feature);
            this.GetFeature(out playersFeature);

            _collectablesCounter = world.AddEntity("CollectablesCounter");
            _collectablesCounter.Set<CollectablesCounterComponent>();

            _snakeFilter = Filter.Create("SnakeFilter")
                .With<IsSnake>()
                .Push();

            _collectablesFilter = Filter.Create("CollectablesFilter")
                .With<CollectableComponent>()
                .Push();
        }
        
        void ISystemBase.OnDeconstruct() {}

        public void AdvanceTick(in float deltaTime)
        {
            if (playersFeature.PlayerEntity.IsEmpty() || _collectablesFilter.Count > 0)
                return;

            ref var counterComponent = ref _collectablesCounter.Get<CollectablesCounterComponent>();
            var collectableType = feature.GetCollectable(counterComponent.counter);
            counterComponent.counter++;

            var collectableEnt = world.AddEntity("collectable");
            collectableEnt.Set(new CollectableComponent
            {
                pointsCount = collectableType.points,
                snakeGrow = collectableType.snakeGrow,
            });

            var randomPosition = GenerateRandomPosition();
            collectableEnt.Set(new PositionComponent { value = randomPosition });

            world.InstantiateView(collectableType.viewId, collectableEnt);
        }

        private Vector3 GenerateRandomPosition()
        {
            Vector3 randomPosition;
            bool intersection;

            do
            {
                intersection = false;
                randomPosition = GenerateRandomVectorOnGameField();
                foreach (var ent in _snakeFilter)
                {
                    if (ent.Read<PositionComponent>().value != randomPosition)
                        continue;

                    intersection = true;
                    break;
                }

            } while (intersection);

            return randomPosition;
        }

        private Vector3 GenerateRandomVectorOnGameField()
        {
            var gameFieldFeature = world.GetFeature<GameFieldFeature>();
            return new Vector3(
                UnityEngine.Random.Range(-gameFieldFeature.xSize / 2, gameFieldFeature.xSize / 2),
                0,
                UnityEngine.Random.Range(-gameFieldFeature.ySize / 2, gameFieldFeature.ySize / 2));
        }

    }
}