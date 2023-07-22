using ME.ECS;

namespace Game.Features.Player.Views
{
    using Game.Features.Player.Components;
    using ME.ECS.Views.Providers;
    using System.Threading.Tasks;
    using UnityEngine;

    public class SnakeView : MonoBehaviourView
    {
        public override bool applyStateJob => true;

        public Animator animator;

        public GameObject faceGO;

        private const string _destroyAnimationState = "DestroyAnim";

        public override void OnInitialize()
        {
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

            if (entity.Has<DestroyComponent>())
            {
                DestroyAnimation();
            }
        }

        private async void DestroyAnimation()
        {
            await Task.Delay((int)(entity.Read<DestroyComponent>().destoyAfter * 1000));
            animator.Play(_destroyAnimationState);
        }
    }
}