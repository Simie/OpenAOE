using System;
using Ninject.Extensions.Logging;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Data.Commands;
using OpenAOE.Games.AGE2.Data.Components;

namespace OpenAOE.Games.AGE2.Systems
{
    class UnitMoveSystem : FilteredSystem<ITransform, IMovable>, Triggers.IOnEntityTick, Triggers.IOnCommand<MoveCommand>
    {
        private readonly IEntityService _entityService;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        public override string Name
        {
            get { return nameof(UnitMoveSystem); }
        }

        public UnitMoveSystem(IEntityService entityService, ITimeService timeService, ILogger logger)
        {
            _entityService = entityService;
            _timeService = timeService;
            _logger = logger;
        }

        public void OnCommand(MoveCommand command)
        {
            var targetEntity = _entityService.GetEntity(command.Target);

            if (targetEntity == null)
            {
                _logger.Error($"Target entity `{command.Target}` was not found.");
                return;
            }

            if (!targetEntity.HasComponent<IMovable>())
            {
                _logger.Error($"Target entity `{command.Target}` did not have IMovable component.");
                return;
            }

            _logger.Info($"Issuing move command to entity `{targetEntity.Id}`: {command.Position}");
            targetEntity.Modify<IWriteableMovable>().TargetPosition = command.Position;
        }

        public void OnTick(EngineEntity entity)
        {
            var movable = entity.Current<IMovable>();

            if (!movable.TargetPosition.HasValue)
            {
                return;
            }

            var transform = entity.Current<ITransform>();
            var diff = movable.TargetPosition.Value - transform.Position;

            var distance = diff.Magnitude;

            // Detect when arrived at destination
            if (distance < movable.MoveSpeed*_timeService.Step)
            {
                _logger.Info($"Entity `{entity.Id}` has arrived at destination {movable.TargetPosition}");

                entity.Modify<IWriteableTransform>().Position = movable.TargetPosition.Value;
                entity.Modify<IWriteableMovable>().TargetPosition = null;
                return;
            }

            var moveAmount = movable.MoveSpeed*_timeService.Step;
            entity.Modify<IWriteableTransform>().Position = transform.Position + moveAmount * diff.Normalized;
        }
    }
}
