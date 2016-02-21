using Ninject.Modules;

namespace OpenAOE.Engine
{
    /// <summary>
    /// Interface for a module that can be added to a simulation. This will
    /// be loaded into the Ninject child kernel for a simulation.
    /// </summary>
    public interface IEngineModule : INinjectModule
    {
         
    }
}