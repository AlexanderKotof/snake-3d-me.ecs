using ME.ECS;

namespace Game.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using GameFieldFeature.Components; using GameFieldFeature.Modules; using GameFieldFeature.Systems; using GameFieldFeature.Markers;
    using Game.Features.GameField.Components;
    using ME.ECS.Views;

    namespace GameFieldFeature.Components {}
    namespace GameFieldFeature.Modules {}
    namespace GameFieldFeature.Systems {}
    namespace GameFieldFeature.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class GameFieldFeatureFeature : Feature
    {
        public int xSize;
        public int ySize;

        public GameField.Views.GameFieldView view;

        public ViewId GameFieldViewId { get; private set; }

        protected override void OnConstruct()
        {
            var entity = world.AddEntity("GameField");
            entity.Set(new GameFieldSize()
            {
                xSize = xSize,
                ySize = ySize,
            });

            entity.Set<IsGameField>();

            GameFieldViewId = world.RegisterViewSource(view);
            world.InstantiateView(GameFieldViewId, entity);
        }

        protected override void OnDeconstruct() {
            
        }

    }

}