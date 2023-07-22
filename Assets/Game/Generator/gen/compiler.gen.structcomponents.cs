namespace ME.ECS {

    public static partial class ComponentsInitializer {

        static partial void InitTypeIdPartial() {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Game.Features.Collectables.Components.CollectableComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.Collectables.Components.CollectablesCounterComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.GameField.Components.GameFieldSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.MovementDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.PositionComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Components.InputComponent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.GameField.Components.IsGameField>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.IsSnake>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.SnakeComponent>(false, false, false, false, true, false, false, false, false);

        }

        static partial void Init(State state, ref ME.ECS.World.NoState noState) {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Game.Features.Collectables.Components.CollectableComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.Collectables.Components.CollectablesCounterComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.GameField.Components.GameFieldSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.MovementDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.PositionComponent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Components.InputComponent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.GameField.Components.IsGameField>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.IsSnake>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Game.Features.PlayerFeature.Components.SnakeComponent>(false, false, false, false, true, false, false, false, false);

            ComponentsInitializerWorld.Setup(ComponentsInitializerWorldGen.Init);
            CoreComponentsInitializer.Init(state, ref noState);


            state.structComponents.ValidateUnmanaged<Game.Features.Collectables.Components.CollectableComponent>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Game.Features.Collectables.Components.CollectablesCounterComponent>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Game.Features.GameField.Components.GameFieldSize>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Game.Features.PlayerFeature.Components.MovementDirection>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Game.Features.PlayerFeature.Components.PositionComponent>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Game.Components.InputComponent>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Game.Features.GameField.Components.IsGameField>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Game.Features.PlayerFeature.Components.IsSnake>(ref state.allocator, true);
            state.structComponents.ValidateCopyable<Game.Features.PlayerFeature.Components.SnakeComponent>(false);

        }

    }

    public static class ComponentsInitializerWorldGen {

        public static void Init(Entity entity) {


            entity.ValidateDataUnmanaged<Game.Features.Collectables.Components.CollectableComponent>(false);
            entity.ValidateDataUnmanaged<Game.Features.Collectables.Components.CollectablesCounterComponent>(false);
            entity.ValidateDataUnmanaged<Game.Features.GameField.Components.GameFieldSize>(false);
            entity.ValidateDataUnmanaged<Game.Features.PlayerFeature.Components.MovementDirection>(false);
            entity.ValidateDataUnmanaged<Game.Features.PlayerFeature.Components.PositionComponent>(false);
            entity.ValidateDataUnmanaged<Game.Components.InputComponent>(true);
            entity.ValidateDataUnmanaged<Game.Features.GameField.Components.IsGameField>(true);
            entity.ValidateDataUnmanaged<Game.Features.PlayerFeature.Components.IsSnake>(true);
            entity.ValidateDataCopyable<Game.Features.PlayerFeature.Components.SnakeComponent>(false);

        }

    }

}
