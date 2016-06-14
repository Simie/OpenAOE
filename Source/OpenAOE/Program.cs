using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.NLog4;
using OpenAOE.Engine;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Games.AGE2.Data.Components;

namespace OpenAOE
{
    class Program
    {
        public static List<EntityTemplate> TestTemplates = new List<EntityTemplate>()
        {
            new EntityTemplate("Unit", new List<IComponent>()
            {
                new Transform() {  },
                new Movable()
                {
                    TargetPosition = new FixVector2(10,10),
                    MoveSpeed = 1f
                }
            })
        };

        public static List<EntityData> TestData = new List<EntityData>()
        {
            new EntityData(0, new List<IComponent>()
            {
                new Transform() {  },
                new Movable()
                {
                    TargetPosition = new FixVector2(10,10),
                    MoveSpeed = 1f
                }
            })
        };


        static void Main(string[] args)
        {
            var context = new StandardKernel(
                new NinjectSettings()
                {
                    LoadExtensions = false
                }, new EngineModule(), new Games.AGE2.Module(), new NLogModule());


            var log = context.Get<ILoggerFactory>().GetCurrentClassLogger();
            log.Info("Starting");

            var engineFactory = context.Get<IEngineFactory>();

            log.Info("Creating Engine");
            var engine = engineFactory.Create(TestData, TestTemplates);

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
