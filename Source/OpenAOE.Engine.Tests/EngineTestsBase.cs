using Ninject;
using Ninject.Extensions.Logging;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.Tests
{
    public class EngineTestsBase
    {
        protected IKernel Kernel = new MoqMockingKernel(new NinjectSettings(), new EngineModule());

        protected IEngineFactory Factory;

        public EngineTestsBase()
        {
            Kernel.Bind<ILogger>().ToMock();
            Kernel.Bind<IEntityTemplateProvider>().ToMock();
            Factory = Kernel.Get<IEngineFactory>();
        }
    }
}
