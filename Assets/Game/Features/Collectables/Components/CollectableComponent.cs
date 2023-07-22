using ME.ECS;

namespace Game.Features.Collectables.Components
{
    public struct CollectableComponent : IComponent
    {
        public int pointsCount;
        public int snakeGrow;
    }

    public struct CollectablesCounterComponent : IComponent
    {
        public int counter;
    }
}