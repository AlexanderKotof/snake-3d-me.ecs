using ME.ECS;

namespace Game.Components
{
    public struct DestroyAfterComponent : IComponent
    {
        public float destoyAfter;
        public float timer;
    }
}