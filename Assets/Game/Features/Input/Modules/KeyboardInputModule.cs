using ME.ECS;

namespace Game.Modules
{
    using Features;
    using Game.Features.Input.Markers;
    using Game.Features.Player.Components;
    using UnityEngine;

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class KeyboardInputModule : IModule, IUpdate
    {
        private PlayerInputFeature _inputFeature;
        private PlayerFeature _playerFeature;

        public World world { get; set; }
        
        void IModuleBase.OnConstruct() {

            world.GetFeature(out _inputFeature);
            world.GetFeature(out _playerFeature);
        }
        
        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime)
        {
            var input = new Vector3(Input.GetAxis(_inputFeature.horizontalAxis), 0, Input.GetAxis(_inputFeature.verticalAxis));

            if (input.x == 0 && input.z == 0)
                return;

            if (input.x != 0)
            {
                world.AddMarker(new InputMarker { inputDirection = input.x > 0 ? Vector3.right : Vector3.left });
            }

            if (input.z != 0)
            {
                world.AddMarker(new InputMarker { inputDirection = input.z > 0 ? Vector3.forward : Vector3.back });
            }
        }
        
    }
    
}
