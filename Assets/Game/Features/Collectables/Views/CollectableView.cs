using ME.ECS;

namespace Game.Features.Collectables.Views
{
    using Game.Components;
    using ME.ECS.Views.Providers;

    public class CollectableView : MonoBehaviourView
    {
        public override bool applyStateJob => true;

        public override void OnInitialize()
        {
            
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
        }
    }
}