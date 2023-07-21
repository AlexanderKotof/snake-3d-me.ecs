using ME.ECS;

namespace Game.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Collectables.Components; using Collectables.Modules; using Collectables.Systems; using Collectables.Markers;
    
    namespace Collectables.Components {}
    namespace Collectables.Modules {}
    namespace Collectables.Systems {}
    namespace Collectables.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class CollectablesFeature : Feature {

        protected override void OnConstruct() {
            
        }

        protected override void OnDeconstruct() {
            
        }

    }

}