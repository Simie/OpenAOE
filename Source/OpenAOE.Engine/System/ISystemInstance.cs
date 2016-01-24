using System.Collections.Generic;
using OpenAOE.Engine.Entity;

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
        IReadOnlyList<IEntity> Entities { get; }
    }
}