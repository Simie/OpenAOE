using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Utility;
using OpenAOE.Services;

namespace OpenAOE.Utilities
{
    /// <summary>
    /// A "Bag" of entities that updates with new entities that match the filter.
    /// </summary>
    public sealed class EntityBag : IDisposable
    {
        public IComponentFilter Filter { get; }

        public event Action<EntityBag, IReadOnlyEntity> Added;
        public event Action<EntityBag, IReadOnlyEntity> Removed;

        public IReadOnlyList<IReadOnlyEntity> Contents { get { return _contents; } }

        private readonly List<IReadOnlyEntity> _contents = new List<IReadOnlyEntity>();
        private readonly IGameEngineService _engineService;

        public static EntityBag Create<T1>(IGameEngineService gameEngine) 
            where T1 : class, IComponent
        {
            return new EntityBag(gameEngine, new GenericComponentFilter<T1>());
        }

        public static EntityBag Create<T1, T2>(IGameEngineService gameEngine) 
            where T1 : class, IComponent
            where T2 : class, IComponent
        {
            return new EntityBag(gameEngine, new GenericComponentFilter<T1, T2>());
        }

        public static EntityBag Create<T1, T2, T3>(IGameEngineService gameEngine) 
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
        {
            return new EntityBag(gameEngine, new GenericComponentFilter<T1, T2, T3>());
        }

        EntityBag(IGameEngineService engineService, IComponentFilter componentFilter)
        {
            Filter = componentFilter;
            _engineService = engineService;
            OnEngineChanged(this, new EngineChangedEventArgs(engineService.Engine));
            engineService.EngineChanged += OnEngineChanged;
            engineService.EngineEvent += OnEngineEvent;
        }

        private void AddEntity(IReadOnlyEntity entity)
        {
            _contents.Add(entity);
            Added?.Invoke(this, entity);
        }

        private void RemoveEntity(IReadOnlyEntity entity)
        {
            if (_contents.Remove(entity))
            {
                Removed?.Invoke(this, entity);
            }
        }
        
        private void OnEngineChanged(object sender, EngineChangedEventArgs engineChangedEventArgs)
        {
            // Clear existing entities
            for (var i = _contents.Count - 1; i >= 0; i--)
            {
                RemoveEntity(_contents[i]);
            }

            // Add new entities
            foreach (var newEntity in engineChangedEventArgs.NewEngine.Entities)
            {
                if (Filter.Filter(newEntity))
                {
                    AddEntity(newEntity);
                }
            }
        }

        private void OnEngineEvent(object sender, GameEngineEventArgs gameEngineEventArgs)
        {
            // TODO implement a lookup by uint key
            if (gameEngineEventArgs.Event is EntityAdded)
            {
                var e = (EntityAdded) gameEngineEventArgs.Event;
                AddEntity(gameEngineEventArgs.Engine.Entities.Single(p => p.Id == e.EntityId));
            }

            if (gameEngineEventArgs.Event is EntityRemoved)
            {
                var e = (EntityRemoved) gameEngineEventArgs.Event;
                RemoveEntity(gameEngineEventArgs.Engine.Entities.Single(p => p.Id == e.EntityId));
            }
        }

        public void Dispose()
        {
            _engineService.EngineChanged -= OnEngineChanged;
            _engineService.EngineEvent -= OnEngineEvent;
        }
    }
}