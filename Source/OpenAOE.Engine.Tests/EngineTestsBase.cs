using Ninject;
using Ninject.Extensions.Logging;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;

namespace OpenAOE.Engine.Tests
{
    public class EngineTestsBase
    {
        protected IEngineFactory Factory;
        protected IKernel Kernel = new MoqMockingKernel(new NinjectSettings(), new EngineModule());

        public EngineTestsBase()
        {
            Kernel.Bind<ILogger>().ToMock();
            Factory = Kernel.Get<IEngineFactory>();
        }
    }
}
