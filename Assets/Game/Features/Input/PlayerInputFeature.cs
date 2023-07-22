using ME.ECS;

namespace Game.Features
{
    using Game.Features.Input.Systems;
    using Modules;

    namespace InputFeature.Components { }
    namespace InputFeature.Modules { }
    namespace InputFeature.Systems { }
    namespace InputFeature.Markers { }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayerInputFeature : Feature 
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";

        protected override void OnConstruct()
        {
            AddModule<KeyboardInputModule>();
            AddSystem<InputToRPCSystem>();
        }

        protected override void OnDeconstruct() {
            
        }

        
    }

}