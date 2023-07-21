using ME.ECS;

namespace Game.Modules {
    
    using Components; using Modules; using Systems; using Features; using Markers;
    using UnityEngine.Windows;
    using UnityEngine.XR;
    using Game.Features.Input.Markers;
    using UnityEngine;
    using Game.Features.PlayerFeature.Components;

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class KeyboardInputModule : IModule, IUpdate
    {
        private PlayerInputFeature feature;

        private PlayersFeature playerFeature;

        public World world { get; set; }
        
        void IModuleBase.OnConstruct() {

            world.GetFeature(out feature);
            world.GetFeature(out playerFeature);
        }
        
        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime)
        {
            var currentDirection = playerFeature.PlayerEntity.Read<MovementDirection>().value;
            var input = new Vector3(UnityEngine.Input.GetAxis(feature.horizontalAxis), 0, UnityEngine.Input.GetAxis(feature.verticalAxis));

            if (input.x == 0 && input.z == 0)
                return;

            var dotProduct = Vector3.Dot(input, currentDirection);
            if (dotProduct != 0)
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
