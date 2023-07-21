using ME.ECS;

namespace Game.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Movement.Components; using Movement.Modules; using Movement.Systems; using Movement.Markers;

    namespace Movement.Components {}
    namespace Movement.Modules {}
    namespace Movement.Systems {}
    namespace Movement.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class MovementFeature : Feature {

        public int ticksPerMove = 10;

        protected override void OnConstruct() 
        {
            AddSystem<MovementSystem>();
        }

        protected override void OnDeconstruct() {
            
        }

    }

}