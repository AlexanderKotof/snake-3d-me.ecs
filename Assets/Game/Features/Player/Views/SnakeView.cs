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

        public override void OnInitialize()
        {
            world.GetFeature(out feature);
        }

        public override void OnDeInitialize()
        {

        }

        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately)
        {

        }

        public override void ApplyState(float deltaTime, bool immediately)
        {
            targetPosition = entity.Read<PositionComponent>().value;
        }

        private void Update()
        {
            if ((transform.position - targetPosition).sqrMagnitude < 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, world.GetTickTime() / feature.ticksPerMove * Time.deltaTime * movementSpeed);
            }
            else
                transform.position = targetPosition;
        }

    }

}