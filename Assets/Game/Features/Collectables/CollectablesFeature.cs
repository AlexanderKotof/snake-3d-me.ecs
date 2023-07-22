﻿using ME.ECS;

namespace Game.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Collectables.Components; using Collectables.Modules; using Collectables.Systems; using Collectables.Markers;
    using Game.Features.Collectables.Views;
    using System;

    namespace Collectables.Components {}
    namespace Collectables.Modules {}
    namespace Collectables.Systems {}
    namespace Collectables.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class CollectablesFeature : Feature
    {
        [Serializable]
        public class Collectable
        {
            public CollectableView view;
            public ViewId viewId;
            public int points;
            public int snakeGrow;
        }

        public Collectable[] collectables;

        public int bananaRate = 5;

        protected override void OnConstruct()
        {
            foreach(var collectable in collectables)
            {
                collectable.viewId = world.RegisterViewSource(collectable.view);
            }

            AddSystem<SpawnCollectableSystem>();
            AddSystem<CollectSystem>();
        }

        protected override void OnDeconstruct()
        {
            
        }

        public Collectable GetCollectable(int counter)
        {
            var collectableType = counter > 0 && counter % bananaRate == 0 ? 1 : 0;
            var collectable = collectables[collectableType];
            return collectable;
        }
    }

}