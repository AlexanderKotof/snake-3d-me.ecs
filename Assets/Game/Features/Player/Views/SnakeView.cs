using ME.ECS;

namespace Game.Features.PlayerFeature.Views
{
    using Game.Features.PlayerFeature.Components;
    using ME.ECS.Views.Providers;
    using UnityEngine;

    public class SnakeView : MonoBehaviourView
    {
        Vector3 targetPosition;
        public override bool applyStateJob => true;

        MovementFeature feature;
        public float movementSpeed;

        public GameObject faceGO;

        public override void OnInitialize()
        {
            world.GetFeature(out feature);
            faceGO.SetActive(entity.Has<SnakeComponent>());
        }

        public override void OnDeInitialize()
        {

        }

        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately)
        {

        }

        public override void ApplyState(float deltaTime, bool immediately)
        {
            transform.position = entity.Read<PositionComponent>().value;

            if (entity.Has<MovementDirection>())
            {
                transform.rotation = Quaternion.LookRotation(entity.Read<MovementDirection>().value);
            }
        }
    }
}