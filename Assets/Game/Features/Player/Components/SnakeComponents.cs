using ME.ECS;
using ME.ECS.Collections;
using UnityEngine;

namespace Game.Features.PlayerFeature.Components
{
    public struct IsSnake : IComponent { }

    public struct MovementDirection : IComponent
    {
        public Vector3 value;
    }

    public struct SnakeComponent : IComponent, ICopyable<SnakeComponent>
    {
        public BufferArray<Entity> tail;

        public void CopyFrom(in SnakeComponent other)
        {
            tail = other.tail;
        }

        public void OnRecycle()
        {

        }
    }
}