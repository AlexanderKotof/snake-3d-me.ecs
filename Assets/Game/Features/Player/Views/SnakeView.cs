using ME.ECS;

namespace Game.Features.Player.Views
{
    using Game.Components;
    using Game.Features.Player.Components;
    using ME.ECS.Views.Providers;
    using System.Threading.Tasks;
    using UnityEngine;

    public class SnakeView : MonoBehaviourView
    {
        public override bool applyStateJob => false;
        public float movementSpeed = 0.5f;

        private Vector3 _destination;
        private MovementFeature _movementFeature;

        public Animator animator;
        public GameObject faceGO;

        private const string _destroyAnimationState = "DestroyAnim";

        public override void OnInitialize()
        {
            _movementFeature = world.GetFeature<MovementFeature>();
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
            _destination = entity.Read<PositionComponent>().value;

            if (entity.Has<MovementDirection>())
            {
                transform.rotation = Quaternion.LookRotation(entity.Read<MovementDirection>().value);
            }

            if (entity.Has<DestroyComponent>())
            {
                DestroyAnimation();
            }
        }

        private void Update()
        {
            if ((_destination - transform.position).sqrMagnitude <= 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destination, _movementFeature.ticksPerMove * world.GetTickTime() * movementSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = _destination;
            }
        }

        private async void DestroyAnimation()
        {
            await Task.Delay((int)(entity.Read<DestroyComponent>().destoyAfter * 1000));
            animator.Play(_destroyAnimationState);
        }
    }
}