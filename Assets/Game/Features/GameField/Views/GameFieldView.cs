using ME.ECS;

namespace Game.Features.GameField.Views {
    
    using ME.ECS.Views.Providers;
    using UnityEngine;

    public class GameFieldView : MonoBehaviourView {

        public Renderer _renderer;
        public override bool applyStateJob => true;

        public override void OnInitialize()
        {
            var gameFieldSize = entity.Read<Components.GameFieldSize>();
            transform.localScale = new Vector3(gameFieldSize.xSize + 1, 1, gameFieldSize.ySize + 1);

            _renderer.material.mainTextureScale = new Vector2(gameFieldSize.xSize / 2, gameFieldSize.ySize / 2);
        }
        
        public override void OnDeInitialize() {
            
        }
        
        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately)
        {

        }
        
        public override void ApplyState(float deltaTime, bool immediately) {
            
        }
        
    }
    
}