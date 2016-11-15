using System.Collections.Generic;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System.Implementation;

namespace OpenAOE.Engine.System
{
    internal interface ISystemInstance
    {
        /// <summary>
        /// The System wrapped by this SystemInstance class
        /// </summary>
        ISystem System { get; }

        /// <summary>
        /// List of entities that this system operates on.
        /// </summary>
        IReadOnlyList<EngineEntity> Entities { get; }

        /// <summary>
        /// List of command handlers (if any) this systems contains.
        /// </summary>
        IReadOnlyList<ICommandHandler> CommandHandlers { get; }

        bool HasEntityTick { get; }

        bool HasEntityAdd { get; }

        bool HasEntityRemove { get; }

        bool HasTick { get; }
    }
}
