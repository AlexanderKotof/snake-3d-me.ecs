using ME.ECS;

namespace Game.Features.Collectables.Views
{
    using Game.Components;
    using ME.ECS.Views.Providers;
    using System;
    using TMPro;
    using UnityEngine;

    public class CollectableView : MonoBehaviourView
    {
        public GameObject timerView;
        public TMP_Text timerText;
        public override bool applyStateJob => true;

        public override void OnInitialize()
        {
             timerView.SetActive(entity.Has<DestroyAfterComponent>());
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

            if (entity.Has<DestroyAfterComponent>())
            {
                var destroy = entity.Read<DestroyAfterComponent>();
                var span = TimeSpan.FromSeconds(destroy.destoyAfter - destroy.timer);
                timerText.SetText($"{span.Seconds}:{span.Milliseconds}");
            }
        }
    }
}