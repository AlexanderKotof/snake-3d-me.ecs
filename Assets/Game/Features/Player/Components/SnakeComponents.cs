using ME.ECS;
using ME.ECS.Buffers;
using ME.ECS.Collections;
using UnityEngine;

namespace Game.Features.Player.Components
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
            ArrayUtils.Copy(other.tail, ref tail);
        }

        public void OnRecycle()
        {
            PoolArray<Entity>.Recycle(tail);
        }
    }
}