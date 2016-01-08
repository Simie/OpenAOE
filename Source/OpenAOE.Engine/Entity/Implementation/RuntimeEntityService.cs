using System;
using System.Collections.Generic;
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

        private readonly List<IEntity> _addedEntities = new List<IEntity>();
        private readonly List<IEntity> _removedEntities = new List<IEntity>();

        private readonly List<IEntity> _entityList = new List<IEntity>();
        private readonly Dictionary<uint, IEntity> _entityLookup = new Dictionary<uint, IEntity>();

        private readonly UniqueIdProvider _idProvider;
        private readonly IEventPoster _eventPoster;
        private readonly ILogger _logger;
        private readonly IAccessGate _addEntityGate;

        public RuntimeEntityService(UniqueIdProvider idProvider, IAccessGate addEntityGate, IEventPoster eventPoster, ILogger logger)
        {
            _idProvider = idProvider;
            _addEntityGate = addEntityGate;
            _eventPoster = eventPoster;
            _logger = logger;
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
            if (!_addEntityGate.TryEnter())
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
            CheckAddEntityGate();

            _logger.Info("Creating entity with prototype `{0}`.", prototype);
            throw new NotImplementedException();
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
    }
}
