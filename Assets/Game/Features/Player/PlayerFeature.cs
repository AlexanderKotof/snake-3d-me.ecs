﻿using ME.ECS;

namespace Game.Features
{

    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;
    using Player.Components;
    using Player.Modules;
    using Player.Systems;
    using Player.Markers;
    using Game.Features.Player.Views;
    using ME.ECS.Collections;
    using UnityEngine;
    using System;
    using Game.Features.Player.Systems;
    using Game.Events;

    namespace Player.Components { }
    namespace Player.Modules { }
    namespace Player.Systems { }
    namespace Player.Markers { }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayerFeature : Feature
    {
        public SnakeView snakeView;
        public int spawnCount;

        public float destroyTimePerSegment = 0.5f;

        public GlobalEvent playerCreatedEvent;
        public GameOverGlobalEvent gameOverEvent;

        private RPCId createPlayerRPCId;

        public ViewId ViewId { get; private set; }

        public Entity PlayerEntity { get; private set; }

        protected override void OnConstruct()
        {
            ViewId = world.RegisterViewSource(snakeView);

            var network = world.GetModule<NetworkModule>();
            network.RegisterObject(this);
            createPlayerRPCId = network.RegisterRPC(new Action(CreatePlayer).Method);

            network.RPC(this, createPlayerRPCId);

            AddSystem<GameOverSystem>();
        }

        private void CreatePlayer()
        {
            var tail = new Entity[spawnCount - 1];

            for (int i = 0; i < spawnCount; i++)
            {
                var entity = world.AddEntity($"Snake{i}");

                if (i == 0)
                {
                    entity.Set(new MovementDirection 
                    {
                        value = Vector3.left 
                    });
                    SetPlayer(entity);
                }
                else
                {
                    tail[i - 1] = entity;
                }

                entity.Set<IsSnake>();
                entity.Set(new PositionComponent 
                {
                    value = new Vector3(i, 0, 0) 
                });

                world.InstantiateView(ViewId, entity);
            }

            PlayerEntity.Set(new SnakeComponent { tail = new BufferArray<Entity>(tail, tail.Length) });

            playerCreatedEvent.Execute(PlayerEntity);
        }

        public void SetPlayer(in Entity entity)
        {
            PlayerEntity = entity;
        }

        public void AddTailSegments(int count)
        {
            ref var tailComponent = ref PlayerEntity.Get<SnakeComponent>();

            for (int i = 0; i < count; i++)
            {
                var tailLength = tailComponent.tail.Length;

                var entity = world.AddEntity($"Snake{tailLength}");

                entity.Set<IsSnake>();

                var previousPosition = tailComponent.tail[tailLength - 1].Read<PositionComponent>().value;
                var deltaPosition = previousPosition - tailComponent.tail[tailLength - 2].Read<PositionComponent>().value;

                entity.Set(new PositionComponent { value = previousPosition + deltaPosition });

                tailComponent.tail.Resize(tailLength + 1);

                tailComponent.tail[tailLength] = entity;

                world.InstantiateView(ViewId, entity);
            }
        }

        protected override void OnDeconstruct()
        {
            
        }
    }
}