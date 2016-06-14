using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Ninject.Extensions.Logging;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Entity.Implementation
{
    internal class RuntimeEntityService : IEntityService
    {
        /// <summary>
        /// Return the read-only list of entities.
        /// </summary>
        public IReadOnlyList<EngineEntity> Entities => _entityList;

        /// <summary>
        /// Return the list of entities that were added during the currently processing tick. 
        /// </summary>
        internal IReadOnlyList<EngineEntity> AddedEntities => _addedEntities;

        /// <summary>
        /// Return the list of entities that were removed during the currently processing tick. 
        /// </summary>
        internal IReadOnlyList<EngineEntity> RemovedEntities => _removedEntities;


        /// <summary>
        /// Restricts access to the AddEntity method.
        /// </summary>
        [CanBeNull]
        internal IAccessGate AddEntityAccessGate;

        internal readonly IEntityTemplateProvider TemplateProvider;

        private readonly List<EngineEntity> _addedEntities = new List<EngineEntity>();
        private readonly List<EngineEntity> _removedEntities = new List<EngineEntity>();

        private readonly List<EngineEntity> _entityList = new List<EngineEntity>();
        private readonly Dictionary<uint, EngineEntity> _entityLookup = new Dictionary<uint, EngineEntity>();

        private readonly UniqueIdProvider _idProvider;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger _logger;

        public RuntimeEntityService(IEventDispatcher eventDispatcher,
            ILogger logger, [CanBeNull] IEntityTemplateProvider templateProvider = null, [CanBeNull] ICollection<EngineEntity> entities = null)
        {
            _eventDispatcher = eventDispatcher;
            _logger = logger;
            TemplateProvider = templateProvider;

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    _entityList.Add(entity);
                    _entityLookup.Add(entity.Id, entity);
                }
            }

            _idProvider = new UniqueIdProvider(_entityList.Count > 0 ? _entityList.Max(p => p.Id + 1) : 0);
        }

        public EngineEntity GetEntity(uint id)
        {
            EngineEntity e;

            if (!_entityLookup.TryGetValue(id, out e))
                return null;

            return e;
        }

        private void CheckAddEntityGate()
        {
            if (AddEntityAccessGate != null && !AddEntityAccessGate.TryEnter())
            {
                throw new InvalidOperationException("Entities may only be created from a synchronous update stage.");
            }
        }
        
        private void InternalAddEntity(EngineEntity e)
        {
            _logger.Info("Adding entity with ID `{0}`.", e.Id);
            _addedEntities.Add(e);
            _eventDispatcher.Post(new EntityAdded(e.Id));
        }

        public EngineEntity CreateEntity(IEnumerable<IComponent> components)
        {
            CheckAddEntityGate();

            _logger.Info("Creating new entity from components.");

            var entity = new EngineEntity(_idProvider.Next(), components, _eventDispatcher);
            InternalAddEntity(entity);
            return entity;
        }

        public EngineEntity CreateEntity(string prototype)
        {
            if (TemplateProvider == null)
            {
                throw new InvalidOperationException(
                    "EntityTemplateProvider was not provided when creating RuntimeEntityService");
            }

            CheckAddEntityGate();

            _logger.Info("Creating entity with prototype `{0}`.", prototype);

            var template = TemplateProvider.Get(prototype);

            var entity = new EngineEntity(_idProvider.Next(), template.Components.Select(p => p.Clone()), _eventDispatcher);
            entity.Prototype = prototype;
            InternalAddEntity(entity);
            return entity;
        }

        public void RemoveEntity(EngineEntity entity)
        {
            _logger.Info("Removing entity `{0}`", entity.Id);

            var didRemove = false;

            lock (_removedEntities)
            {
                if (!_removedEntities.Contains(entity))
                {
                    _removedEntities.Add(entity);
                    didRemove = true;
                }
            }

            if (didRemove)
            {
                _eventDispatcher.Post(new EntityRemoved(entity.Id));
            }
        }

        internal void CommitAdded()
        {
            _logger.Trace("Committing Added Entities");

            foreach (var a in _addedEntities)
            {
                _entityList.Add(a);
                _entityLookup.Add(a.Id, a);
            }

            _addedEntities.Clear();
        }

        internal void CommitRemoved()
        {
            _logger.Trace("Committing Removed Entities");

            foreach (var a in _removedEntities)
            {
                _entityList.Remove(a);
                _entityLookup.Remove(a.Id);
            }

            _removedEntities.Clear();
        }

        internal void CommitDirty()
        {
            _logger.Trace("Commiting dirty entities");

            foreach (var engineEntity in _entityList)
            {
                engineEntity.Commit();
            }
        }
    }
}
