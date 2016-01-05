using System;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.NLog4;
using OpenAOE.Engine;

namespace OpenAOE
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new StandardKernel(
                new NinjectSettings()
                {
                    LoadExtensions = false
                }, new EngineModule(), new Games.AGE2.Module(), new NLogModule());

            var log = context.Get<ILoggerFactory>().GetCurrentClassLogger();
            log.Info("Starting");

            /*context.Bind<IDataService>().ToConstant(VersionedDataService.FromRoot(new Root()));
            var instance = context.Get<ISimulationInstance>();
            context.Unbind(typeof(IDataService));*/

            log.Info("Done");
            Console.ReadKey(true);
            log.Info("Exiting");
        }
    }
}
