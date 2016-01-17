using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.NLog4;
using OpenAOE.Engine;
using OpenAOE.Engine.Entity;

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
                }, new Engine.EngineModule(), new Games.AGE2.Module(), new NLogModule());


            var log = context.Get<ILoggerFactory>().GetCurrentClassLogger();
            log.Info("Starting");

            var engineFactory = context.Get<IEngineFactory>();

            log.Info("Creating Engine");
            var engine = engineFactory.Create(new List<EntityData>(), new List<EntityTemplate>());

            for (var i = 0; i < 10; ++i)
            {
                var tick = engine.Tick(new EngineTickInput());
                tick.Start();
                tick.Wait();
                engine.Synchronize();
            }

            /*context.Bind<IDataService>().ToConstant(VersionedDataService.FromRoot(new Root()));
            var instance = context.Get<ISimulationInstance>();
            context.Unbind(typeof(IDataService));*/

            log.Info("Done");
            Console.ReadKey(true);
            log.Info("Exiting");
        }
    }
}
