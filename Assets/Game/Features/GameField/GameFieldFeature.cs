using ME.ECS;

namespace Game.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using GameField.Components; using GameField.Modules; using GameField.Systems; using GameField.Markers;
    using ME.ECS.Views;

    namespace GameField.Components {}
    namespace GameField.Modules {}
    namespace GameField.Systems {}
    namespace GameField.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class GameFieldFeature : Feature
    {
        public int xSize;
        public int ySize;

        protected override void OnConstruct()
        {
        }

        protected override void OnDeconstruct() {
            
        }

    }

}