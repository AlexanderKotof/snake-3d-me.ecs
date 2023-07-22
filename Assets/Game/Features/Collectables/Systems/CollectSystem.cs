using ME.ECS;

namespace Game.Features.Collectables.Systems {

    #pragma warning disable
    using Game.Components; using Game.Modules; using Game.Systems; using Game.Markers;
    using Components; using Modules; using Systems; using Markers;
    using Game.Features.Player.Components;
    using System;
#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class CollectSystem : ISystemFilter {
        
        private CollectablesFeature feature;
        private PlayerFeature playerFeature;

        public static event Action<int, int> AddPoints;

        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out feature);
            this.GetFeature(out playerFeature);
        }
        
        void ISystemBase.OnDeconstruct() {}
        
        #if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        #endif
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-CollectSystem")
                .With<CollectableComponent>()
                .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            var collectablePosition = entity.Read<PositionComponent>().value;

            if (playerFeature.PlayerEntity.IsEmpty() || collectablePosition != playerFeature.PlayerEntity.Read<PositionComponent>().value)
                return;

            Collect(entity);

            entity.DestroyAllViews();

            world.RemoveEntity(entity);
        }

        private void Collect(Entity entity)
        {
            var collectable = entity.Read<CollectableComponent>();
            playerFeature.AddTailSegments(collectable.snakeGrow);

            var snakeLength = playerFeature.PlayerEntity.Read<SnakeComponent>().tail.Length + 1;
            AddPoints?.Invoke(collectable.pointsCount, snakeLength);
        }
    }
}