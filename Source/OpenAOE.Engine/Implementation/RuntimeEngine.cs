using System;
using System.Threading.Tasks;
using OpenAOE.Engine.Entity.Implementation;

namespace OpenAOE.Engine.Implementation
{
    internal sealed class RuntimeEngine : IEngine
    {
        internal RuntimeEntityService EntityService;

        public RuntimeEngine(RuntimeEntityService entityService)
        {
            EntityService = entityService;
        }

        public Task<EngineTickResult> Tick(EngineTickInput input)
        {
            throw new NotImplementedException();
        }

        public void Synchronize()
        {
            throw new NotImplementedException();
        }
    }
}
