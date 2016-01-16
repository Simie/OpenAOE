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
        /// Return the list of entities that were added during the currently processing tick. 
        /// </summary>
        public IReadOnlyList<IEntity> AddedEntities => _addedEntities;

        /// <summary>
        /// Return the list of entities that were removed during the currently processing tick. 
        /// </summary>
        public IReadOnlyList<IEntity> RemovedEntities => _removedEntities;

        /// <summary>
        /// Return the read-only list of entities.
        /// </summary>
        public IReadOnlyList<IEntity> Entities => _entityList;

        /// <summary>
        /// Restricts access to the AddEntity method.
        /// </summary>
        [CanBeNull]
        internal IAccessGate AddEntityAccessGate;

        private readonly List<IEntity> _addedEntities = new List<IEntity>();
        private readonly List<IEntity> _removedEntities = new List<IEntity>();

        private readonly List<IEntity> _entityList = new List<IEntity>();
        private readonly Dictionary<uint, IEntity> _entityLookup = new Dictionary<uint, IEntity>();

        private readonly UniqueIdProvider _idProvider;
        private readonly IEventPoster _eventPoster;
        private readonly ILogger _logger;
        private readonly IEntityTemplateProvider _templateProvider;

        public RuntimeEntityService(UniqueIdProvider idProvider, IEventPoster eventPoster,
            ILogger logger, [CanBeNull] IEntityTemplateProvider templateProvider = null)
        {
            _idProvider = idProvider;
            _eventPoster = eventPoster;
            _logger = logger;
            _templateProvider = templateProvider;
        }

        public IEntity GetEntity(uint id)
        {
            IEntity e;

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
        
        private void InternalAddEntity(IEntity e)
        {
            _logger.Info("Adding entity with ID `{0}`.", e.Id);
            _addedEntities.Add(e);
            _eventPoster.Post(new EntityAdded(e.Id));
        }

        public IEntity CreateEntity(IEnumerable<IComponent> components)
        {
            CheckAddEntityGate();

            _logger.Info("Creating new entity from components.");

            var entity = new RuntimeEntity(_idProvider.Next(), components, _eventPoster);
            InternalAddEntity(entity);
            return entity;
        }

        public IEntity CreateEntity(string prototype)
        {
            if (_templateProvider == null)
            {
                throw new InvalidOperationException(
                    "IEntityTemplateProvider was not provided when creating RuntimeEntityService");
            }

            CheckAddEntityGate();

            _logger.Info("Creating entity with prototype `{0}`.", prototype);

            var template = _templateProvider.Get(prototype);

            var entity = new RuntimeEntity(_idProvider.Next(), template.Components.Select(p => p.Clone()), _eventPoster);
            entity.Prototype = prototype;
            InternalAddEntity(entity);
            return entity;
        }

        public void RemoveEntity(IEntity entity)
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
                _eventPoster.Post(new EntityRemoved(entity.Id));
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
    }
}
